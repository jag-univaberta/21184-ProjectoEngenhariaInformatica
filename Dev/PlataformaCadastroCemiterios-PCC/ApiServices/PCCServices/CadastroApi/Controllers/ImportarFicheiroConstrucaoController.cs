using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroApi.Models;
using Microsoft.AspNetCore.Authorization;
using Npgsql.Internal;
using System.Globalization;
using ArcShapeFile;
using pccShapeObject;
using ACadSharp;
using ACadSharp.Entities;
using pccBase4;
using System.Xml;
using ACadSharp.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace CadastroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImportarFicheiroConstrucaoController : ControllerBase
    {
        private readonly ProjectoContext _context;
        private readonly IConfiguration _configuration;

        public ImportarFicheiroConstrucaoController(ProjectoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public class InfoFicheiros
        {

            public IList<IFormFile> File { get; set; }

            public string Construcaoid { get; set; }
            public string Userid { get; set; }

        }
        [HttpGet]
        public IActionResult GetImportarFicheiroConstrucao (string filepath)

        { 
            string res = "";

            try
            {
                string prjPath = Path.ChangeExtension(filepath, ".prj");
                if (System.IO.File.Exists(prjPath))
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(prjPath))
                        {
                            string linha;
                            while ((linha = sr.ReadLine()) != null)
                            {
                                // Processar cada linha do arquivo
                                Console.WriteLine(linha);
                            }
                        }
                        res = "leu prj";
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("Erro ao ler o arquivo: " + e.Message);
                        return BadRequest("Arquivo .prj erro a ler.");
                    }
                }
                else
                {
                    // Arquivo .prj não encontrado
                    return BadRequest("Arquivo .prj associado não encontrado.");
                }
                prjPath = Path.ChangeExtension(filepath, ".shp");
                if (System.IO.File.Exists(prjPath))
                {
                    try
                    {
                        ShapeFile ShapeRead = new ShapeFile();

                        //ShapeRead.Open( Ficheiro);

                        System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                        ShapeRead.Open(prjPath);

                        ShapeRead.Close();
                        return Ok(res + " shp ok");
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("Erro ao ler o arquivo: " + e.Message);
                        return BadRequest(res + "Arquivo .prj erro a ler.");
                    }
                    catch (Exception ex)
                    {
                        string totalex = ex.Message;

                        if (ex.InnerException != null)
                        {
                            totalex = totalex + " inner1: " + ex.InnerException.Message;
                        }
                        return BadRequest(res + totalex);
                    }
                }
                else
                {
                    // Arquivo .prj não encontrado
                    return BadRequest("Arquivo .shp associado não encontrado.");
                }

            }
            catch (Exception ex)
            {
                string totalex = ex.Message;

                if (ex.InnerException != null)
                {
                    totalex = totalex + " inner: " + ex.InnerException.Message;
                }
                return BadRequest(res + totalex);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostImportarFicheiroPretensaoAsync([FromForm] InfoFicheiros infoficheiros)
        {
            List<string> wktGeometries = new List<string>();

            List<string> ficheiros = new List<string>();

            string filename = infoficheiros.File[0].FileName;
            string filetype = infoficheiros.File[0].ContentType;

            string Ficheiro_Pretensaoid = infoficheiros.Construcaoid;
            string Ficheiro_Userid = infoficheiros.Userid;


            for (int i = 0; i < infoficheiros.File.Count; i++)
            {
                byte[] fileData;
                // Read file data from the incoming request
                using (var ms = new MemoryStream())
                {
                    infoficheiros.File[i].CopyTo(ms);
                    fileData = ms.ToArray();
                    string DirectorioTemporario = _configuration["Impressoes:TempDir"];
                    // Specify the file path where you want to save the file
                    string filePath = infoficheiros.File[i].FileName;
                    string ficheiro = "";
                    if (DirectorioTemporario.EndsWith("\\"))
                    {
                        ficheiro = DirectorioTemporario + filePath;
                    }
                    else
                    {
                        ficheiro = DirectorioTemporario + "\\" + filePath;
                    }
                    // Write the byte array to a file using FileStream
                    using (var fs = new FileStream(ficheiro, FileMode.Create))
                    {
                        fs.Write(fileData, 0, fileData.Length);
                    }
                    var extension = Path.GetExtension(ficheiro).ToLower();
                    if (extension.Equals(".dwg") || extension.Equals(".dxf") ||
                        extension.Equals(".dgn") || extension.Equals(".shp"))
                    {
                        ficheiros.Add(ficheiro);
                    }
                    if (extension.Equals(".zip"))
                    {
                        if (IsZipFile(ficheiro))
                        {
                            string extractPath = _configuration["Impressoes:TempDir"];
                            if (!extractPath.EndsWith("\\"))
                            {
                                extractPath += "\\";
                            }
                            extractPath += DateTime.Now.ToString("yyyyMMddHHmmss") + "\\";
                            ExtractZipFile(ficheiro, extractPath);
                            // Check the extracted files
                            foreach (string caminho in Directory.GetFiles(extractPath))
                            {
                                string extension1 = Path.GetExtension(caminho).ToLower();
                                if (extension1.Equals(".dwg") || extension1.Equals(".dxf") ||
                                    extension1.Equals(".dgn") || extension1.Equals(".shp"))
                                {
                                    ficheiros.Add(caminho);
                                }
                            }
                        }

                    }
                }
            }

            foreach (string ficheiro in ficheiros)
            {
                var extension = Path.GetExtension(ficheiro).ToLower();
                switch (extension)
                {
                    case ".dwg":
                        // code block
                        wktGeometries = WKT_from_DWG(ficheiro);
                        break;
                    case ".dxf":
                        // code block
                        wktGeometries = WKT_from_DXF(ficheiro);
                        break;
                    case ".shp":
                        // code block
                        List<string> objectosprop = new List<string>();
                        List<pccShapeObject.pccShapeObject> objectoshp = new List<pccShapeObject.pccShapeObject>();

                        if (Pvt_ReadShapeFile(ficheiro, ref objectosprop, ref objectoshp))
                        {
                            foreach (pccShapeObject.pccShapeObject el in objectoshp)
                            {
                                // Your code to process each g10ShapeObject goes here
                                wktGeometries.Add(el.Geometria.WKT);

                            }
                        }

                        break;
                    case ".dgn":
                        // code block
                        break;
                    default:
                        // code block
                        break;
                }

            }
            if (wktGeometries.Count > 0)
            {
           
                int n_geometries = 0;
                var id = int.Parse(Ficheiro_Pretensaoid); 
                // 1. Encontrar a construção existente na base de dados pelo seu ID.
                var construcao = await _context.Construcao.FindAsync(id);

                if (construcao == null)
                {
                    // Se a construção não for encontrada, retorna um erro 404 Not Found.
                    return NotFound($"Construção com o ID {Ficheiro_Pretensaoid} não encontrada.");
                }
                foreach (string GeomWKT in wktGeometries)
                {
                    try
                    {

                        // 2. Converter a string WKT para um objeto Geometry.
                        // O WKTReader é a classe da NetTopologySuite responsável por esta conversão.
                        var reader = new NetTopologySuite.IO.WKTReader(); 
                        Geometry novaGeometria = (Geometry) reader.Read(GeomWKT);

                        // Opcional: Definir o SRID (Spatial Reference System Identifier) se necessário.
                        // Por exemplo, 4326 para WGS 84.
                        // novaGeometria.SRID = 4326;

                        // 3. Atualizar a propriedade da geometria no objeto da construção.
                        construcao.Geometria = novaGeometria;

                        // 4. Guardar as alterações na base de dados.
                        await _context.SaveChangesAsync();

                        // 5. Calcular o centroide e a BBox.
                        // Use as funções de NetTopologySuite.
                        var centroide = novaGeometria.Centroid;
                        var bbox = novaGeometria.Envelope;

                        // 6. Converter as geometrias de volta para WKT.
                        var writer = new NetTopologySuite.IO.WKTWriter();
                        var centroideWKT = writer.Write(centroide);
                        var bboxWKT = writer.Write(bbox);

                        // 7. Retornar uma resposta HTTP 200 OK com os dados.
                        // Use um formato simples para o cliente analisar.
                        return Ok($"{centroideWKT}|{bboxWKT}");
                    }
                    catch (Exception ex)
                    {
                        // Se o WKT for inválido, o `reader.Read` irá lançar uma exceção.
                        // Capturamos o erro e retornamos uma resposta 400 Bad Request.
                        return BadRequest($"Erro ao processar o WKT da geometria: {ex.Message}");
                    }
                }
                return NoContent();
           

             }
            else
            {
                return Ok("|Ficheiro sem objectos ou ficheiro inválido.");
            }

        }
        private List<string> WKT_from_DXF(string filename)
        {
            List<string> wktGeometries = new List<string>();

            try
            {
                CadDocument doc;
                using (DxfReader reader = new DxfReader(filename))
                {
                    doc = reader.Read();
                }

                if (doc != null)
                {
                    foreach (Entity entity in doc.Entities)
                    {
                        ObjectType tipo_objeto = (ObjectType)doc.GetCadObject<Entity>(entity.Handle).ObjectType;

                        // Convert each entity's geometry to WKT
                        string wkt = GetWKTFromEntityDXF(tipo_objeto, entity);
                        if (!string.IsNullOrEmpty(wkt))
                        {
                            wktGeometries.Add(wkt);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return wktGeometries;
        }


        private List<string> WKT_from_DWG(string filename)
        {
            List<string> wktGeometries = new List<string>();

            try
            {
                CadDocument doc;
                using (DwgReader reader = new DwgReader(filename))
                {
                    doc = reader.Read();
                }

                if (doc != null)
                {
                    foreach (Entity entity in doc.Entities)
                    {
                        ObjectType tipo_objeto = (ObjectType)doc.GetCadObject<Entity>(entity.Handle).ObjectType;

                        // Convert each entity's geometry to WKT
                        string wkt = GetWKTFromEntity(tipo_objeto, entity);
                        if (!string.IsNullOrEmpty(wkt))
                        {
                            wktGeometries.Add(wkt);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return wktGeometries;
        }
        private string GetWKTFromEntity(ObjectType tipo_objeto, Entity entity)
        {

            //ObjectType tipo_objeto = (ObjectType)doc.GetCadObject<Entity>(entity.Handle).ObjectType;

            string wkt = "";
            //LWPOLYLINE
            switch (tipo_objeto)
            {
                case ObjectType.POINT:
                    ACadSharp.Entities.Point ponto = (ACadSharp.Entities.Point)entity; // Cast para POINT

                    wkt = $"POINT ({ponto.Location.X.ToString(CultureInfo.InvariantCulture)} {ponto.Location.Y.ToString(CultureInfo.InvariantCulture)})";


                    break;
                case ObjectType.LINE:
                case ObjectType.LWPOLYLINE:
                case ObjectType.POLYLINE_2D:
                case ObjectType.POLYLINE_3D:

                    ACadSharp.Entities.LwPolyline linha = (ACadSharp.Entities.LwPolyline)entity; // Cast para LWPOLYLINE

                    if (linha.IsClosed)
                    {
                        // Polilinha fechada -- como poligono
                        wkt = "";
                        List<ACadSharp.Entities.LwPolyline.Vertex> vertices = linha.Vertices;

                        // Percorre todos os vértices da polyline
                        for (int i = 0; i < vertices.Count; i++)
                        {
                            if (i > 0) { wkt = wkt + ","; }
                            wkt = wkt + " " + vertices[i].Location.X.ToString(CultureInfo.InvariantCulture) + " " + vertices[i].Location.Y.ToString(CultureInfo.InvariantCulture);


                        }
                        wkt = wkt + ", " + vertices[0].Location.X.ToString(CultureInfo.InvariantCulture) + " " + vertices[0].Location.Y.ToString(CultureInfo.InvariantCulture);

                        wkt = "POLYGON ((" + wkt + "))";

                    }
                    else
                    {
                        // Polilinha aberta
                        wkt = "LINESTRING (";
                        List<ACadSharp.Entities.LwPolyline.Vertex> vertices = linha.Vertices;

                        // Percorre todos os vértices da polyline
                        for (int i = 0; i < vertices.Count; i++)
                        {
                            ACadSharp.Entities.LwPolyline.Vertex vertex = vertices[i];

                            wkt += vertex.Location.X.ToString(CultureInfo.InvariantCulture) + " " + vertex.Location.Y.ToString(CultureInfo.InvariantCulture);


                        }
                        wkt = ")";

                    }
                    break;


                default:
                    // code block
                    break;
            }
            //Entity clone = (Entity)doc.GetCadObject<Entity>(entity.Handle).Clone();


            return wkt;
        }


        private string GetWKTFromEntityDXF(ObjectType tipo_objeto, Entity entity)
        {

            //ObjectType tipo_objeto = (ObjectType)doc.GetCadObject<Entity>(entity.Handle).ObjectType;

            string wkt = "";
            //LWPOLYLINE
            switch (tipo_objeto)
            {
                case ObjectType.POINT:
                    ACadSharp.Entities.Point ponto = (ACadSharp.Entities.Point)entity; // Cast para POINT

                    wkt = $"POINT ({ponto.Location.X.ToString(CultureInfo.InvariantCulture)} {ponto.Location.Y.ToString(CultureInfo.InvariantCulture)})";


                    break;
                case ObjectType.LINE:
                case ObjectType.LWPOLYLINE:
                case ObjectType.POLYLINE_2D:
                case ObjectType.POLYLINE_3D:

                    ACadSharp.Entities.LwPolyline linha = (ACadSharp.Entities.LwPolyline)entity; // Cast para LWPOLYLINE

                    if (linha.IsClosed)
                    {
                        // Polilinha fechada -- como poligono
                        wkt = "";
                        List<ACadSharp.Entities.LwPolyline.Vertex> vertices = linha.Vertices;

                        // Percorre todos os vértices da polyline
                        for (int i = 0; i < vertices.Count; i++)
                        {
                            if (i > 0) { wkt = wkt + ","; }
                            wkt = wkt + " " + vertices[i].Location.X.ToString(CultureInfo.InvariantCulture) + " " + vertices[i].Location.Y.ToString(CultureInfo.InvariantCulture);


                        }
                        wkt = wkt + ", " + vertices[0].Location.X.ToString(CultureInfo.InvariantCulture) + " " + vertices[0].Location.Y.ToString(CultureInfo.InvariantCulture);

                        wkt = "POLYGON ((" + wkt + "))";

                    }
                    else
                    {
                        // Polilinha aberta
                        wkt = "LINESTRING (";
                        List<ACadSharp.Entities.LwPolyline.Vertex> vertices = linha.Vertices;

                        // Percorre todos os vértices da polyline
                        for (int i = 0; i < vertices.Count; i++)
                        {
                            ACadSharp.Entities.LwPolyline.Vertex vertex = vertices[i];

                            wkt += vertex.Location.X.ToString(CultureInfo.InvariantCulture) + " " + vertex.Location.Y.ToString(CultureInfo.InvariantCulture);


                        }
                        wkt = ")";

                    }
                    break;


                default:
                    // code block
                    break;
            }
            //Entity clone = (Entity)doc.GetCadObject<Entity>(entity.Handle).Clone();


            return wkt;
        }

        private static bool Pvt_ReadShapeFile(string Ficheiro,
                                        ref List<string> ListaPropriedades, ref List<pccShapeObject.pccShapeObject> ListaObjectos)
        {
            bool res = false;

            try
            {
                ListaPropriedades = new List<string>();
                ListaObjectos = new List<pccShapeObject.pccShapeObject>();

                ShapeFile ShapeRead = new ShapeFile();

                //ShapeRead.Open( Ficheiro);

                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                ShapeRead.Open(Ficheiro, false);


                for (int i = 0; i < ShapeRead.Fields.Count; i++)
                {
                    string definicao = $"{ShapeRead.Fields[i].Name}|{ShapeRead.Fields[i].Type.ToString()}|{ShapeRead.Fields[i].Size}|{ShapeRead.Fields[i].Decimal}";
                    ListaPropriedades.Add(definicao);
                }

                for (int ThisRecord = 0; ThisRecord < ShapeRead.RecordCount; ThisRecord++)
                {
                    pccShapeObject.pccShapeObject objecto = new pccShapeObject.pccShapeObject();

                    for (int ThisData = 0; ThisData < ShapeRead.Fields.Count; ThisData++)
                    {
                        pccShapeObject.pccShapeObjectProperties aux = new pccShapeObject.pccShapeObjectProperties();
                        aux.NomeCampo = ShapeRead.Fields[ThisData].Name.ToString();
                        aux.ValorCampo = ShapeRead.Fields[ThisData].Value != null ? ShapeRead.Fields[ThisData].Value.ToString() : "";
                        objecto.Propriedades.Add(aux);
                    }

                    if (ShapeRead.ShapeType == eShapeType.shpPoint ||
                        ShapeRead.ShapeType == eShapeType.shpPointZ)
                    {
                        pccGeoPoint objaux = new pccGeoPoint();

                        for (int ThisPart = 0; ThisPart <= ShapeRead.Parts.Count; ThisPart++)
                        {
                            for (int VertCount = ShapeRead.Parts[ThisPart].Begins; VertCount <= ShapeRead.Parts[ThisPart].Ends; VertCount++)
                            {
                                objaux.X = ShapeRead.Vertices[VertCount].X_Cord;
                                objaux.Y = ShapeRead.Vertices[VertCount].Y_Cord;
                            }
                        }
                        objecto.Geometria = objaux;
                    }

                    if (ShapeRead.ShapeType == eShapeType.shpPolyLine ||
                        ShapeRead.ShapeType == eShapeType.shpPolyLineZ)
                    {
                        pccGeoLineString objaux = new pccGeoLineString();

                        for (int ThisPart = 0; ThisPart <= ShapeRead.Parts.Count; ThisPart++)
                        {
                            for (int VertCount = ShapeRead.Parts[ThisPart].Begins; VertCount <= ShapeRead.Parts[ThisPart].Ends; VertCount++)
                            {
                                pccGeoPoint ptaux = new pccGeoPoint();
                                ptaux.X = ShapeRead.Vertices[VertCount].X_Cord;
                                ptaux.Y = ShapeRead.Vertices[VertCount].Y_Cord;
                                objaux.AddVertice(ref ptaux);
                            }
                        }
                        objecto.Geometria = objaux;
                    }

                    if (ShapeRead.ShapeType == eShapeType.shpPolygon ||
                        ShapeRead.ShapeType == eShapeType.shpPolygonZ)
                    {
                        if (ShapeRead.Parts.Count == 1)
                        {
                            pccGeoPolygon objaux = new pccGeoPolygon();

                            for (int ThisPart = 0; ThisPart < ShapeRead.Parts.Count; ThisPart++)
                            {
                                for (int VertCount = ShapeRead.Parts[ThisPart].Begins; VertCount <= ShapeRead.Parts[ThisPart].Ends; VertCount++)
                                {
                                    pccGeoPoint ptaux = new pccGeoPoint();
                                    ptaux.X = ShapeRead.Vertices[VertCount].X_Cord;
                                    ptaux.Y = ShapeRead.Vertices[VertCount].Y_Cord;
                                    objaux.AddVertice(ref ptaux);
                                }
                            }
                            objecto.Geometria = objaux;
                        }
                        else
                        {
                            pccGeoMultiPolygon objaux = new pccGeoMultiPolygon();

                            for (int ThisPart = 0; ThisPart <= ShapeRead.Parts.Count; ThisPart++)
                            {
                                pccGeoPolygon objaux1 = new pccGeoPolygon();
                                for (int VertCount = ShapeRead.Parts[ThisPart].Begins; VertCount <= ShapeRead.Parts[ThisPart].Ends; VertCount++)
                                {
                                    pccGeoPoint ptaux = new pccGeoPoint();
                                    ptaux.X = ShapeRead.Vertices[VertCount].X_Cord;
                                    ptaux.Y = ShapeRead.Vertices[VertCount].Y_Cord;
                                    objaux1.AddVertice(ref ptaux);
                                }
                                objaux.AddPolygon(ref objaux1);
                            }
                            objecto.Geometria = objaux;
                        }
                    }

                    ListaObjectos.Add(objecto);
                    ShapeRead.MoveNext();
                }

                res = true;
            }
            catch (Exception ex)
            {
                string totalex = ex.Message.ToString();
                if (ex.InnerException != null)
                {
                    totalex = totalex + " inner: " + ex.InnerException.Message.ToString();
                }


                res = false;
            }

            return res;
        }

        // Function to check if a file is a zip file
        private bool IsZipFile(string filePath)
        {
            try
            {
                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    return IsZipFile(stream);
                }
            }
            catch
            {
                return false;
            }
        }
        // Function to check if a stream is a zip file
        private bool IsZipFile(Stream stream)
        {
            byte[] signature = new byte[4];
            stream.Read(signature, 0, 4);
            stream.Seek(0, SeekOrigin.Begin);
            return signature[0] == 0x50 && signature[1] == 0x4B &&
                   signature[2] == 0x03 && signature[3] == 0x04;
        }

        // Function to extract a zip file
        private void ExtractZipFile(string zipFilePath, string extractPath)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath);
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
  
    } 

}
