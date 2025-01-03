
using API.Contracts.Services;

namespace API.Infra
{
    public class CryptoService : ICryptoService
    {
        public async Task<string> HashPassword(string plainPassword)
        {
            return  await Task.Run(
                () => BCrypt.Net.BCrypt.HashPassword(plainPassword));
        }

        public async Task<bool> VerifyPassword(string plainPassword, string hashedPassword)
        {
            return await Task.Run(
                () => BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword));
        }
    }
}
