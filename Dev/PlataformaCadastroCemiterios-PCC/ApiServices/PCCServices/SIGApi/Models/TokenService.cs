using SIGApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// using SuaAplicacao.Models; // Namespace da classe JwtSettings

namespace SIGApi.Models
{


    public interface ITokenService
    {
        string GenerateToken(string userId, string username, IEnumerable<string> roles); // Adapte os parâmetros conforme necessário
    }

    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings) // Injete as configurações
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(string userId, string username, IEnumerable<string> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims são informações sobre o utilizador que vão dentro do token
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId), // Subject (ID do utilizador)
            new Claim(JwtRegisteredClaimNames.Name, username), // Nome do utilizador (pode usar ClaimTypes.Name)
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT ID - identificador único para o token
            // Adicionar claims de Roles
            // new Claim(ClaimTypes.Role, "Admin"),
            // new Claim(ClaimTypes.Role, "User")
        };

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(2), // Tempo de expiração do token (ex: 2 horas)
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); // Serializa o token para uma string
        }
    }

    // Registe este serviço em Program.cs:
    // builder.Services.AddScoped<ITokenService, TokenService>();


}