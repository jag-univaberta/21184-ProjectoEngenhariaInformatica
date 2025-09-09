using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIGApi.Models;

namespace SIGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartografialayerController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public CartografialayerController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Cartografialayer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cartografialayer>>> GetCartografialayers()
        {
            return await _context.Cartografialayers.ToListAsync();
        }

        // GET: api/Cartografialayer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cartografialayer>> GetCartografialayer(int id)
        {
            var cartografialayer = await _context.Cartografialayers.FindAsync(id);

            if (cartografialayer == null)
            {
                return NotFound();
            }

            return cartografialayer;
        }

        // PUT: api/Cartografialayer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartografialayer(int id, Cartografialayer cartografialayer)
        {
            if (id != cartografialayer.RecId)
            {
                return BadRequest();
            }

            _context.Entry(cartografialayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartografialayerExists(id))
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

        // POST: api/Cartografialayer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cartografialayer>> PostCartografialayer(Cartografialayer cartografialayer)
        {
            _context.Cartografialayers.Add(cartografialayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartografialayer", new { id = cartografialayer.RecId }, cartografialayer);
        }

        // DELETE: api/Cartografialayer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartografialayer(int id)
        {
            var cartografialayer = await _context.Cartografialayers.FindAsync(id);
            if (cartografialayer == null)
            {
                return NotFound();
            }

            _context.Cartografialayers.Remove(cartografialayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartografialayerExists(int id)
        {
            return _context.Cartografialayers.Any(e => e.RecId == id);
        }

        [HttpGet("PorParent/{filtro}")]
        public async Task<ActionResult<IEnumerable<Cartografialayer>>> GetCartografialayerPorParents(string filtro)
        {


            string[] filtroArray = filtro.Split(',').Select(s => s.Trim()).ToArray();

            // Agora, o Entity Framework Core consegue traduzir esta query
            // para uma SQL 'IN' clause.
            var layers = await _context.Cartografialayers
                .Where(c => filtroArray.Contains(c.Parent.ToString()))
                .ToListAsync();

            return Ok(layers);
        }
    }
}
