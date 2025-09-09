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
    public class TipoMovimentoController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public TipoMovimentoController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/TipoMovimento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoMovimento>> GetTipoMovimento(int id)
        {
            var tipoMovimento = await _context.TipoMovimento.FindAsync(id);

            if (tipoMovimento == null)
            {
                return NotFound();
            }

            return tipoMovimento;
        }

        [HttpGet()]
        public async Task<ActionResult<List<TipoMovimento>>> GetTipoMovimento()
        {
            try
            {
                List<TipoMovimento> dados = await _context.TipoMovimento.ToListAsync();

                return dados;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // PUT: api/TipoMovimento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutTipoMovimento([FromBody] TipoMovimento tipoMovimento)
        {
            if (!TipoMovimentoExists(tipoMovimento.RecId))
            {
                return NotFound();
            }

            _context.Entry(tipoMovimento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMovimentoExists(tipoMovimento.RecId))
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

        // POST: api/TipoMovimento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoMovimento>> PostTipoMovimento([FromBody] TipoMovimento tipoMovimento)
        {
            _context.TipoMovimento.Add(tipoMovimento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoMovimento", new { id = tipoMovimento.RecId }, tipoMovimento);
        }

        // DELETE: api/TipoMovimento
        [HttpDelete()]
        public async Task<IActionResult> DeleteTipoMovimento([FromBody] TipoMovimento obj)
        {
            var tipoMovimento = await _context.TipoMovimento.FindAsync(obj.RecId);
            if (tipoMovimento == null)
            {
                return NotFound();
            }

            _context.TipoMovimento.Remove(tipoMovimento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoMovimentoExists(int id)
        {
            return _context.TipoMovimento.Any(e => e.RecId == id);
        }
    }
}
