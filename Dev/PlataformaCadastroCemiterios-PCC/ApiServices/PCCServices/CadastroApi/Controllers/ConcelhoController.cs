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
    public class ConcelhoController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public ConcelhoController(ProjectoContext context)
        {
            _context = context;
        }      

        //// GET: api/Concelho/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Concelho>> GetConcelho(string id)
        //{
        //    var Concelho = await _context.Concelho.FindAsync(id);

        //    if (Concelho == null)
        //    {
        //        return NotFound();
        //    }

        //    return Concelho;
        //}
        [HttpGet("{cod_distrito}")]
        public async Task<ActionResult<List<Concelho>>> GetConcelho(string cod_distrito)
        {
            var concelhos = await _context.Concelho
                .Where(c => c.Di == cod_distrito)
                .ToListAsync();

            if (!concelhos.Any())
            {
                return NotFound();
            }

            return concelhos;
        }
        //[HttpGet()]
        //public async Task<ActionResult<List<Concelho>>> GetConcelho()
        //{
        //    try
        //    {
        //        List<Concelho> dados = await _context.Concelho.ToListAsync();

        //        return dados;
        //    }
        //    catch (Exception)
        //    {
        //        return NotFound();
        //    }
        //}

        // PUT: api/Concelho
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutConcelho([FromBody] Concelho Concelho)
        {
            if (!ConcelhoExists(Concelho.RecId))
            {
                return NotFound();
            }
           
            _context.Entry(Concelho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConcelhoExists(Concelho.RecId))
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

        // POST: api/Concelho
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Concelho>> PostConcelho([FromBody] Concelho Concelho)
        {
            _context.Concelho.Add(Concelho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConcelho", new { id = Concelho.RecId }, Concelho);
        }

        // DELETE: api/Concelho
        [HttpDelete()]
        public async Task<IActionResult> DeleteConcelho([FromBody] Concelho obj)
        {
            var Concelho = await _context.Concelho.FindAsync(obj.RecId);
            if (Concelho == null)
            {
                return NotFound();
            }

            _context.Concelho.Remove(Concelho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConcelhoExists(string id)
        {
            return _context.Concelho.Any(e => e.RecId == id);
        }
    }
}
