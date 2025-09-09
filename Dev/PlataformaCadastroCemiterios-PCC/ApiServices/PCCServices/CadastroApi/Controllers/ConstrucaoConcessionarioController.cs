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
    public class ConstrucaoConcessionarioController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public ConstrucaoConcessionarioController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/ConstrucaoConcessionario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConstrucaoConcessionario>>> GetConstrucaoConcessionario()
        {
            return await _context.ConstrucaoConcessionario.ToListAsync();
        }

        // GET: api/ConstrucaoConcessionario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConstrucaoConcessionario>> GetConstrucaoConcessionario(int id)
        {
            var construcaoConcessionario = await _context.ConstrucaoConcessionario.FindAsync(id);

            if (construcaoConcessionario == null)
            {
                return NotFound();
            }

            return construcaoConcessionario;
        }

        // PUT: api/ConstrucaoConcessionario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConstrucaoConcessionario(int id, ConstrucaoConcessionario construcaoConcessionario)
        {
            if (id != construcaoConcessionario.RecId)
            {
                return BadRequest();
            }

            _context.Entry(construcaoConcessionario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConstrucaoConcessionarioExists(id))
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

        // POST: api/ConstrucaoConcessionario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConstrucaoConcessionario>> PostConstrucaoConcessionario(ConstrucaoConcessionario construcaoConcessionario)
        {
            _context.ConstrucaoConcessionario.Add(construcaoConcessionario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConstrucaoConcessionario", new { id = construcaoConcessionario.RecId }, construcaoConcessionario);
        }

        // DELETE: api/ConstrucaoConcessionario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConstrucaoConcessionario(int id)
        {
            var construcaoConcessionario = await _context.ConstrucaoConcessionario.FindAsync(id);
            if (construcaoConcessionario == null)
            {
                return NotFound();
            }

            _context.ConstrucaoConcessionario.Remove(construcaoConcessionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConstrucaoConcessionarioExists(int id)
        {
            return _context.ConstrucaoConcessionario.Any(e => e.RecId == id);
        }
    }
}
