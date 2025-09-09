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
    public class GrupoUtilizadorController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public GrupoUtilizadorController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/GrupoUtilizador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrupoUtilizador>>> GetGrupoUtilizador()
        {
            return await _context.GrupoUtilizador.ToListAsync();
        }

        // GET: api/GrupoUtilizador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GrupoUtilizador>> GetGrupoUtilizador(int id)
        {
            var grupoUtilizador = await _context.GrupoUtilizador.FindAsync(id);

            if (grupoUtilizador == null)
            {
                return NotFound();
            }

            return grupoUtilizador;
        }

        // PUT: api/GrupoUtilizador/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrupoUtilizador(int id, GrupoUtilizador grupoUtilizador)
        {
            if (id != grupoUtilizador.RecId)
            {
                return BadRequest();
            }

            _context.Entry(grupoUtilizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GrupoUtilizadorExists(id))
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

        // POST: api/GrupoUtilizador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GrupoUtilizador>> PostGrupoUtilizador(GrupoUtilizador grupoUtilizador)
        {
            _context.GrupoUtilizador.Add(grupoUtilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrupoUtilizador", new { id = grupoUtilizador.RecId }, grupoUtilizador);
        }

        // DELETE: api/GrupoUtilizador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrupoUtilizador(int id)
        {
            var grupoUtilizador = await _context.GrupoUtilizador.FindAsync(id);
            if (grupoUtilizador == null)
            {
                return NotFound();
            }

            _context.GrupoUtilizador.Remove(grupoUtilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GrupoUtilizadorExists(int id)
        {
            return _context.GrupoUtilizador.Any(e => e.RecId == id);
        }
    }
}
