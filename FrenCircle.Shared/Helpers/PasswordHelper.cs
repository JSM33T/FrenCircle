using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Shared.Helpers
{
    public static class PasswordHelper
    {
        public static string GenerateSalt(int size = 32)
        {
            var saltBytes = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(password + salt);
            var hash = sha256.ComputeHash(combined);
            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedHash, string salt)
        {
            var computedHash = HashPassword(password, salt);
            return computedHash == storedHash;
        }
    }
}
