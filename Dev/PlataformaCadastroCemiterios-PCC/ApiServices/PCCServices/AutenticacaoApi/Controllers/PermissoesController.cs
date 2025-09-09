using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutenticacaoApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace AutenticacaoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PermissoesController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public PermissoesController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Permissoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permissoes>>> GetPermissoes()
        {
            return await _context.Permissoes.ToListAsync();
        }

        // GET: api/Permissoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Permissoes>> GetPermissoes(int id)
        {
            var permissoes = await _context.Permissoes.FindAsync(id);

            if (permissoes == null)
            {
                return NotFound();
            }

            return permissoes;
        }

        // PUT: api/Permissoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermissoes(int id, Permissoes permissoes)
        {
            if (id != permissoes.RecId)
            {
                return BadRequest();
            }

            _context.Entry(permissoes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermissoesExists(id))
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

        // POST: api/Permissoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Permissoes>> PostPermissoes(Permissoes permissoes)
        {
            _context.Permissoes.Add(permissoes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPermissoes", new { id = permissoes.RecId }, permissoes);
        }

        // DELETE: api/Permissoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermissoes(int id)
        {
            var permissoes = await _context.Permissoes.FindAsync(id);
            if (permissoes == null)
            {
                return NotFound();
            }

            _context.Permissoes.Remove(permissoes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PermissoesExists(int id)
        {
            return _context.Permissoes.Any(e => e.RecId == id);
        }
    }
}
