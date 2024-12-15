using AhlApp.Shared.Security;

namespace AhlApp.Infrastructure.Security
{
    public class SecurityHasher : ISecurityHasher
    {
        public (string Hash, string Salt) Hash(string input)
        {
            var salt = GenerateSalt();
            using var hmac = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(salt));
            var hash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input)));
            return (hash, salt);
        }

        public bool Validate(string input, string hash, string salt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA256(Convert.FromBase64String(salt));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input)));
            return computedHash == hash;
        }

        private static string GenerateSalt()
        {
            var saltBytes = new byte[16];
            using var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }
}
