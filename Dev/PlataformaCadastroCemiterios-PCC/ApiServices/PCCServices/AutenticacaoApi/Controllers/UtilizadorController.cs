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
    public class UtilizadorController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public UtilizadorController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Utilizador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizador>>> GetUtilizador()
        {
            return await _context.Utilizador.ToListAsync();
        }

        // GET: api/Utilizador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizador>> GetUtilizador(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);

            if (utilizador == null)
            {
                return NotFound();
            }

            return utilizador;
        }

        // PUT: api/Utilizador/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizador(int id, Utilizador utilizador)
        {
            if (id != utilizador.RecId)
            {
                return BadRequest();
            }
            string senhaHash = PasswordHasher.HashPassword(utilizador.PalavraPasse);
            utilizador.PalavraPasse = senhaHash;
            _context.Entry(utilizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilizadorExists(id))
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

        // POST: api/Utilizador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilizador>> PostUtilizador(Utilizador utilizador)
        {
            string senhaHash = PasswordHasher.HashPassword(utilizador.PalavraPasse);
            utilizador.PalavraPasse = senhaHash;

            _context.Utilizador.Add(utilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilizador", new { id = utilizador.RecId }, utilizador);
        }

        // DELETE: api/Utilizador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizador(int id)
        {
            var utilizador = await _context.Utilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            _context.Utilizador.Remove(utilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilizadorExists(int id)
        {
            return _context.Utilizador.Any(e => e.RecId == id);
        }
    }
}
