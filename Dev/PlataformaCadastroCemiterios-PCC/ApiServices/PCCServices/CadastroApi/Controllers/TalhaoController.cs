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
    public class TalhaoController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public TalhaoController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Talhao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Talhao>>> GetTalhao()
        {
            return await _context.Talhao.ToListAsync();
        }

        // GET: api/Talhao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Talhao>> GetTalhao(int id)
        {
            var talhao = await _context.Talhao.FindAsync(id);

            if (talhao == null)
            {
                return NotFound();
            }

            return talhao;
        }

        // PUT: api/Talhao/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutTalhao([FromBody] Talhao talhao)
        {
            if (!TalhaoExists(talhao.RecId))
            {
                return BadRequest();
            }

            _context.Entry(talhao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TalhaoExists(talhao.RecId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTalhao", new { id = talhao.RecId }, talhao);
        }

        // POST: api/Talhao
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Talhao>> PostTalhao([FromBody] Talhao talhao)
        {
            _context.Talhao.Add(talhao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTalhao", new { id = talhao.RecId }, talhao);
        }

        // DELETE: api/Talhao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTalhao(int id)
        {
            var talhao = await _context.Talhao.FindAsync(id);
            if (talhao == null)
            {
                return NotFound();
            }

            _context.Talhao.Remove(talhao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TalhaoExists(int id)
        {
            return _context.Talhao.Any(e => e.RecId == id);
        }
    }
}
