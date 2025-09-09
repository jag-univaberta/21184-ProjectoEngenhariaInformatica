using AutenticacaoApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AutenticacaoApi.Controllers
{     
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly ProjectoContext _context;

        public AuthController(ITokenService tokenService, ProjectoContext context, HttpClient httpClient, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;

            _context = context; 

            _httpClient = httpClient;

            _configuration = configuration;

            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Este endpoint não requer autenticação prévia
        public async Task<IActionResult> Login([FromBody] Models.LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Username e Password são obrigatórios.");
            }

            // Procurar o utilizador na base de dados pelo nome de login.
            Utilizador checkutilizador = await _context.Utilizador.FirstOrDefaultAsync(u => u.Login == request.Username);
            // Se o utilizador não existir, cria-se um novo.
            if (checkutilizador == null)
            {
                // A senha é hashed antes de ser guardada na base de dados
                string senhaHash = PasswordHasher.HashPassword(request.Password);
                // Cria a nova instância de Utilizador
                var novoUtilizador = new Utilizador
                {
                    Nome = request.Username,
                    Login = request.Username,
                    Email = "",
                    Activo = true,
                    PalavraPasse = senhaHash // Guarda o hash da senha
                };
                // Adiciona o novo utilizador ao contexto e guarda na base de dados
                _context.Utilizador.Add(novoUtilizador);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Novo utilizador '{request.Username}' criado e autenticado com sucesso.");
            }
            Utilizador utilizador = await _context.Utilizador.FirstOrDefaultAsync(u => u.Login == request.Username);
            string senhaHashGuardada = utilizador?.PalavraPasse;
            bool senhaCorreta = PasswordHasher.VerifyPassword(request.Password, senhaHashGuardada);
            if (!senhaCorreta)
            {
                return Unauthorized("Credenciais inválidas."); // Não dê muita informação sobre o erro
            }
            // Exemplo estático 
            var userId = utilizador.RecId; // Obtenha o ID real do utilizador
            var roles = new List<string> { "User" }; // Obtenha as roles reais
            if (request.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                userId = utilizador.RecId;
                roles = new List<string> { "Admin", "User" };
            }
            // ------------------------------------------------------------

            // --- Gerar o Token ---
            var tokenString = _tokenService.GenerateToken(userId.ToString(), request.Username, roles);
            // Adicione o cabeçalho de autorização. O esquema 'Bearer' é o padrão para JWT.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
            string SIGApibaseAddress = _configuration["SIGApibaseAddress"];

            //arvore cartografia
            string urlcart = SIGApibaseAddress + "api/ArvoreCartografia/" + userId;
            HttpResponseMessage responsecart = await _httpClient.GetAsync(urlcart);
            responsecart.EnsureSuccessStatusCode();
            string responsecartBody = await responsecart.Content.ReadAsStringAsync();
            JsonDocument jsonObjectcart = JsonDocument.Parse(responsecartBody);
            string treecartografia_data = System.Text.Json.JsonSerializer.Serialize(jsonObjectcart);

            string CadastroApibaseAddress = _configuration["CadastroApibaseAddress"];
            // Adicione o cabeçalho de autorização. O esquema 'Bearer' é o padrão para JWT.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            //arvore cadastro de cemiterios
            string urlcadastro = CadastroApibaseAddress + "api/ArvoreCadastro/" + userId;
            HttpResponseMessage responsecadastro = await _httpClient.GetAsync(urlcadastro);
            responsecadastro.EnsureSuccessStatusCode();
            string responsecadastroBody = await responsecadastro.Content.ReadAsStringAsync();
            JsonDocument jsonObjectcadastro = JsonDocument.Parse(responsecadastroBody);
            string treecadastro_data = System.Text.Json.JsonSerializer.Serialize(jsonObjectcadastro);

            //App Estado inicial 
            // Adicione o cabeçalho de autorização. O esquema 'Bearer' é o padrão para JWT.
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            string urlestado = CadastroApibaseAddress + "api/CadastroEstadoApp/" + userId;
            HttpResponseMessage responseestado = await _httpClient.GetAsync(urlestado);
            responsecadastro.EnsureSuccessStatusCode();
            string responseestadoBody = await responseestado.Content.ReadAsStringAsync();
            JsonDocument jsonObjectestado = JsonDocument.Parse(responseestadoBody);
            string estado_data = System.Text.Json.JsonSerializer.Serialize(jsonObjectestado); 
            JsonElement root = jsonObjectestado.RootElement;
            // Obter os valores das propriedades
            int utilizador_id = root.GetProperty("utilizador_id").GetInt32();
            string definicoes = root.GetProperty("definicoes").GetString();
            string[] auxArray = definicoes.Split("|");

            string lista_gruposcartografia = auxArray[1];
            string allLayers = "";
            if (lista_gruposcartografia != "")
            {
                string filtro_parents = lista_gruposcartografia.Replace(";", ",");
                // Adicione o cabeçalho de autorização. O esquema 'Bearer' é o padrão para JWT.
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                string urllayers = SIGApibaseAddress + "api/Cartografialayer/PorParent/" + filtro_parents;
                HttpResponseMessage responselayers = await _httpClient.GetAsync(urllayers);
                responselayers.EnsureSuccessStatusCode();
                string responselayersBody = await responselayers.Content.ReadAsStringAsync();
                JsonDocument jsonObjectlayers = JsonDocument.Parse(responselayersBody);
                string layers_data = System.Text.Json.JsonSerializer.Serialize(jsonObjectlayers);
                JsonElement layersArray = jsonObjectlayers.RootElement; 

                //Criar lista para ter os elementos
                List<string> layerNames = new List<string>();                 
                foreach (JsonElement layerObject in layersArray.EnumerateArray())
                {
                     
                    string layerName = layerObject.GetProperty("layer").GetString(); 
                    layerNames.Add(layerName);
                }
                allLayers = string.Join("|", layerNames);
            }

            string treecartografia_init = auxArray[1];
            string treeinstrumentos_init = "";
           // string allLayers = "";
            string mapa_defs = auxArray[0]; //"-51540.0797712748;186357.10743414902;-51540.0797712748;186357.10743414902;105080.49246806474";
            //string treecartografia_data = "[]";
            string treepois_data = "[]";
            string treepretensao_data = "[]";
            string treeinstrumentos_data = "[]";
            // cod | subcod | codpai | subcodpai | tipo | perm (1 ou 256~) 
            string permissoes = "|1510||||||1;|1520||||||1;|1530||||||1;";
            string separadores_app = "";
            string separadores_escrita = "";
            string separadores_leitura = "";
            separadores_app = "EXT|CAR|CEM";
            separadores_escrita = "EXT|CAR|CEM";
            separadores_leitura = "";
            string Sessionid = "";
            return Ok(new
            {
                authtoken = tokenString,
                applicationname = "Projecto Final - Cadastro Cemitério",
                applicationsigla = "PCC",
                userid = userId,
                username = request.Username,
                usersession = Sessionid.ToString(),
                separadores = "CAR|CEM|EXT",
                separadorestooltips = "Cartografia|Cadastro de Cemitério|Layer Externos|",
                separadoressiglas = "CAR|CEM|EXT|",
                separadoresids = "|||",
                separadoresdata = "",
                treecartografia_init = treecartografia_init,
                layers_iniciais = allLayers,
                mapa_defs = mapa_defs,
                treecartografia_data = treecartografia_data,
                treeconstrucoes_data = treecadastro_data, 
                showgridpretensoes = false,
                showgridpatrimonio = false,
                permissoes = permissoes,
                separadores_app = separadores_app,
                separadores_escrita = separadores_escrita,
                separadores_leitura = separadores_leitura
            });

        }
        private string GenerateJwtSecurityToken()
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(400),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Função de exemplo - Substitua pela sua lógica real!
        private bool ValidateUserCredentials(string username, string password)
        {
            // Consulte a base de dados, use ASP.NET Core Identity, etc.
            // NUNCA compare passwords em texto plano. Use hashing (ex: Identity faz isso).
            // Exemplo muito básico (NÃO USE ISTO EM PRODUÇÃO):
            return (username.Equals("user", StringComparison.OrdinalIgnoreCase)) ||
                   (username.Equals("admin", StringComparison.OrdinalIgnoreCase));

       

        }

        private async Task<LoginCredentials> AuthenticateUserAsync(LoginCredentials user)
        {
            LoginCredentials _user = null;
            if (user.Username == "visualizador" && user.Password == "rodazilausiv")
            {
                _user = new LoginCredentials { Username = "Administrator" };
            }
            else
            {
                string username = user.Username;
                string password = user.Password;
                string tokenid = user.TokenId;
                string datahora;


                string md5PassComposite;
                string encoded_pass;

                md5PassComposite = username.ToUpper() + password;
                cls_Security.cGetCrypto getMD5 = new cls_Security.cGetCrypto();
                encoded_pass = getMD5.GetCrypto(md5PassComposite);

                DateTime currentDate = DateTime.Now;
                datahora = currentDate.ToString("yyyyMMddHHmmss");
                try
                {
                    Utilizador User = await GetUtilizadorPorLoginESenhaComSingle(username, encoded_pass);
                 
                    /* OBTER A SESSAO */
                   // String aux_1 = "select * from f_login('" + username + "','" + encoded_pass + "','" + tokenid + "','" + datahora + "')";

                    
              
                    string Credentials_userid = User.RecId.ToString();
                    string Credentials_username = User.Nome;
                    _user = new LoginCredentials { Username = Credentials_username };
                }
                catch (Exception)
                {
                    _user = null;
                }

            }
            return _user;
        }
        private async Task<Utilizador?> GetUtilizadorPorLoginESenhaComSingle(string login, string palavraPasse)
        {
            if (_context.Utilizador == null)
            {
                return null;
            }

            var utilizador = await _context.Utilizador
                                           .SingleOrDefaultAsync(u => u.Login == login && u.PalavraPasse == palavraPasse);
            return utilizador;
        }

        private class LoginCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string TokenId { get; set; }
            public string Datahora { get; set; }
        }
    }
}
