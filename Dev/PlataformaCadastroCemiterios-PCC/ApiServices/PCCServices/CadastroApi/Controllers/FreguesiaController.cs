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
    public class FreguesiaController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public FreguesiaController(ProjectoContext context)
        {
            _context = context;
        }      

        //// GET: api/Freguesia/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Freguesia>> GetFreguesia(int id)
        //{
        //    var Freguesia = await _context.Freguesia.FindAsync(id);

        //    if (Freguesia == null)
        //    {
        //        return NotFound();
        //    }

        //    return Freguesia;
        //}
        [HttpGet("{cod_concelho}")]
        public async Task<ActionResult<List<Freguesia>>> GetFreguesia(string cod_concelho)
        {
            var freguesias = await _context.Freguesia
                .Where(c => c.Co == cod_concelho)
                .ToListAsync();

            if (!freguesias.Any())
            {
                return NotFound();
            }

            return freguesias;
        }
        //[HttpGet()]
        //public async Task<ActionResult<List<Freguesia>>> GetFreguesia()
        //{
        //    try
        //    {
        //        List<Freguesia> dados = await _context.Freguesia.ToListAsync();

        //        return dados;
        //    }
        //    catch (Exception)
        //    {
        //        return NotFound();
        //    }
        //}

        // PUT: api/Freguesia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutFreguesia([FromBody] Freguesia Freguesia)
        {
            if (!FreguesiaExists(Freguesia.RecId))
            {
                return NotFound();
            }
           
            _context.Entry(Freguesia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FreguesiaExists(Freguesia.RecId))
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

        // POST: api/Freguesia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Freguesia>> PostFreguesia([FromBody] Freguesia Freguesia)
        {
            _context.Freguesia.Add(Freguesia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFreguesia", new { id = Freguesia.RecId }, Freguesia);
        }

        // DELETE: api/Freguesia
        [HttpDelete()]
        public async Task<IActionResult> DeleteFreguesia([FromBody] Freguesia obj)
        {
            var Freguesia = await _context.Freguesia.FindAsync(obj.RecId);
            if (Freguesia == null)
            {
                return NotFound();
            }

            _context.Freguesia.Remove(Freguesia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FreguesiaExists(string id)
        {
            return _context.Freguesia.Any(e => e.RecId == id);
        }
    }
}
