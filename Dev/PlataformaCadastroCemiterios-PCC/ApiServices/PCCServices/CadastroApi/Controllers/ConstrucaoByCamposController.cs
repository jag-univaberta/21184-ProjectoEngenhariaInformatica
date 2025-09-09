using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroApi.Models;
using NetTopologySuite.IO;
using NetTopologySuite.Geometries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax; // Necessário para a classe Geometry 
using CsvHelper;
using CsvHelper.Configuration;
using System.Configuration;
using System.Text;
using System.Globalization;

namespace CadastroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConstrucaoByCamposController : ControllerBase
    {
        private readonly ProjectoContext _context;
        private readonly IConfiguration _configuration;

        public class ConstrucaoItem
        {
            public string Id { get; set; }
            public string Rec_id { get; set; }
            public string Designacao { get; set; }
            public string Descricao { get; set; }
            public string Tipo { get; set; }
            public string Numero { get; set; }
        }

        public class ConstrucaoCampos
        {
            public string designacao { get; set; }
            public string nome { get; set; }
            public string nif { get; set; }
            public int tipoconstrucao { get; set; } 
        }

        public partial class ConstrucoesResposta
        {
            public string Id { get; set; } = null!;

            public string? Designacao { get; set; }

            public string? Construcao_centroid { get; set; }
            public string? Construcao_bbox { get; set; }
            public string? Locpin { get; set; }
            public string? Open { get; set; }
            public string? Nif { get; set; }
            public string? Nome { get; set; } 

            public string? Tipoconstrucao { get; set; }
            public string? NrMovimentos { get; set; }
            public string? NrDocumentos { get; set; }
            public string? X { get; set; }
            public string? Y { get; set; } 
        }


        public class ConstrucaoOut
        {
            public int RecId { get; set; }

            public int TipoconstrucaoId { get; set; }

            public string Designacao { get; set; } = null!;

            public int TalhaoId { get; set; }

            public string? GeometriaWKT { get; set; }
             
            public string? Centroid { get; set; }

            public string? Area { get; set; } 

            public string? Perimetro { get; set; }

            public string? Mbr { get; set; }
        }

        // --- DTO (Data Transfer Object) ---
        // Um objeto simples para receber a string WKT do corpo do pedido (request body).
        public class GeometriaUpdateDto
        {
            public string Wkt { get; set; }
        }

        public class ConstrucoesResult
        {
            public List<ConstrucoesResposta> Dados { get; set; }
            public string CaminhoFicheiro { get; set; } // Adicionar a propriedade CaminhoPdf
        }

        public ConstrucaoByCamposController(ProjectoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost] 
        public async Task<ActionResult<ConstrucoesResult>> PostConstrucaoByCampos([FromBody] ConstrucaoCampos dto)
        {


            // Comece a query com a tabela Construcao.
            var query = _context.Construcao.AsQueryable();

            // Junte a tabela de Concessionarios para poder pesquisar por nome e nif.
            // Use um Left Join para incluir construções sem concessionário.
            var construcoesComConcessionarios = from c in query
                                                join cc in _context.ConstrucaoConcessionario on c.RecId equals cc.ConstrucaoId into ccc
                                                from ccd in ccc.DefaultIfEmpty() // Left Join
                                                join cs in _context.Concessionarios on ccd.ConcessionarioId equals cs.RecId into ccs
                                                from csc in ccs.DefaultIfEmpty() // Left Join
                                                select new { Construcao = c, Concessionario = csc };

            // Adicione as cláusulas Where apenas se os parâmetros tiverem dados.

            // 1. Pesquisa por Designacao
            if (!string.IsNullOrEmpty(dto.designacao))
            {
                // Usa Contains para pesquisa parcial (ex: "moradia" em "Moradia T3")
                construcoesComConcessionarios = construcoesComConcessionarios.Where(x => x.Construcao.Designacao.Contains(dto.designacao));
            }

            // 2. Pesquisa por Tipo de Construcao
            if (dto.tipoconstrucao>0)
            {
                // Usa o valor exato, pois é um ID.
                construcoesComConcessionarios = construcoesComConcessionarios.Where(x => x.Construcao.TipoconstrucaoId == dto.tipoconstrucao);
            }

            // 3. Pesquisa por Nome do Concessionario
            if (!string.IsNullOrEmpty(dto.nome))
            {
                // Verifica se a propriedade Nome do Concessionario não é nula antes de usar Contains
                construcoesComConcessionarios = construcoesComConcessionarios.Where(x => x.Concessionario != null && x.Concessionario.Nome.Contains(dto.nome));
            }

            // 4. Pesquisa por NIF do Concessionario
            if (!string.IsNullOrEmpty(dto.nif))
            {
                // Verifica se a propriedade Nif do Concessionario não é nula
                construcoesComConcessionarios = construcoesComConcessionarios.Where(x => x.Concessionario != null && x.Concessionario.Nif == dto.nif);
            }

            // Agora, retorne apenas os objetos 'Construcao' e remova os duplicados se a pesquisa por concessionário
            // tiver encontrado múltiplos registos para a mesma construção.
            var resultadosFinais = await construcoesComConcessionarios
                                            .Select(x => x.Construcao)
                                            .Distinct()
                                            .ToListAsync();

            
            List<ConstrucoesResposta> dados = new List<ConstrucoesResposta>();

            ConstrucoesResposta objeto_resposta = new ConstrucoesResposta();
             
                int i = 0;
            // Use um ciclo foreach para iterar sobre cada Construcao na lista
            foreach (var construcao in resultadosFinais)
            {
                // A partir daqui, você pode aceder às propriedades de cada objeto 'construcao'.
                Console.WriteLine($"RecId: {construcao.RecId}, Designacao: {construcao.Designacao}");
                // Você pode fazer qualquer processamento ou usar os dados aqui.
                objeto_resposta = new ConstrucoesResposta();

                objeto_resposta.Id = construcao.RecId.ToString();
                objeto_resposta.Designacao = construcao.Designacao.ToString();
                if (construcao.Geometria != null) {  
                    if (construcao.Geometria.Centroid.ToString() != "")
                    {
                        objeto_resposta.Construcao_centroid = construcao.Geometria.Centroid.AsText().ToString();
                        objeto_resposta.Construcao_bbox = construcao.Geometria.Boundary.AsText().ToString();
                        objeto_resposta.Locpin = "<i class='fa fa-map-marker'></i>";
                        objeto_resposta.X = construcao.Geometria.Centroid.Coordinate.X.ToString();
                        objeto_resposta.Y = construcao.Geometria.Centroid.Coordinate.Y.ToString();

                    }
                    else
                    {
                        objeto_resposta.Construcao_centroid = "";
                        objeto_resposta.Construcao_bbox = "";
                        objeto_resposta.Locpin = "";
                        objeto_resposta.X = "";
                        objeto_resposta.Y = "";

                    }
                }
                else
                {
                    objeto_resposta.Construcao_centroid = "";
                    objeto_resposta.Construcao_bbox = "";
                    objeto_resposta.Locpin = "";
                    objeto_resposta.X = "";
                    objeto_resposta.Y = "";
                }
                objeto_resposta.Open = "<i class='fa fa-folder-open'></i>";

                var concessionarioreg = await _context.ConstrucaoConcessionario
                .Where(c => c.ConstrucaoId == construcao.RecId)
                .ToListAsync();

                if (concessionarioreg.Any()) // ou concessionario.Count > 0
                {
                    var primeiroRegisto = concessionarioreg.FirstOrDefault();

                    var concessionario = await _context.Concessionarios.FindAsync(primeiroRegisto.ConcessionarioId);
                    if (concessionario == null)
                    {
                        objeto_resposta.Nif = concessionario.Nif;

                        objeto_resposta.Nome = concessionario.Nome;
                    } else
                    {
                        objeto_resposta.Nif = "";
                        objeto_resposta.Nome = "";
                    }
                }
                else
                {
                    objeto_resposta.Nif = "";
                    objeto_resposta.Nome = "";
                }
                var tipoconst = await _context.TipoConstrucao.FindAsync(construcao.TipoconstrucaoId);
                if (tipoconst != null) { 
                    objeto_resposta.Tipoconstrucao = tipoconst.Designacao;
                } else { 
                    objeto_resposta.Tipoconstrucao = ""; 
                }
            
                dados.Add(objeto_resposta);

                i = i + 1;
            }
             

            byte[] pdfBytes = GerarCsvConstrucoes(dados); 

            // 1. Definir o diretório de armazenamento
            string diretorio = _configuration["Impressoes:TempDir"];

            // 2. Gerar um nome de arquivo único
            string nomeArquivo = Guid.NewGuid().ToString() + ".csv";

            // 3. Salvar o PDF no servidor
            string caminhoArquivo = Path.Combine(diretorio, nomeArquivo);
            System.IO.File.WriteAllBytes(caminhoArquivo, pdfBytes);

            string caminhoRelativo = Path.Combine("Resources/Temp", nomeArquivo); 

            string urlBase = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            string urlDownload = $"{urlBase}/{caminhoRelativo}";
            var result = new ConstrucoesResult
            {
                Dados = dados,
                CaminhoFicheiro = nomeArquivo  // Adicionar o caminho do ficheiro ao resultado
            };

            return Ok(result);

            
        }
        // Classe de mapeamento
        public sealed class ConstrucoesRespostaMap : ClassMap<ConstrucoesResposta>
        {
            public ConstrucoesRespostaMap()
            {
                Map(m => m.Id).Name("Construção Id");
                Map(m => m.Nif).Name("NIF Concessionário");
                Map(m => m.Nome).Name("Nome Concessionário"); 
                Map(m => m.Tipoconstrucao).Name("Tipo Construção"); 
                Map(m => m.X).Name("Centroid X");
                Map(m => m.Y).Name("Centroid Y");
            }
        }
        private byte[] GerarCsvConstrucoes(List<ConstrucoesResposta> dados)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    // Configurar o mapeamento das propriedades (opcional)
                    csvWriter.Context.RegisterClassMap<ConstrucoesRespostaMap>();

                    // Escrever os dados no CSV
                    csvWriter.WriteRecords(dados);

                    // Fechar o writer para garantir que todos os dados sejam escritos no stream
                    streamWriter.Flush();

                    // Retornar o CSV em bytes
                    return memoryStream.ToArray();
                }

            }
            catch (Exception ex)
            {

                return null;
            }

        }
     
        [HttpGet("{filename}")]
        public ActionResult GetConstrucaoByCampos(string filename)
        {
            try
            {
                // 1. Definir o diretório de armazenamento
                string diretorio = _configuration["Impressoes:TempDir"];
                // 3. Salvar o PDF no servidor
                string caminhoArquivo = Path.Combine(diretorio, filename);

                // Return the byte[] result
                byte[] documentData = ReadFileAsByteArray(caminhoArquivo);  // Replace this with your actual implementation


                // Set the content type and file name
                string contentType = "application/octet-stream"; // Adjust the content type according to your file type

                // Return the file as the response
                return File(documentData, contentType, filename);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during file reading
                Console.WriteLine("Error reading file: " + ex.Message);
                return null;
            }

        }

        private byte[] ReadFileAsByteArray(string filePath)
        {
            byte[] fileBytes = null;

            try
            {
                fileBytes = System.IO.File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occurred during file reading
                Console.WriteLine("Error reading file: " + ex.Message);
            }

            return fileBytes;
        }

    }
}
