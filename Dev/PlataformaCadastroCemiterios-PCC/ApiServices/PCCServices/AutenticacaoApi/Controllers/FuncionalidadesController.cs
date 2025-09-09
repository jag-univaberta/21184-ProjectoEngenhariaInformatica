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
    public class FuncionalidadesController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public FuncionalidadesController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Funcionalidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionalidade>>> GetFuncionalidades()
        {
            return await _context.Funcionalidade.ToListAsync();
        }

        // GET: api/Funcionalidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionalidade>> GetFuncionalidade(int id)
        {
            var funcionalidade = await _context.Funcionalidade.FindAsync(id);

            if (funcionalidade == null)
            {
                return NotFound();
            }

            return funcionalidade;
        }

        // PUT: api/Funcionalidades/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuncionalidade(int id, Funcionalidade funcionalidade)
        {
            if (id != funcionalidade.CodigoFuncionalidade)
            {
                return BadRequest();
            }

            _context.Entry(funcionalidade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionalidadeExists(id))
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

        // POST: api/Funcionalidades
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Funcionalidade>> PostFuncionalidade(Funcionalidade funcionalidade)
        {
            _context.Funcionalidade.Add(funcionalidade);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FuncionalidadeExists(funcionalidade.CodigoFuncionalidade))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFuncionalidade", new { id = funcionalidade.CodigoFuncionalidade }, funcionalidade);
        }

        // DELETE: api/Funcionalidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionalidade(int id)
        {
            var funcionalidade = await _context.Funcionalidade.FindAsync(id);
            if (funcionalidade == null)
            {
                return NotFound();
            }

            _context.Funcionalidade.Remove(funcionalidade);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FuncionalidadeExists(int id)
        {
            return _context.Funcionalidade.Any(e => e.CodigoFuncionalidade == id);
        }
    }
}
