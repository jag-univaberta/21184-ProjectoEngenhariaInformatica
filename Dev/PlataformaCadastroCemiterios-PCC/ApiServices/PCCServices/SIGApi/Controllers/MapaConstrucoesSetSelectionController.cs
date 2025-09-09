using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using OSGeo.MapGuide;
using pccMap4;
using System.Xml;
using System.Data;
using pccBase4;

namespace SIGApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MapaConstrucoesSetSelectionController : Controller
    {
        private readonly IConfiguration _configuration;

        public MapaConstrucoesSetSelectionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public class MapaConstrucoesSetSelectionCredentials
        {
            public string mapa { get; set; }
            public string mapadef { get; set; }
            public string viewer { get; set; }
            public string sessionid { get; set; }
            public string cx { get; set; }
            public string cy { get; set; }
            public string escala { get; set; }
            public string construcaoid { get; set; }
        }

 
        // GET: api/Mapa
        [HttpPost]
        public IActionResult Connect([FromBody] MapaConstrucoesSetSelectionCredentials mapaCredentials)
        {
            Boolean resposta = true;
            try
            {

                pccGeoUtils geoaux = new pccGeoUtils();

                string XMLSelection = "";

                string construcaoid = mapaCredentials.construcaoid; 

                string Cx = mapaCredentials.cx;
                string Cy = mapaCredentials.cy;
                string Escala = mapaCredentials.escala;

                var conn = new MgSiteConnection();
                string sessionId = mapaCredentials.sessionid;
                bool Viewer = (mapaCredentials?.viewer == "true" ? true : false);


                MgResourceService resSvc = null; 

                var mdfId = new MgResourceIdentifier(mapaCredentials.mapadef);

                if (string.IsNullOrEmpty(sessionId))
                {
                    var userInfo = new MgUserInformation("Anonymous", "");
                    conn.Open(userInfo);
                    var site = conn.GetSite();
                    sessionId = site.CreateSession();
                     
                    userInfo.SetMgSessionId(sessionId);
                    resSvc = (MgResourceService)conn.CreateService(MgServiceType.ResourceService);
                     
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
                string G10FeatureSource = _configuration["MapGuide:PCCFeatureSource"];


                string ficheiro = layerdef_pretensao + "PCC_Construcoes.xml";

                pccMapguide4Server ms;
                pccMapguide4Map m;


                ms = new pccMapguide4Server(sWebConfigIni, sessionId, sWebConfigIni);

                IPCCIMapServer aa = (IPCCIMapServer)ms;

                m = new pccMapguide4Map(ref aa, mapaCredentials.mapa, sMetricCSWKT);


                try
                {
                    long dimensao = 600;
                    double centrox = Convert.ToDouble(Cx.ToString());
                    double centroy = Convert.ToDouble(Cy.ToString());
                                                                       
                    double escala_double = Convert.ToDouble(Escala);
                    long escala = Convert.ToInt64(escala_double); 
                    double imagewidth = Convert.ToDouble(dimensao.ToString());
                    double imageheight = Convert.ToDouble(dimensao.ToString());
                    double metersPerPixel = 0.0254 / 300;
                    double metersPerUnit = m.GetMetersPerUnit();
                    double mapawidth = imagewidth * metersPerPixel * escala / metersPerUnit;
                    double mapaheight = imageheight * metersPerPixel * escala / metersPerUnit;

                    pccBase4.pccGeoPoint centro = new pccBase4.pccGeoPoint(centrox, centroy);
                    pccMap4View v = new pccMap4View(centro, escala, m.GetInitialView.Dpi, mapawidth, mapaheight, imagewidth, imageheight);
                    m.SetActualView(v);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    throw;
                }
                try
                {

                    
                    bool aux = true;

                    if (m.SelectionClear())
                    {
                        if (!string.IsNullOrEmpty(construcaoid))
                        {
                            
                            aux = m.SelectionByParametersQuery("PCC_Construcoes", "rec_id=" + construcaoid + "", false);
                            

                            if (!aux) m.SelectionClear();
                        }
                        if (aux)
                        {

                            OSGeo.MapGuide.MgSelection sel = (OSGeo.MapGuide.MgSelection)m.ActualSelection;
                            MgReadOnlyLayerCollection Lista_Layers;

                            Lista_Layers = sel.GetLayers();

                            if (sel.GetLayers() != null)
                            {

                                XMLSelection = sel.ToXml();
                                return Ok(XMLSelection.ToString());
                            }
                            else
                            {
                                return Ok("");
                            }
                        }
                        else
                        {
                            return Ok("");

                        }


                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok("");

        }
   

        }
    }