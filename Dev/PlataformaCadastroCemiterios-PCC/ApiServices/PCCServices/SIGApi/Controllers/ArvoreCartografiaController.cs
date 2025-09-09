using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using SIGApi.Models;

namespace SIGApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArvoreCartografiaController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ArvoreCartografiaController> _logger;
        private readonly ProjectoContext _context;

        // Injectar outros serviços necessários


        public ArvoreCartografiaController(ITokenService tokenService, ProjectoContext context, HttpClient httpClient, IConfiguration configuration, ILogger<ArvoreCartografiaController> logger)
        {
            _tokenService = tokenService;

            _context = context;

            _httpClient = httpClient;

            _configuration = configuration;

            _logger = logger;
        }

        //[Authorize]
        [HttpGet("{userid}")]
        public async Task<string> GetArvoreCartografia(string userid)
        {
            try
            {

                FormattableString query = $@"
                SELECT
                    p.rec_id AS RecId,
                    p.nome AS Nome,
                    p.parent AS Parent,
                    public.f_cartografia_nivel(p.rec_id) AS Nivel,
                    CAST(public.f_getcartografialayers(p.rec_id) AS TEXT) AS Layers, -- Exemplo: Cast para TEXT se a função retorna um tipo complexo
                    p.ordem AS Ordem
                FROM
                    inf_geografica.cartografia p order by p.ordem";

                List<ArvoreCartografia> dados = await _context.Database
                    .SqlQuery<ArvoreCartografia>(query)
                    .ToListAsync();


                //List<ArvoreCartografia> dados = await _context.ArvoreCartografia
             // .FromSqlInterpolated($@"select p.rec_id as recid, p.nome as nome, p.parent as parent, public.f_cartografia_nivel(rec_id) as nivel, public.f_getcartografialayers(p.rec_id) as layers, p.ordem as ordem from inf_geografica.cartografia p")
             // .ToListAsync();

                int contador = 0;
                List<ArvoreCartografiaItem> listaobj = new List<ArvoreCartografiaItem>();
               
                ArvoreCartografiaItem elinicial = new ArvoreCartografiaItem();
                elinicial.RecId = contador;
                elinicial.Nome = "Cartografia Base";
                elinicial.Tipo = "parent";
                elinicial.Parent = -1;
                elinicial.Nivel = 0;
                elinicial.Layers = "";
                elinicial.Ordem = 0;
                elinicial.Id = contador;
                listaobj.Add(elinicial);
                contador++;

                 
                foreach (ArvoreCartografia item in dados)
                {
                    ArvoreCartografiaItem el = new ArvoreCartografiaItem();
                    el.RecId = item.RecId;
                    el.Nome = item.Nome;
                    el.Tipo = "cartografia";
                    el.Parent = item.Parent == null ? 0 : item.Parent; 
                    el.Nivel = item.Nivel;
                    el.Layers = item.Layers == "" ? "" : item.Layers; 
                    el.Ordem = item.Ordem; 
                    el.Id = contador;
                    listaobj.Add(el);
                    contador++;
                }

                List<ArvoreCartografiaItem> listaobjfinal = new List<ArvoreCartografiaItem>();
                foreach (ArvoreCartografiaItem item in listaobj)
                {
                    if (item.Tipo =="parent")
                    {
                        listaobjfinal.Add(item);
                    }
                    else
                    {
                        int posicao = 0;
                        for (int i = 0; i < listaobjfinal.Count; i++)
                        {
                            if (listaobjfinal[i].RecId == item.Parent)
                            {
                                posicao = i;
                                for (int j = i + 1; j < listaobjfinal.Count; j++)
                                {
                                    if (listaobjfinal[j].Parent == item.Parent)
                                    {
                                        //if (item.Layers == listaobjfinal[j].Layers)
                                       // {
                                            posicao = j;
                                        //}
                                    }
                                    else
                                    {
                                        j = listaobjfinal.Count - 1;
                                    }
                                }
                                listaobjfinal.Insert(posicao + 1, item);
                            }
                        }
                    }
                }

                foreach (ArvoreCartografiaItem el in listaobjfinal)
                {
                    if (el.Parent != 0)
                    {
                        for (int i = 0; i < listaobjfinal.Count; i++)
                        {

                            if (listaobjfinal[i].RecId == el.Parent)
                            {
                                el.Parent = (int)listaobjfinal[i].Id;
                                i = listaobjfinal.Count - 1;
                            }
                        }
                    }
                }
                List<long> listaPais = new List<long>();
                long nivel = 0;
                long pai_anterior = -1;
                System.Text.StringBuilder resultJSON = new System.Text.StringBuilder("");
                for (int x = 0; x < listaobjfinal.Count; x++)
                {
                    if (x == 0)
                    {
                        resultJSON.Clear();
                        resultJSON.Append("[" + System.Environment.NewLine + System.Environment.NewLine);
                        listaPais.Add(0);
                    }

                    long id_actual = (long)listaobjfinal[x].Id;

                    long pai_actual = listaobjfinal[x].Parent == null ? 0 : (long)listaobjfinal[x].Parent;

                    long pai_seguinte;
                    if (x == listaobjfinal.Count - 1)
                    {
                        pai_seguinte = -99;
                    }
                    else
                    {
                        pai_seguinte = listaobjfinal[x + 1].Parent == null ? 0 : (long)listaobjfinal[x + 1].Parent;
                    }

                    /* aqui temos id_actual, pai_anterior, pai_actual, pai_seguinte */

                    if (pai_anterior == pai_actual)
                    {
                        if (pai_seguinte == pai_actual)
                        {
                            resultJSON.Append("{" + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"id" + @"""" + ":" + @"""" + listaobjfinal[x].Id.ToString() + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"tipo" + @"""" + ":" + @"""" + listaobjfinal[x].Tipo.ToString() + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"checkbox" + @"""" + ":true" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"rec_id" + @"""" + ":" + @"""" + listaobjfinal[x].RecId + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"parentid" + @"""" + ":" + @"""" + listaobjfinal[x].Parent + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"layers" + @"""" + ":" + @"""" + listaobjfinal[x].Layers + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"value" + @"""" + ":" + @"""" + System.Security.SecurityElement.Escape(listaobjfinal[x].Nome) + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"opened" + @"""" + ":true" + System.Environment.NewLine);
                            resultJSON.Append("}" + System.Environment.NewLine + "," + System.Environment.NewLine);
                            pai_anterior = pai_actual;

                            nivel = x;
                            //listaPais.Add(id_actual);
                        }
                        else
                        {
                            // o seguinte não é o mesmo pai                        
                            resultJSON.Append("{" + System.Environment.NewLine);
                            resultJSON.Append(@"""" + "id" + @"""" + ":" + @"""" + listaobjfinal[x].Id.ToString() + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"tipo" + @"""" + ":" + @"""" + listaobjfinal[x].Tipo.ToString() + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"checkbox" + @"""" + ":true" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"rec_id" + @"""" + ":" + @"""" + listaobjfinal[x].RecId + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"parentid" + @"""" + ":" + @"""" + listaobjfinal[x].Parent + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"layers" + @"""" + ":" + @"""" + listaobjfinal[x].Layers + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + "value" + @"""" + ":" + @"""" + System.Security.SecurityElement.Escape(listaobjfinal[x].Nome) + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"opened" + @"""" + ":true" + System.Environment.NewLine);
                            if (pai_seguinte == id_actual)
                            {
                                resultJSON.Append("," + System.Environment.NewLine);
                                resultJSON.Append(@"""" + "items" + @"""" + ":[" + System.Environment.NewLine);
                                listaPais.Add(id_actual);
                            }
                            else
                            {
                                resultJSON.Append("}" + System.Environment.NewLine);
                            }
                        }
                    }
                    else
                    {
                        int entrou = 0;
                        for (int h = listaPais.Count - 1; h >= 0; h--)
                        {
                            if (listaPais[h] != pai_actual)
                            {
                                entrou = 1;
                                listaPais.RemoveAt(h);
                                resultJSON.Append("]}" + System.Environment.NewLine);
                            }
                            else
                            {
                                h = 0;
                            }
                        }
                        if (entrou == 1)
                        {
                            resultJSON.Append("," + System.Environment.NewLine);
                        }
                        if (pai_seguinte == pai_actual)
                        {
                            resultJSON.Append("{" + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"id" + @"""" + ":" + @"""" + listaobjfinal[x].Id.ToString() + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"tipo" + @"""" + ":" + @"""" + listaobjfinal[x].Tipo.ToString() + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"checkbox" + @"""" + ":true" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"rec_id" + @"""" + ":" + @"""" + listaobjfinal[x].RecId + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"parentid" + @"""" + ":" + @"""" + listaobjfinal[x].Parent + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"layers" + @"""" + ":" + @"""" + listaobjfinal[x].Layers + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"value" + @"""" + ":" + @"""" + System.Security.SecurityElement.Escape(listaobjfinal[x].Nome) + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"opened" + @"""" + ":true" + System.Environment.NewLine);
                            resultJSON.Append("}," + System.Environment.NewLine);

                            pai_anterior = pai_actual;

                            nivel = x;
                        }
                        else
                        {
                            resultJSON.Append("{" + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"id" + @"""" + ":" + @"""" + listaobjfinal[x].Id.ToString() + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"tipo" + @"""" + ":" + @"""" + listaobjfinal[x].Tipo.ToString() + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"checkbox" + @"""" + ":true" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"rec_id" + @"""" + ":" + @"""" + listaobjfinal[x].RecId + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"parentid" + @"""" + ":" + @"""" + listaobjfinal[x].Parent + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"layers" + @"""" + ":" + @"""" + listaobjfinal[x].Layers + @"""" + "," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"value" + @"""" + ":" + @"""" + System.Security.SecurityElement.Escape(listaobjfinal[x].Nome) + @"""," + System.Environment.NewLine);
                            resultJSON.Append(@"""" + @"opened" + @"""" + ":true" + System.Environment.NewLine);
                            if (pai_seguinte == id_actual)
                            {
                                resultJSON.Append("," + System.Environment.NewLine);
                                resultJSON.Append(@"""" + "items" + @"""" + ":[" + System.Environment.NewLine);
                                listaPais.Add(id_actual);
                            }
                            else
                            {
                                resultJSON.Append("}" + System.Environment.NewLine);
                            }
                            pai_anterior = id_actual;
                        }
                    }
                }
                for (int j = 1; j < listaPais.Count; j++)
                {
                    resultJSON.Append("]}" + System.Environment.NewLine);
                }
                if (listaobjfinal.Count == 0)
                {
                    resultJSON.Append("[" + System.Environment.NewLine);
                }
                resultJSON.Append("]" + System.Environment.NewLine);

                return resultJSON.ToString();
                }
            catch (Exception ex)
            {

                string totalex = ex.Message.ToString();
                if (ex.InnerException != null)
                {
                    totalex = totalex + " inner: " + ex.InnerException.Message.ToString();
                }
                _logger.LogError("Erro: {erro} ", totalex);
                return "[]";
            }

        } 


    }
  
}
