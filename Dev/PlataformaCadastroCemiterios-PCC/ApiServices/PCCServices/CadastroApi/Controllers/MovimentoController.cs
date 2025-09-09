using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroApi.Models;
using static CadastroApi.Controllers.ConstrucaoController;
using Microsoft.AspNetCore.Authorization;

namespace CadastroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovimentoController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public MovimentoController(ProjectoContext context)
        {
            _context = context;
        }

        // GET: api/Movimento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movimento>>> GetMovimentos()
        {
            return await _context.Movimentos.ToListAsync();
        }

        // GET: api/Movimento/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovimentoItem>> GetMovimento(int id)
        {
            var movimento = await _context.Movimentos.FindAsync(id);

            if (movimento == null)
            {
                return NotFound();
            }
            Residente residente = await _context.Residente.FindAsync(movimento.ResidenteId);
            TipoMovimento tipomov = await _context.TipoMovimento.FindAsync(movimento.TipomovimentoId);
            var nodeMovimento = new MovimentoItem
            {
                RecId = movimento.RecId.ToString(),
                DataMovimento = movimento.DataMovimento.ToString(),
                ResidenteId = movimento.ResidenteId.ToString(),
                TipomovimentoId = movimento.TipomovimentoId.ToString(),
                ConstrucaodestinoId = movimento.ConstrucaodestinoId.ToString(),
                TipomovimentoNome = tipomov.Designacao.ToString(),
                ResidenteNome = residente.Nome.ToString(),
                Residente_Datanascimento = residente.DataNascimento.ToString(),
                Residente_Datafalecimento = residente.DataFalecimento.ToString(),
                Residente_Datainumacao = residente.DataInumacao.ToString(),
            };
            return nodeMovimento;
        }
        [HttpGet("PorConstrucao/{id}")]
        public async Task<ActionResult<IEnumerable<Movimento>>> GetMovimentosPorConstrucao(int id)
        {
            // Utiliza LINQ com a cláusula Where para filtrar os movimentos
            // cujo ConstrucaoId corresponde ao ID fornecido na rota.
            var movimentos = await _context.Movimentos
                                            .Where(c => c.ConstrucaodestinoId == id)
                                            .ToListAsync();

            // Se não encontrar nenhum movimento para a construção, não é um erro.
            // Retorna simplesmente uma lista vazia (HTTP 200 OK) 

            List<MovimentoItem> aux = new List<MovimentoItem>();
            // Adicionar os movimentos como filhos da construcao
            foreach (var movimento in movimentos)
            {
                // Verifica se o talhão pai existe no nosso mapa de nós

                Residente residente = await _context.Residente.FindAsync(movimento.ResidenteId);
                TipoMovimento tipomov = await _context.TipoMovimento.FindAsync(movimento.TipomovimentoId);

                var nodeMovimento = new MovimentoItem
                {
                    RecId = movimento.RecId.ToString(),
                    DataMovimento = movimento.DataMovimento.ToString(),
                    ResidenteId = movimento.ResidenteId.ToString(),
                    TipomovimentoId = movimento.TipomovimentoId.ToString(),
                    ConstrucaodestinoId = movimento.ConstrucaodestinoId.ToString(),
                    TipomovimentoNome = tipomov.Designacao.ToString(),
                    ResidenteNome = residente.Nome.ToString(),
                    Residente_Datanascimento = residente.DataNascimento.ToString(),
                    Residente_Datafalecimento = residente.DataFalecimento.ToString(),
                    Residente_Datainumacao = residente.DataInumacao.ToString(),
                };
                aux.Add(nodeMovimento);

            }


            return Ok(aux);
        }


        // PUT: api/Movimento/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimento(int id, Movimento movimento)
        {
            if (id != movimento.RecId)
            {
                return BadRequest();
            }

            _context.Entry(movimento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimentoExists(id))
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

        // POST: api/Movimento
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movimento>> PostMovimento([FromBody] InsertMovimento movimento)
        {

            Residente auxresident = new Residente();
            auxresident.Nome = movimento.Nome;
            auxresident.DataNascimento = movimento.Data_nascimento;
            auxresident.DataFalecimento=movimento.Data_falecimento;
            auxresident.DataInumacao=movimento.Data_inumacao;

            _context.Residente.Add(auxresident);
            await _context.SaveChangesAsync(); 
            TipoMovimento auxtipomovimento = await _context.TipoMovimento.FindAsync(int.Parse(movimento.Tipomovimento_Id));

            Construcao auxconstrucao= await _context.Construcao.FindAsync(int.Parse(movimento.Construcao_id));

            Movimento novomovimento = new Movimento();
            novomovimento.TipomovimentoId = auxtipomovimento.RecId;
            novomovimento.ResidenteId = auxresident.RecId;
            novomovimento.ConstrucaodestinoId = auxconstrucao.RecId;
            novomovimento.DataMovimento = movimento.Data_movimento;

            _context.Movimentos.Add(novomovimento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovimento", new { id = novomovimento.RecId }, novomovimento);
        }

        // DELETE: api/Movimento/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimento(int id)
        {
            var movimento = await _context.Movimentos.FindAsync(id);
            if (movimento == null)
            {
                return NotFound();
            }

            _context.Movimentos.Remove(movimento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimentoExists(int id)
        {
            return _context.Movimentos.Any(e => e.RecId == id);
        }
    }
}
