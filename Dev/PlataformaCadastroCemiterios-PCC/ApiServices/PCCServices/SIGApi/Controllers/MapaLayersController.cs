using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using OSGeo.MapGuide;
using pccMap4;
namespace SIGApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MapaLayersController : Controller
    {
        private readonly IConfiguration _configuration;

        public MapaLayersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       // [Authorize]
        [HttpGet("{sessionId}")]
        public async Task<string> GetMapaLayers(string sessionId)
        {
            var conn = new MgSiteConnection();
            MgResourceService resSvc = null;

            var userInfo = new MgUserInformation(sessionId);
            conn.Open(userInfo);
            resSvc = (MgResourceService)conn.CreateService(MgServiceType.ResourceService);


            string sWebConfigIni = _configuration["MapGuide:WebConfigPath"];
            string layerdef_pretensao = _configuration["MapGuide:LayersDefinition"];


            string sMetricCSWKT = _configuration["MapGuide:MetricCSWKT"];
            sMetricCSWKT = sMetricCSWKT.Replace("#", "\""); //* obrigatorio colocar isto 
            string PccFeatureSource = _configuration["MapGuide:PccFeatureSource"];

            pccMapguide4Server ms;
            pccMapguide4Map m;


            ms = new pccMapguide4Server(sWebConfigIni, sessionId, sWebConfigIni);

            IPCCIMapServer aa = (IPCCIMapServer)ms;

            string Mapa = _configuration["MapGuide:Nome_do_Mapa"];

            m = new pccMapguide4Map(ref aa, Mapa, sMetricCSWKT);


            //OSGeo.MapGuide.MgLayerCollection grupos = m.MapObject.GetLayers().Layers;

            var resposta1 = m.GetLayers.Layers;

            long contador = 1;

            System.Text.StringBuilder resultJSON = new System.Text.StringBuilder("");
            resultJSON.Clear();
            resultJSON.Append("[" + System.Environment.NewLine);
            foreach (pccMapguide4Layer el in resposta1)
            {
                if (contador > 1)
                {
                    resultJSON.Append("," + System.Environment.NewLine);
                }

                resultJSON.Append("{" + System.Environment.NewLine);

                resultJSON.Append(@"""" + @"id" + @"""" + ":" + @"""" + el.Id.ToString() + @"""" + "," + System.Environment.NewLine);

                resultJSON.Append(@"""" + @"value" + @"""" + ":" + @"""" + el.Name.ToString() + @"""" + System.Environment.NewLine);

                resultJSON.Append("}" + System.Environment.NewLine);

                contador++;
            }
            resultJSON.Append("]" + System.Environment.NewLine);
            return resultJSON.ToString();
        }


        //[Authorize]
        [HttpPost]
        public IActionResult PostMapaLayers([FromBody] MapaLayersCredentials mapaCredentials)
        {
            Boolean resposta = true;
            try
            {
                var conn = new MgSiteConnection();
                string sessionId = mapaCredentials.Sessionid;
                bool Viewer = (mapaCredentials?.Viewer == "true" ? true : false);
                string Addedlayers = mapaCredentials.Addedlayers;

                string Removedlayers = mapaCredentials.Removedlayers;

                MgResourceService resSvc = null;
                //var mdfId = new MgResourceIdentifier("Library://Samples/Sheboygan/Maps/Sheboygan.MapDefinition");

                var mdfId = new MgResourceIdentifier(mapaCredentials.Mapadef);

                if (string.IsNullOrEmpty(sessionId))
                {
                    var userInfo = new MgUserInformation("Anonymous", "");
                    conn.Open(userInfo);
                    var site = conn.GetSite();
                    sessionId = site.CreateSession();
                    //Important: Attach the generated session id, otherwise we can't save to any resources in the session repo
                    userInfo.SetMgSessionId(sessionId);
                    resSvc = (MgResourceService)conn.CreateService(MgServiceType.ResourceService);

                    //mapguide-react-layout makes an incorrect assumption during init that if it was initialized with a session id, then
                    //a runtime map must've already been created as it assumes it's coming back from a browser refresh, so to cater for
                    //this assumption, pre-create the runtime map state and save it.
                    var map = new MgMap(conn);
                    var mapName = mdfId.Name;
                    map.Create(mdfId, mapName);
                    var mapStateId = new MgResourceIdentifier($"Session:{sessionId}//{mapName}.Map");
                    var sel = new MgSelection(map);
                    sel.Save(resSvc, mapName);
                    map.Save(resSvc, mapStateId);
                }
                else //Reuse existing session
                {
                    var userInfo = new MgUserInformation(sessionId);
                    conn.Open(userInfo);
                    resSvc = (MgResourceService)conn.CreateService(MgServiceType.ResourceService);
                }

                string sWebConfigIni = _configuration["MapGuide:WebConfigPath"];
                string layerdef_pretensao = _configuration["MapGuide:LayersDefinition"];

                if (Viewer)
                {
                    if (_configuration.GetSection("MapGuide:LayersDefinitionViewer").Exists())
                    {
                        layerdef_pretensao = _configuration["MapGuide:LayersDefinitionViewer"];
                    }
                }

                string sMetricCSWKT = _configuration["MapGuide:MetricCSWKT"];
                sMetricCSWKT = sMetricCSWKT.Replace("#", "\""); //* obrigatorio colocar isto 
                string PccFeatureSource = _configuration["MapGuide:PccFeatureSource"];


                string ficheiro = layerdef_pretensao + "PCC_Construcoes.xml";

                pccMapguide4Server ms;
                pccMapguide4Map m;

                ms = new pccMapguide4Server(sWebConfigIni, sessionId, sWebConfigIni);

                IPCCIMapServer aa = (IPCCIMapServer)ms;

                m = new pccMapguide4Map(ref aa, mapaCredentials.Mapa, sMetricCSWKT);

                if (Removedlayers != "")
                {
                    string filterAux = "";
                    string[] idsTemp = Removedlayers.Split("|", StringSplitOptions.None);
                    idsTemp = Array.FindAll(idsTemp, s => !string.IsNullOrEmpty(s));
                    foreach (string layername in idsTemp)
                    {
                        OSGeo.MapGuide.MgLayerGroupCollection grupos = m.MapObject.GetLayerGroups();

                        pccMapguide4Layer l = m.get_GetLayer(layername);
                        if (l == null)
                        {
                            // return false;
                        }
                        else
                        {
                            try
                            {
                                l.SetVisibility(false);
                                m.Save();
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    foreach (var grupo in grupos)
                                    {
                                        if (grupo.Name == layername)
                                        {
                                            grupo.SetVisible(false);
                                            m.Save();
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    // Handle the exception
                                    resposta = false;
                                }
                            }
                            //return true;
                        }
                    }



                }

                if (Addedlayers != "")
                {
                    string filterAux = "";
                    string[] idsTemp = Addedlayers.Split("|", StringSplitOptions.None);
                    idsTemp = Array.FindAll(idsTemp, s => !string.IsNullOrEmpty(s));
                    foreach (string layername in idsTemp)
                    {
                        OSGeo.MapGuide.MgLayerGroupCollection grupos = m.MapObject.GetLayerGroups();

                        pccMapguide4Layer l = m.get_GetLayer(layername);
                        if (l == null)
                        {
                            // return false;
                        }
                        else
                        {
                            try
                            {
                                l.SetVisibility(true);
                                m.Save();
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    foreach (var grupo in grupos)
                                    {
                                        if (grupo.Name == layername)
                                        {
                                            grupo.SetVisible(true);
                                            m.Save();
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    // Handle the exception
                                    resposta = false;
                                }
                            }
                            //return true;
                        }
                    }



                }

           

                if (resposta)
                {
                    return Ok("Mapa atualizado.");
                }
                else
                {
                    return BadRequest("Falhou atualização do mapa.");
                }
                 
            }
            catch (Exception ex)
            {

                throw ex; 
            }

        }

        public class MapaLayersCredentials
        {
            public string Mapa { get; set; }
            public string Mapadef { get; set; }

            public string Sessionid { get; set; }
            public string Viewer { get; set; }
            public string Addedlayers { get; set; }
            public string Removedlayers { get; set; }
        }

    }
}
