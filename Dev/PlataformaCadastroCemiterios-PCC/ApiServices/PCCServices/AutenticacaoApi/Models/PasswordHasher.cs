using Microsoft.CodeAnalysis.Scripting;
using BCrypt.Net;
using Microsoft.CodeAnalysis.Scripting;

namespace AutenticacaoApi.Models
{


    public static class PasswordHasher
    {
        // A complexidade do hash. Um custo mais alto é mais seguro, mas também mais lento.
        private const int HashingCost = 12;

        /// <summary>
        /// Gera um hash seguro e salgado de uma senha.
        /// </summary>
        /// <param name="password">A senha de texto puro a ser hashed.</param>
        /// <returns>O hash da senha, incluindo o sal.</returns>
        public static string HashPassword(string password)
        {
            // BCrypt.Net.HashPassword gera automaticamente um sal.
            return BCrypt.Net.BCrypt.HashPassword(password, HashingCost);
        }

        /// <summary>
        /// Verifica se uma senha de texto puro corresponde a um hash.
        /// </summary>
        /// <param name="password">A senha de texto puro para verificar.</param>
        /// <param name="hashedPassword">O hash da senha armazenado no banco de dados.</param>
        /// <returns>True se as senhas coincidirem, caso contrário, False.</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
