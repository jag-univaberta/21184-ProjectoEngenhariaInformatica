using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Index.HPRtree;
using CadastroApi.Models;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CadastroApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CadastroEstadoAppController : ControllerBase
    {
        private readonly ProjectoContext _context;
        private readonly IConfiguration _configuration;
         

        public CadastroEstadoAppController(ProjectoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        } 
         
        [HttpGet("{Utilizador_id}")]
        public async Task<ActionResult<AppEstado>> GetCadastroEstadoApp(int Utilizador_id)
        {
            var estado = await _context.AppEstado.FindAsync(Utilizador_id);

            if (estado == null)
            {
                return NotFound();
            }

            return estado;
        }
       
        // PUT: api/Cemiterios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutCadastroEstadoApp([FromBody] AppEstado estado)
        {
            if (!AppEstadoExists(estado.Utilizador_id))
            {
                //return NotFound();
                _context.AppEstado.Add(estado);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAppEstado", new { Utilizador_id = estado.Utilizador_id }, estado);
            }
            else
            {
                _context.Entry(estado).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppEstadoExists(estado.Utilizador_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetAppEstado", new { Utilizador_id = estado.Utilizador_id }, estado);
            }
        }
        [HttpPost]
        public async Task<ActionResult<AppEstado>> PostCadastroEstadoApp([FromBody] AppEstado estado)
        {
            if (!AppEstadoExists(estado.Utilizador_id))
            {
                _context.AppEstado.Add(estado);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAppEstado", new { Utilizador_id = estado.Utilizador_id }, estado);
            } else
            {
                _context.Entry(estado).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppEstadoExists(estado.Utilizador_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetAppEstado", new { Utilizador_id = estado.Utilizador_id }, estado);

            }
        }
         
        [HttpDelete()]
        public async Task<IActionResult> DeleteCadastroEstadoApp([FromBody] AppEstado obj)
        {
            var estado = await _context.AppEstado.FindAsync(obj.Utilizador_id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.AppEstado.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool AppEstadoExists(int userid)
        {
            return _context.AppEstado.Any(e => e.Utilizador_id == userid);
        }
    }
}
