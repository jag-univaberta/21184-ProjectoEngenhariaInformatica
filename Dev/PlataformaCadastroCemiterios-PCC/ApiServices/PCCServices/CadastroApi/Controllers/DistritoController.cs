using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace CadastroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DistritoController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public DistritoController(ProjectoContext context)
        {
            _context = context;
        }      

        // GET: api/Distrito/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Distrito>> GetDistrito(int id)
        {
            var Distrito = await _context.Distrito.FindAsync(id);

            if (Distrito == null)
            {
                return NotFound();
            }

            return Distrito;
        }

        [HttpGet()]
        public async Task<ActionResult<List<Distrito>>> GetDistrito()
        {
            try
            {
                List<Distrito> dados = await _context.Distrito.ToListAsync();

                return dados;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // PUT: api/Distrito
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutDistrito([FromBody] Distrito Distrito)
        {
            if (!DistritoExists(Distrito.RecId))
            {
                return NotFound();
            }
           
            _context.Entry(Distrito).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistritoExists(Distrito.RecId))
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

        // POST: api/Distrito
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Distrito>> PostDistrito([FromBody] Distrito Distrito)
        {
            _context.Distrito.Add(Distrito);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistrito", new { id = Distrito.RecId }, Distrito);
        }

        // DELETE: api/Distrito
        [HttpDelete()]
        public async Task<IActionResult> DeleteDistrito([FromBody] Distrito obj)
        {
            var Distrito = await _context.Distrito.FindAsync(obj.RecId);
            if (Distrito == null)
            {
                return NotFound();
            }

            _context.Distrito.Remove(Distrito);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistritoExists(string id)
        {
            return _context.Distrito.Any(e => e.RecId == id);
        }
    }
}
