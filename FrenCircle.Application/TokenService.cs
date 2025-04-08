using FrenCircle.Contracts.Interfaces.Services;
using FrenCircle.Shared.ConfigModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Application
{
    public class TokenService(IOptions<FcConfig> config) : ITokenService
    {
        private readonly JwtConfig _config = config.Value.JwtConfig!;

        public (string AccessToken, string RefreshToken, DateTime ExpiresAt, DateTime IssuedAt) GenerateTokens(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(30);
            var issuedAt = DateTime.UtcNow;

            //var token = new JwtSecurityToken(
            //    issuer: _config.Issuer,
            //    audience: _config.Audience,
            //    claims: [
            //        new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
            //        ],
            //    expires: expiresAt,
            //    signingCredentials: creds
            //);

            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: [
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name,"jsm33t"),
                    new Claim(JwtRegisteredClaimNames.Iat,
                              new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(),
                              ClaimValueTypes.Integer64)
                ],
                notBefore: issuedAt,           // <-- add this
                expires: expiresAt,
                signingCredentials: creds
            );


            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            string refreshToken = Guid.NewGuid().ToString();

            return (accessToken, refreshToken, expiresAt, issuedAt);
        }
    }

}
