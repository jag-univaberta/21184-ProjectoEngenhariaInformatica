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
using Microsoft.AspNetCore.Authorization; // Necessário para a classe Geometry 

namespace CadastroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConstrucaoController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public class ConstrucaoItem
        {
            public string Id { get; set; }
            public string Rec_id { get; set; }
            public string Nome { get; set; }
            public string Descricao { get; set; }
            public string Tipo { get; set; }
            public string Numero { get; set; } 
             
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

        public ConstrucaoController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Construcao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Construcao>>> GetConstrucao()
        {
            return await _context.Construcao.ToListAsync();
        }

        // GET: api/Construcao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConstrucaoOut>> GetConstrucao(int id)
        {
            var construcao = await _context.Construcao.FindAsync(id);

            if (construcao == null)
            {
                return NotFound();
            }
            ConstrucaoOut obj = new ConstrucaoOut
            {
                RecId = construcao.RecId,
                Designacao = construcao.Designacao, 
                TalhaoId = construcao.TalhaoId,
                TipoconstrucaoId = construcao.TipoconstrucaoId,
                
            };
            // Add a null check for construcao.Geometria
            if (construcao.Geometria != null)
            {
                obj.GeometriaWKT = construcao.Geometria.AsText().ToString();
                obj.Centroid = construcao.Geometria.Centroid.AsText().ToString();
                obj.Area = construcao.Geometria.Area.ToString();
                obj.Perimetro = construcao.Geometria.Length.ToString();
                obj.Mbr = construcao.Geometria.Envelope.AsText().ToString();
            }
            else
            {
                // Provide default values if Geometria is null
                obj.GeometriaWKT = "";
                obj.Centroid = "";
                obj.Area = "0";
                obj.Perimetro = "0";
                obj.Mbr = "";
            }
            return obj;
        }
        [HttpGet("PorTalhao/{id}")]
        public async Task<ActionResult<IEnumerable<Construcao>>> GetConstrucoesPorTalhao(int id)
        {
            // Utiliza LINQ com a cláusula Where para filtrar as construções
            // cujo TalhaoId corresponde ao ID fornecido na rota.
            var construcoes = await _context.Construcao
                                            .Where(c => c.TalhaoId == id)
                                            .ToListAsync();

            // Se não encontrar nenhuma construção para o talhão, não é um erro.
            // Retorna simplesmente uma lista vazia (HTTP 200 OK), o que é a prática
            // recomendada para este tipo de consulta.

            List<ConstrucaoItem> aux = new List<ConstrucaoItem>();
            // Adicionar as construções como filhas dos seus respetivos talhões
            foreach (var construcao in construcoes)
            {
                // Verifica se o talhão pai existe no nosso mapa de nós
                 
                var nodeConstrucao = new ConstrucaoItem
                {
                    Id = construcao.RecId.ToString(),
                    Rec_id = construcao.RecId.ToString(), 
                    Nome = construcao.Designacao,
                    Descricao = construcao.Designacao,
                    Tipo = "construcao",
                    Numero = construcao.RecId.ToString(),
                };
                aux.Add(nodeConstrucao);
                
            }


            return Ok(aux);
        }

        // PUT: api/Construcao/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConstrucao(int id, Construcao construcao)
        {
            if (id != construcao.RecId)
            {
                return BadRequest();
            }

            _context.Entry(construcao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConstrucaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Construcao
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Construcao>> PostConstrucao(Construcao construcao)
        {
            _context.Construcao.Add(construcao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConstrucao", new { id = construcao.RecId }, construcao);
        }

        // DELETE: api/Construcao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConstrucao(int id)
        {
            var construcao = await _context.Construcao.FindAsync(id);
            if (construcao == null)
            {
                return NotFound();
            }

            _context.Construcao.Remove(construcao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // --- NOVO MÉTODO PARA ATUALIZAR A GEOMETRIA ---
        // PUT: api/Construcao/5/geometria
        [HttpPut("{id}/geometria")]
        public async Task<IActionResult> UpdateConstrucaoGeometria(int id, [FromBody] GeometriaUpdateDto geometriaDto)
        {
            // Validação básica do input
            if (geometriaDto == null || string.IsNullOrWhiteSpace(geometriaDto.Wkt))
            {
                return BadRequest("O WKT da geometria não pode ser nulo ou vazio.");
            }

            // 1. Encontrar a construção existente na base de dados pelo seu ID.
            var construcao = await _context.Construcao.FindAsync(id);

            if (construcao == null)
            {
                // Se a construção não for encontrada, retorna um erro 404 Not Found.
                return NotFound($"Construção com o ID {id} não encontrada.");
            }

            try
            {
                // 2. Converter a string WKT para um objeto Geometry.
                // O WKTReader é a classe da NetTopologySuite responsável por esta conversão.
                var reader = new WKTReader();
                var novaGeometria = reader.Read(geometriaDto.Wkt);

                // Opcional: Definir o SRID (Spatial Reference System Identifier) se necessário.
                // Por exemplo, 4326 para WGS 84.
                // novaGeometria.SRID = 4326;

                // 3. Atualizar a propriedade da geometria no objeto da construção.
                construcao.Geometria = novaGeometria;

                // 4. Guardar as alterações na base de dados.
                await _context.SaveChangesAsync();

                // Retorna uma resposta 204 No Content, que é o padrão para
                // operações de atualização bem-sucedidas que não retornam dados.
                return NoContent();
            }
            catch (Exception ex)
            {
                // Se o WKT for inválido, o `reader.Read` irá lançar uma exceção.
                // Capturamos o erro e retornamos uma resposta 400 Bad Request.
                return BadRequest($"Erro ao processar o WKT da geometria: {ex.Message}");
            }
        }

        private bool ConstrucaoExists(int id)
        {
            return _context.Construcao.Any(e => e.RecId == id);
        }
    }
}
