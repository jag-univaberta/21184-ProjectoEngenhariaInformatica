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
    public class CartografiaController : ControllerBase
    {
        private readonly ProjectoContext _context;

        public CartografiaController(ProjectoContext context)
        {
            _context = context;
        }

        // Método auxiliar para verificar existência (usado no tratamento de concorrência)
        private bool CartografiaExists(int id)
        {
            return _context.Cartografia.Any(e => e.RecId == id);
        }


        // GET: api/Cartografia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cartografia>>> GetCartografia()
        {
            return await _context.Cartografia.ToListAsync();
        }

        // GET: api/Cartografia/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cartografia>> GetCartografia(int id)
        {
            // É uma boa prática incluir as Layers relacionadas ao buscar uma Cartografia
            var cartografia = await _context.Cartografia
                                            .Include(c => c.Layers) // Inclui as layers associadas
                                            .FirstOrDefaultAsync(c => c.RecId == id);


            //var cartografia = await _context.Cartografia.FindAsync(id);

            if (cartografia == null)
            {
                return NotFound();
            }  

            return cartografia;
        }

        // PUT: api/Cartografia/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartografia(int id, Cartografia cartografia)
        {
            // 1. Validação do ID da rota vs ID do payload
            if (id != cartografia.RecId)
            {
                return BadRequest("O RecId na URL deve corresponder ao RecId no corpo da requisição.");
            }

            // 2. Validação do Modelo (DataAnnotations)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartografiaExistente = await _context.Cartografia
                                              .Include(c => c.Layers) // Carregar layers existentes para comparação
                                              .FirstOrDefaultAsync(c => c.RecId == id);

            if (cartografiaExistente == null)
            {
                return NotFound($"Cartografia com RecId {id} não encontrada para atualização.");
            }

            // Atualizar propriedades escalares da Cartografia existente com os valores do payload
            _context.Entry(cartografiaExistente).CurrentValues.SetValues(cartografia);

            // Gerenciar a coleção de Layers baseando-se nos nomes das layers

            // Nomes das layers presentes no payload (ignorando nulos ou vazios)
            var nomesLayersPayload = cartografia.Layers
                                         .Select(l => l.Layer)
                                         .Where(name => !string.IsNullOrEmpty(name))
                                         .ToHashSet();

            // Layers existentes no banco de dados para esta cartografia
            var layersExistentesDb = cartografiaExistente.Layers.ToList();

            // Layers para remover: aquelas que existem no banco de dados
            // mas cujo nome não está no payload.
            var layersParaRemover = layersExistentesDb
                .Where(dbLayer => !string.IsNullOrEmpty(dbLayer.Layer) && !nomesLayersPayload.Contains(dbLayer.Layer))
                .ToList();

            if (layersParaRemover.Any())
            {
                _context.RemoveRange(layersParaRemover);
            }

            // Layers para adicionar: aquelas cujos nomes estão no payload
            // mas não existem no banco de dados para esta cartografia.
            foreach (var nomeLayerPayload in nomesLayersPayload)
            {
                var existeNoDb = layersExistentesDb
                                    .Any(dbLayer => dbLayer.Layer == nomeLayerPayload);

                if (!existeNoDb)
                {
                    var novaLayer = new Cartografialayer
                    {
                        Layer = nomeLayerPayload,
                        Parent = cartografiaExistente.RecId // Define a FK para a cartografia pai
                                                            // RecId será 0 por padrão, indicando uma nova entidade
                    };
                    _context.Cartografialayers.Add(novaLayer);
                }
                // Se a layer com este nome já existe (existeNoDb == true), não fazemos nada,
                // pois a premissa é que o payload só traz os nomes. Se houvesse outras
                // propriedades na Cartografialayer para atualizar, faríamos isso aqui,
                // localizando a layer existente e atualizando seus valores.
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartografiaExists(id))
                {
                    return NotFound($"Cartografia com RecId {id} não encontrada durante tentativa de salvar (concorrência).");
                }
                else
                {
                    throw; // Re-lançar a exceção se a entidade ainda existe (conflito de concorrência real)
                }
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao atualizar os dados da cartografia: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Para PUT, é comum retornar NoContent (204) ou Ok (200) com a entidade atualizada.
            // return NoContent();
            var cartografiaAtualizada = await _context.Cartografia
                                            .Include(c => c.Layers)
                                            .FirstOrDefaultAsync(c => c.RecId == id); // Recarregar para obter o estado final
            cartografiaAtualizada.Ordem = -1;
            return Ok(cartografiaAtualizada);
        }

        // POST: api/Cartografia
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cartografia>> PostCartografia([FromBody] Cartografia cartografia)
        {
            if (!ModelState.IsValid) // Boa prática: validar o modelo
            {
                return BadRequest(ModelState);
            }

            cartografia.RecId = 0;
            foreach (var layer in cartografia.Layers)
            {
                layer.RecId = 0; // Garantir que novas layers têm RecId 0 
            }
             
            
            _context.Cartografia.Add(cartografia);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) // Capturar exceções específicas do banco de dados
            {
                // Logar o erro (ex.InnerException?.Message)
                return StatusCode(500, "Ocorreu um erro ao salvar os dados da cartografia e suas layers.");
            }

            return CreatedAtAction("GetCartografia", new { id = cartografia.RecId }, cartografia);
        }

        // DELETE: api/Cartografia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartografia(int id)
        {
           

            var cartografia = await _context.Cartografia
                               .Include(c => c.Layers) // Opcional se a cascata estiver no BD/EF Core OnModelCreating
                               .FirstOrDefaultAsync(c => c.RecId == id);

            if (cartografia == null)
            {
                return NotFound();
            }

            _context.Cartografia.Remove(cartografia); // Marca a cartografia e suas layers (se configurado) para exclusão

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) // Tratar possíveis erros de restrição de FK se a cascata não funcionar como esperado
            {
                // Logar a exceção detalhada
                return StatusCode(500, $"Ocorreu um erro ao atualizar os dados da cartografia: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Retorno comum para DELETE bem-sucedido é NoContent (204) ou Ok (200) com uma mensagem.
            // Usando ApiResponse para consistência.
            return Ok();

        }
        internal class CartografiaOrdemInfo
        {
            public int RecId { get; set; }
            public int NovaOrdem { get; set; }
            public string OriginalString { get; set; } // Para debugging ou log
        }

        /// <summary>
        /// Atualiza a ordem de múltiplas entidades Cartografia.
        /// A ordem é determinada pela posição dos elementos na string 'dados' fornecida.
        /// Formato esperado para 'dados': "id1 | parent1 | tipo1 | value1!#id2 | parent2 | tipo2 | value2!#..."
        /// </summary>
        /// <param name="dados">String contendo os dados das cartografias e sua nova ordem implícita.</param>
        /// <returns>Uma resposta indicando o sucesso ou falha da operação de ordenação.</returns>
        [HttpPut("Ordena")] // Rota customizada para a ação de ordenar
        public async Task<IActionResult> PutCartografiaOrdena([FromBody] string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return BadRequest("A string 'dados' não pode ser vazia.");
            }

            var elementosString = data.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
            if (!elementosString.Any())
            {
                return BadRequest("Nenhum elemento encontrado na string 'dados' para processar.");
            }

            var itensParaOrdenar = new List<CartografiaOrdemInfo>();
            var errosDeParse = new List<string>();

            for (int i = 0; i < elementosString.Length; i++)
            {
                var elementoStr = elementosString[i];
                var partes = elementoStr.Split(new[] { "|" }, StringSplitOptions.None); // Mantém todas as partes, mesmo que vazias

                if (partes.Length < 1) // Precisa de pelo menos o ID
                {
                    errosDeParse.Add($"Elemento '{elementoStr}' não tem o formato esperado (ID em falta).");
                    continue;
                }

                if (int.TryParse(partes[0].Trim(), out int recId))
                {
                    // A nova ordem é o índice no array (base 0) ou i + 1 se a ordem for base 1.
                    // Vamos assumir ordem base 0 para simplificar, mas pode ser ajustado.
                    itensParaOrdenar.Add(new CartografiaOrdemInfo { RecId = recId, NovaOrdem = i, OriginalString = elementoStr });
                }
                else
                {
                    errosDeParse.Add($"ID '{partes[0]}' no elemento '{elementoStr}' não é um inteiro válido.");
                }
            }

            if (errosDeParse.Any())
            {
                return BadRequest( "Erros ao processar a string 'dados': " + string.Join("; ", errosDeParse));
            }

            if (!itensParaOrdenar.Any())
            {
                return BadRequest("Nenhum item válido para ordenação após o parse.");
            }

            // Iniciar uma transação para garantir atomicidade se o seu DbContext estiver configurado para tal,
            // ou confiar no SaveChanges() para envolver as operações numa transação.
            // Para múltiplas atualizações, é bom garantir que ou todas são feitas ou nenhuma.
            var strategy = _context.Database.CreateExecutionStrategy();

            try
            {
                await strategy.ExecuteAsync(async () =>
                {
                    using (var transaction = await _context.Database.BeginTransactionAsync())
                    {
                        List<string> errosDeUpdateInternos = new List<string>();

                        foreach (var item in itensParaOrdenar)
                        {
                            var cartografia = await _context.Cartografia.FindAsync(item.RecId);
                            if (cartografia != null)
                            {
                                cartografia.Ordem = item.NovaOrdem;
                            }
                            else
                            {
                                errosDeUpdateInternos.Add($"Cartografia com RecId {item.RecId} (do elemento '{item.OriginalString}') não encontrada.");
                            }
                        }

                        if (errosDeUpdateInternos.Any())
                        {
                            await transaction.RollbackAsync();
                            // Lança uma exceção específica para ser capturada fora do ExecuteAsync
                            throw new CartografiaOrdenaItensNaoEncontradosException(errosDeUpdateInternos);
                        }
                     
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                });

                // Se ExecuteAsync completou sem exceções, a operação foi bem-sucedida.
                return Ok( "Ordem de {itensParaOrdenar.Count} cartografia(s) atualizada com sucesso.");
            }
            catch (CartografiaOrdenaItensNaoEncontradosException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                // Um rollback já deve ter ocorrido na transação dentro do ExecuteAsync se a exceção foi lá.
                // Se a exceção foi lançada pelo SaveChanges e não capturada pela transação interna,
                // a estratégia de execução pode tentar novamente. Se todas as tentativas falharem:
                return StatusCode(500, $"Ocorreu um erro de banco de dados ao atualizar a ordem: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex) // Outras exceções inesperadas
            {
                return StatusCode(500,$"Erro inesperado ao atualizar a ordem: {ex.Message}");
            } 
       }
        public class CartografiaOrdenaItensNaoEncontradosException : Exception
        {
            public List<string> Erros { get; }
            public CartografiaOrdenaItensNaoEncontradosException(List<string> erros)
                : base("Algumas cartografias não foram encontradas durante a tentativa de ordenação.")
            {
                Erros = erros ?? new List<string>();
            }
        }
    }
}
