using System.ComponentModel.DataAnnotations;

namespace AutenticacaoApi.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; } // Informar o cliente quando expira
        public string Username { get; set; }
        // Pode adicionar roles ou outras informações úteis
    }
}
