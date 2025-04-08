using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Interfaces.Services
{
    public interface ITokenService
    {
        (string AccessToken, string RefreshToken, DateTime ExpiresAt, DateTime IssuedAt) GenerateTokens(int userId);
    }

}
