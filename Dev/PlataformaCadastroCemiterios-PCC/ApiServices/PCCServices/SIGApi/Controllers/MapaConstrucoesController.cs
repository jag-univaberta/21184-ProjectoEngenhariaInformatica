using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using OSGeo.MapGuide;
using pccMap4;
using System.Xml;

namespace SIGApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MapaConstrucoesController : Controller
    {
        private readonly IConfiguration _configuration;

        public MapaConstrucoesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

         
        //[Authorize]
        [HttpPost]
        public IActionResult Connect([FromBody] MapaConstrucoesCredentials mapaCredentials)
        {
            Boolean resposta = true;
            try
            {
                var conn = new MgSiteConnection();
                string sessionId = mapaCredentials.Sessionid;

                string Construcoes = mapaCredentials.Construcoes;

                bool Viewer = (mapaCredentials?.Viewer == "true" ? true : false);

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
                string PccSymbolResource = _configuration["MapGuide:PccSymbolResource"];


                string ficheiro = layerdef_pretensao + "PCC_Construcoes.xml";

                pccMapguide4Server ms;
                pccMapguide4Map m;


                ms = new pccMapguide4Server(sWebConfigIni, sessionId, sWebConfigIni);

                IPCCIMapServer aa = (IPCCIMapServer)ms;
                //pccIMap4Server aa = (pccIMap4Server)ms;
                m = new pccMapguide4Map(ref aa, mapaCredentials.Mapa, sMetricCSWKT);

                Boolean res;

                pccMapguide4Layer el1 = m.get_GetLayer("PCC_Construcoes");
                if (el1 != null)
                {
                    res = m.RemoveLayer(el1);
                    m.Save();
                }

                if (Construcoes != "")
                {
                    string filterAux = "";
                    string[] idsTemp = Construcoes.Split("|", StringSplitOptions.None);

                    foreach (string ids in idsTemp)
                    {
                        if (filterAux != "")
                            filterAux = filterAux + ",";

                        filterAux = filterAux + ids + "";
                    }

                    string filter = "talhao_id in (" + filterAux + ")";

                    string layerdef;
                    layerdef = Pvt_getXmlString(ficheiro);
                    layerdef = layerdef.Replace("%filter%", filter);


                    string featuresource = PccFeatureSource; //ConfigurationManager.AppSettings.Get("MAP:G10FeatureSource").ToString();
                    layerdef = layerdef.Replace("%featuresource%", featuresource);

                    string symbolresource = PccSymbolResource;
                    layerdef = layerdef.Replace("%symbolresource%", symbolresource);


                    // XmlDocument doc = new XmlDocument();
                    string laydefStringXML = layerdef.ToString();


                    pccMap4View aux = m.GetActualView();
                    resposta = m.AddLayerfromFile(layerdef, "PCC_Construcoes", "PCC_Construcoes", 0, true);
                    m.SetActualView(aux);
                    m.Save();

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

        private string Pvt_getXmlString(string strFile)
        {
            // Load the xml file into XmlDocument object.
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            try
            {
                xmlDoc.Load(strFile);
            }
            catch (Exception ex)
            {
                string totalex = ex.Message.ToString();
                if (ex.InnerException != null)
                {
                    totalex = totalex + " inner: " + ex.InnerException.Message.ToString();
                }


                return null;
            }

            try
            {
                // Now create StringWriter object to get data from xml document.
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);

                xmlDoc.WriteTo(xw);

                return sw.ToString();
            }
            catch (Exception ex)
            {
                string totalex = ex.Message.ToString();
                if (ex.InnerException != null)
                {
                    totalex = totalex + " inner: " + ex.InnerException.Message.ToString();
                }


                return null;
            }
        }

        public class MapaConstrucoesCredentials
        {
  
            public string Mapa { get; set; }
            public string Mapadef { get; set; }
            public string Sessionid { get; set; }
            public string Viewer { get; set; }
            public string Construcoes { get; set; }

            public int Userid { get; set; }
            public string Usersession { get; set; }

        }

    }
}
