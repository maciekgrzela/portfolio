using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Security
{
    public class SecurityKeyGenerator
    {
        private static SecurityKeyGenerator instance = null;
        public SymmetricSecurityKey Key { get; private set; } = null;
        
        public static SecurityKeyGenerator Instance => instance ?? new SecurityKeyGenerator();

        private SecurityKeyGenerator()
        {
            var tripleDes = new TripleDESCryptoServiceProvider();
            tripleDes.GenerateKey();
            Key = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(tripleDes.Key.ToString() ?? string.Empty));
        }
    }
}