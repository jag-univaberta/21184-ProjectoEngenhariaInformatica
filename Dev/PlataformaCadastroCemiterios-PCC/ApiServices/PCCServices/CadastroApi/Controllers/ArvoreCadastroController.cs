using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Index.HPRtree;
using CadastroApi.Models;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CadastroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArvoreCadastroController : ControllerBase
    {

        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ArvoreCadastroController> _logger;
        private readonly ProjectoContext _context;

        // Injectar outros serviços necessários

        /*
 * NOTAS IMPORTANTES PARA ESTA IMPLEMENTAÇÃO:
 *
 * 1.  Classes DTO: Para este código funcionar, precisa de ter classes que representem
 * os dados das suas tabelas. Exemplo:
 *
 * public class Cemiterio {
 * public int Id { get; set; }
 * public string Nome { get; set; }
 * // ... outras propriedades
 * }
 *
 * public class Talhao {
 * public int Id { get; set; }
 * public string Nome { get; set; }
 * public int CemiterioId { get; set; } // Chave estrangeira para o cemitério
 * // ... outras propriedades
 * }
 *
 * public class Construcao {
 * public int Id { get; set; }
 * public string Nome { get; set; }
 * public int TalhaoId { get; set; } // Chave estrangeira para o talhão
 * // ... outras propriedades
 * }
 *
 * 2.  DbContext: O seu `_context` do Entity Framework Core deve ter os DbSets configurados.
 * Ex: public DbSet<Cemiterio> Cemiterios { get; set; }
 * public DbSet<Talhao> Talhoes { get; set; }
 * public DbSet<Construcao> Construcoes { get; set; }
 *
 * 3.  Serialização JSON: Este código retorna um objeto C#. O ASP.NET Core irá
 * automaticamente serializá-lo para JSON. Não é necessário construir a string manualmente.
*/

        // Classe para representar cada item na estrutura da árvore final
        public class ArvoreCemiterioItem
        {
            public string Id { get; set; }
            public string Recid { get; set; }
            public string Parent { get; set; }
            public string Value { get; set; }
            public string Tipo { get; set; }
            public bool Checkbox { get; set; }
            public bool Opened { get; set; }
          
            public List<ArvoreCemiterioItem> Items { get; set; } = new List<ArvoreCemiterioItem>();
        }
         

        [HttpGet("{userid}")]
        public async Task<IActionResult> GetArvoreCadastro(string userid)
        {
            try
            {
                // --- Etapa 1: Obter todos os dados necessários da base de dados de uma só vez ---
                var todosCemiterios = await _context.Cemiterios.OrderBy(c => c.Nome).ToListAsync();
                var todosTalhoes = await _context.Talhao.OrderBy(t => t.Codigo).ToListAsync();
                var todasConstrucoes = await _context.Construcao.OrderBy(c => c.Designacao).ToListAsync();

                // --- Etapa 2: Construir a estrutura hierárquica em memória ---

                // Dicionários para acesso rápido aos nós da árvore já criados
                var cemiterioNodes = new Dictionary<int, ArvoreCemiterioItem>();
                var talhaoNodes = new Dictionary<int, ArvoreCemiterioItem>();

                // Criar o nó raiz da árvore
                var raiz = new ArvoreCemiterioItem
                {
                    Id = "principal",
                    Recid = "principal",
                    Parent = null, // Raiz não tem pai
                    Value = "Cadastro de Cemitérios",
                    Tipo = "pai",
                    Checkbox = true,
                    Opened = true
                };

                // Adicionar os cemitérios como filhos do nó raiz
                foreach (var cemiterio in todosCemiterios)
                {
                    var nodeCemiterio = new ArvoreCemiterioItem
                    {
                        Id = $"cem-{cemiterio.RecId}",
                        Recid = cemiterio.RecId.ToString(),
                        Parent = raiz.Id,
                        Value = cemiterio.Nome,
                        Tipo = "cemiterio",
                        Checkbox = true,
                        Opened = true
                    };
                    raiz.Items.Add(nodeCemiterio);
                    cemiterioNodes[cemiterio.RecId] = nodeCemiterio; // Guardar referência para acesso rápido
                }

                // Adicionar os talhões como filhos dos seus respetivos cemitérios
                foreach (var talhao in todosTalhoes)
                {
                    // Verifica se o cemitério pai existe no nosso mapa de nós
                    if (cemiterioNodes.TryGetValue(talhao.CemiterioId, out var parentCemiterioNode))
                    {
                        var nodeTalhao = new ArvoreCemiterioItem
                        {
                            Id = $"tal-{talhao.RecId}",
                            Recid = talhao.RecId.ToString(),
                            Parent = parentCemiterioNode.Id,
                            Value = talhao.Codigo,
                            Tipo = "talhao",
                            Checkbox = true,
                            Opened = true
                        };
                        parentCemiterioNode.Items.Add(nodeTalhao);
                        talhaoNodes[talhao.RecId] = nodeTalhao; // Guardar referência
                    }
                }

                // Adicionar as construções como filhas dos seus respetivos talhões
                /*foreach (var construcao in todasConstrucoes)
                {
                    // Verifica se o talhão pai existe no nosso mapa de nós
                    if (talhaoNodes.TryGetValue(construcao.TalhaoId, out var parentTalhaoNode))
                    {
                        var nodeConstrucao = new ArvoreCemiterioItem
                        {
                            Id = $"con-{construcao.RecId}",
                            Recid = construcao.RecId.ToString(),
                            Parent = parentTalhaoNode.Id,
                            Value = construcao.Designacao,
                            Tipo = "construcao",
                            Checkbox = true,
                            Opened = true,
                        };
                        parentTalhaoNode.Items.Add(nodeConstrucao);
                    }
                }*/

                // --- Etapa 3: Retornar o resultado ---
                // O ASP.NET Core irá serializar a lista para JSON automaticamente.
                // A árvore final está contida dentro do nó raiz, por isso retornamos uma lista com ele.
                return Ok(new List<ArvoreCemiterioItem> { raiz });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao gerar a árvore de cemitérios.");
                return StatusCode(500, "Ocorreu um erro interno ao processar o seu pedido.");
            }
        }

        public ArvoreCadastroController(ITokenService tokenService, ProjectoContext context, HttpClient httpClient, IConfiguration configuration, ILogger<ArvoreCadastroController> logger)
        {
            _tokenService = tokenService;

            _context = context;

            _httpClient = httpClient;

            _configuration = configuration;

            _logger = logger;
        }
        //[Authorize]
        //[HttpGet("{userid}")]
        //public async Task<IActionResult> GetArvoreCadastrddo(string userid)
        //{

        //    ArvCadastroDTO obj = new ArvCadastroDTO
        //    {
        //        id = "Cadastro",
        //        recid = "principal",
        //        parentid = null,
        //        tipo = "PAI",
        //        value = "Cadastro",
        //        checkbox = true,
        //        items = new List<ArvCadastroDTO>() // Inicializa a lista de itens
        //    };

        //    var json = JsonConvert.SerializeObject(new List<ArvCadastroDTO> { obj }, Newtonsoft.Json.Formatting.Indented);

        //    return Content(json, "application/json");
        //    /*
        //    try
        //    {
        //        List<ArvoreCadastro> dados = await _context.ArvoreCadastros
        //            .FromSqlRaw($"select * from f_arvore_cadastro('{userid}')")
        //            .ToListAsync();

                
        //        // Transforme os dados em um DTO (Data Transfer Object) mais simples
        //        List<ArvCadastroDTO> items = dados.Select(item => new ArvCadastroDTO
        //        {
        //            id = item.Rec_id,
        //            recid = item.Rec_id,
        //            parentid = item.Parent_id,
        //            tipo = siglaMap.ContainsKey(item.Tipo ?? 0) ? siglaMap[item.Tipo ?? 0] : "",
        //            tema = item.Tema,
        //            value = System.Security.SecurityElement.Escape(item.Nome),
        //            checkbox = true,
        //            items = new List<ArvCadastroDTO>() // Inicializa a lista de itens
        //        }).ToList();

        //        // Construir a hierarquia
        //        var hierarchy = BuildHierarchy(null, items);

        //        // Adicionar o item principal como pai de todos os itens
        //        var principalItem = new ArvCadastroDTO
        //        {
        //            id = "principal",
        //            recid = "principal",
        //            parentid = null,
        //            tipo = "CADASTRO",
        //            tema = null,
        //            value = "Cadastro",
        //            checkbox = true,
        //            items = hierarchy // A hierarquia construída será filha deste item
        //        };

        //        // Serializar os dados para JSON
        //        var json = JsonConvert.SerializeObject(new List<ArvCadastroDTO> { principalItem }, Newtonsoft.Json.Formatting.Indented);

        //        return Content(json, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Logar o erro (usar um logger de verdade em produção)
        //        Console.WriteLine($"Erro ao obter ArvCadastro: {ex}");
        //        return StatusCode(500, "Ocorreu um erro ao processar a solicitação.");
        //    }

        //    */
        //}

        // Função para construir a hierarquia JSON
        private List<ArvCadastroDTO> BuildHierarchy(string parentId, List<ArvCadastroDTO> allItems)
        {
            var children = allItems.Where(item => item.parentid == parentId).ToList();
            List<ArvCadastroDTO> hierarchy = new List<ArvCadastroDTO>();

            foreach (var child in children)
            {
                // Obtém os filhos recursivamente
                child.items = BuildHierarchy(child.recid, allItems);
                hierarchy.Add(child);
            }

            return hierarchy;
        }
        public class ArvCadastroDTO
        {
            public string id { get; set; }
            public string recid { get; set; }
            public string parentid { get; set; }
            public string tipo { get; set; }
            public string value { get; set; }
            public bool checkbox { get; set; }
            public List<ArvCadastroDTO> items { get; set; }
        }

    }
}
