using System.Security.Cryptography;
using System.Text;

namespace FrenCircle.Helpers.Security;

public static class PasswordHasher
{
    public static (string hash, string salt) HashPassword(string password)
    {
        using var hmac = new HMACSHA512();
        var salt = Convert.ToBase64String(hmac.Key);
        var hash = Convert.ToBase64String(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return (hash, salt);
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);

        using var hmac = new HMACSHA512(saltBytes);
        var computedHash = Convert.ToBase64String(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return computedHash == storedHash;
    }
}