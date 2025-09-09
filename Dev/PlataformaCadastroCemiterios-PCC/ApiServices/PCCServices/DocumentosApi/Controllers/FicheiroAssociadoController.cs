using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DocumentosApi.Models;
using static DocumentosApi.Controllers.FicheiroAssociadoController;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;

namespace DocumentosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FicheiroAssociadoController : ControllerBase
    {
        private readonly ProjectoContext _context;
        private readonly IConfiguration _configuration;

        public FicheiroAssociadoController(ProjectoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/FicheiroAssociado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FicheiroAssociado>>> GetFicheiroAssociado()
        {
            return await _context.FicheiroAssociado.ToListAsync();
        }

        // GET: api/FicheiroAssociado/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetFicheiroAssociado(int id)
        {
            var ficheiroAssociado = await _context.FicheiroAssociado.FindAsync(id);

            if (ficheiroAssociado == null)
            {
                return NotFound();
            }

            /*return ficheiroAssociado;*/

            byte[] file = System.IO.File.ReadAllBytes(ficheiroAssociado.NomeAtribuido);
            // 2.Determinar o tipo de conteúdo(Content Type)
            string contentType;
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(ficheiroAssociado.NomeDocumento, out contentType))
            {
                // Usar um tipo de conteúdo genérico se o tipo MIME não for encontrado
                contentType = "application/octet-stream";
            }
            string fileName = ficheiroAssociado.NomeDocumento;
            //res = new byte[imageData.Length];
            return File(file, contentType, fileName);

        }
        [HttpGet("construcao/{id}")]
        public async Task<ActionResult<List<FicheiroAssociado>>> GetFicheiroAssociadobyConstrucao(int id)
        {
            var ficheiroAssociado = await _context.FicheiroAssociado
                .Where(c => c.CodigoAssociacao == id && c.TipoAssociacao=="construcao")
                .ToListAsync();

            if (!ficheiroAssociado.Any())
            {
                List<FicheiroAssociado> aux = new List<FicheiroAssociado>();

                return aux;
            }

            return ficheiroAssociado;
        }
        [HttpGet("movimento/{id}")]
        public async Task<ActionResult<List<FicheiroAssociado>>> GetFicheiroAssociadobyMovimento(int id)
        {
            var ficheiroAssociado = await _context.FicheiroAssociado
                .Where(c => c.CodigoAssociacao == id && c.TipoAssociacao == "movimento")
                .ToListAsync();

            if (!ficheiroAssociado.Any())
            {
                List<FicheiroAssociado> aux = new List<FicheiroAssociado>();

                return aux;
            }

            return ficheiroAssociado;
        }
        // PUT: api/FicheiroAssociado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFicheiroAssociado(int id, FicheiroAssociado ficheiroAssociado)
        {
            if (id != ficheiroAssociado.RecId)
            {
                return BadRequest();
            }

            _context.Entry(ficheiroAssociado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FicheiroAssociadoExists(id))
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

        public class InfoFicheiros
        {
            public IList<IFormFile> File { get; set; }

            public string Descricao { get; set; }

            public string Dataficheiro { get; set; }

            public string Observacao { get; set; }

            public string Userid { get; set; }

            public string Tipoassociacao { get; set; }

            public int Codigoassociacao { get; set; } 
        }

        // POST: api/FicheiroAssociado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FicheiroAssociado>> PostFicheiroAssociado([FromForm] InfoFicheiros ficheiroAssociado)
        {

            string RepositorioDocumentos = _configuration["RepositorioDocumentos"];


            string filename = ficheiroAssociado.File[0].FileName;
            string filetype = ficheiroAssociado.File[0].ContentType;
            byte[] fileData;

            // Read file data from the incoming request
            using (var ms = new MemoryStream())
            {
                ficheiroAssociado.File[0].CopyTo(ms);
                fileData = ms.ToArray();
            }

            // Criar um nome de ficheiro único
            string fileExtension = Path.GetExtension(filename);
            string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            // Construir o caminho completo do ficheiro no repositório
            string filePath = Path.Combine(RepositorioDocumentos, uniqueFileName);

            // Guardar o ficheiro no disco
            try
            {
                System.IO.File.WriteAllBytes(filePath, fileData);
                Console.WriteLine($"Ficheiro guardado com sucesso em: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao guardar o ficheiro: {ex.Message}");
                // Pode adicionar aqui mais lógica para lidar com o erro
            }

            FicheiroAssociado auxficheiroAssociado = new FicheiroAssociado();

            auxficheiroAssociado.NomeDocumento = filename;
            auxficheiroAssociado.DescricaoDocumento = ficheiroAssociado.Descricao;
            auxficheiroAssociado.ObservacaoDocumento = ficheiroAssociado.Observacao ;
            auxficheiroAssociado.TipoAssociacao = ficheiroAssociado.Tipoassociacao;
            auxficheiroAssociado.CodigoAssociacao = ficheiroAssociado.Codigoassociacao;
            auxficheiroAssociado.NomeAtribuido = filePath;
            auxficheiroAssociado.Datahoraupload = ficheiroAssociado.Dataficheiro;
            try
            {
                _context.FicheiroAssociado.Add(auxficheiroAssociado);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                throw ex;
            }


            return CreatedAtAction("GetFicheiroAssociado", new { id = auxficheiroAssociado.RecId }, auxficheiroAssociado);
        }

        // DELETE: api/FicheiroAssociado/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFicheiroAssociado(int id)
        {
            var ficheiroAssociado = await _context.FicheiroAssociado.FindAsync(id);
            if (ficheiroAssociado == null)
            {
                return NotFound();
            }

            _context.FicheiroAssociado.Remove(ficheiroAssociado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FicheiroAssociadoExists(int id)
        {
            return _context.FicheiroAssociado.Any(e => e.RecId == id);
        }
    }
}
