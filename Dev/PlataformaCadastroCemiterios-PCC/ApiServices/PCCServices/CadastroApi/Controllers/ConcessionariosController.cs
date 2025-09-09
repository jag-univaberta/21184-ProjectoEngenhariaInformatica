using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroApi.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace CadastroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConcessionariosController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public ConcessionariosController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Concessionarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Concessionario>>> GetConcessionarios()
        {
            return await _context.Concessionarios.ToListAsync();
        }

        // GET: api/Concessionarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Concessionario>> GetConcessionario(int id)
        {
            var concessionario = await _context.Concessionarios.FindAsync(id);

            if (concessionario == null)
            {
                return NotFound();
            }

            return concessionario;
        }
        [HttpGet("nif/{nif}")]
        public async Task<ActionResult<List<Concessionario>>> GetConcessionariobyNif(string nif)
        {
            var concessionario = await _context.Concessionarios
                .Where(c => c.Nif == nif)
                .ToListAsync();

            if (!concessionario.Any())
            {
                return NotFound();
            }

            return concessionario;
        }
        // PUT: api/Concessionarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutConcessionario([FromBody] Concessionario concessionario)
        {
            if (!ConcessionarioExists(concessionario.RecId))
            {
                return NotFound();
            }

            _context.Entry(concessionario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConcessionarioExists(concessionario.RecId))
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

        // POST: api/Concessionarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Concessionario>> PostConcessionario([FromBody]  Concessionario concessionario)
        {
            _context.Concessionarios.Add(concessionario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConcessionario", new { id = concessionario.RecId }, concessionario);
        } 

        // DELETE: api/Concessionarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConcessionario(int id)
        {
            var concessionario = await _context.Concessionarios.FindAsync(id);
            if (concessionario == null)
            {
                return NotFound();
            }

            _context.Concessionarios.Remove(concessionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConcessionarioExists(int id)
        {
            return _context.Concessionarios.Any(e => e.RecId == id);
        }
    }
}
