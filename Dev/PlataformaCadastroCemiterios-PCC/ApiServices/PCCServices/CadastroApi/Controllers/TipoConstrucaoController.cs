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
    public class TipoConstrucaoController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public TipoConstrucaoController(ProjectoContext context)
        {
            _context = context;
        }      

        // GET: api/TipoConstrucao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoConstrucao>> GetTipoConstrucao(int id)
        {
            var tipoConstrucao = await _context.TipoConstrucao.FindAsync(id);

            if (tipoConstrucao == null)
            {
                return NotFound();
            }

            return tipoConstrucao;
        }

        [HttpGet()]
        public async Task<ActionResult<List<TipoConstrucao>>> GetTipoConstrucao()
        {
            try
            {
                List<TipoConstrucao> dados = await _context.TipoConstrucao.ToListAsync();

                return dados;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // PUT: api/TipoConstrucao
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutTipoConstrucao([FromBody] TipoConstrucao tipoConstrucao)
        {
            if (!TipoConstrucaoExists(tipoConstrucao.RecId))
            {
                return NotFound();
            }
           
            _context.Entry(tipoConstrucao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoConstrucaoExists(tipoConstrucao.RecId))
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

        // POST: api/TipoConstrucao
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoConstrucao>> PostTipoConstrucao([FromBody] TipoConstrucao tipoConstrucao)
        {
            _context.TipoConstrucao.Add(tipoConstrucao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoConstrucao", new { id = tipoConstrucao.RecId }, tipoConstrucao);
        }

        // DELETE: api/TipoConstrucao
        [HttpDelete()]
        public async Task<IActionResult> DeleteTipoConstrucao([FromBody] TipoConstrucao obj)
        {
            var tipoConstrucao = await _context.TipoConstrucao.FindAsync(obj.RecId);
            if (tipoConstrucao == null)
            {
                return NotFound();
            }

            _context.TipoConstrucao.Remove(tipoConstrucao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoConstrucaoExists(int id)
        {
            return _context.TipoConstrucao.Any(e => e.RecId == id);
        }
    }
}
