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
    public class CemiterioController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public CemiterioController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Cemiterios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cemiterio>>> GetCemiterios()
        {
            return await _context.Cemiterios.ToListAsync();
        }

        // GET: api/Cemiterios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cemiterio>> GetCemiterio(int id)
        {
            var cemiterio = await _context.Cemiterios.FindAsync(id);

            if (cemiterio == null)
            {
                return NotFound();
            }

            return cemiterio;
        }

        // PUT: api/Cemiterios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutCemiterio([FromBody] Cemiterio cemiterio)
        {
            if (!CemiterioExists(cemiterio.RecId))
            {
                return NotFound();
            }

            _context.Entry(cemiterio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CemiterioExists(cemiterio.RecId ))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCemiterio", new { id = cemiterio.RecId }, cemiterio);
        }

        // POST: api/Cemiterios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cemiterio>> PostCemiterio([FromBody] Cemiterio cemiterio)
        {
            _context.Cemiterios.Add(cemiterio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCemiterio", new { id = cemiterio.RecId }, cemiterio);
        }

        // DELETE: api/Cemiterios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCemiterio(int id)
        {
            var cemiterio = await _context.Cemiterios.FindAsync(id);
            if (cemiterio == null)
            {
                return NotFound();
            }

            _context.Cemiterios.Remove(cemiterio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CemiterioExists(int id)
        {
            return _context.Cemiterios.Any(e => e.RecId == id);
        }
    }
}
