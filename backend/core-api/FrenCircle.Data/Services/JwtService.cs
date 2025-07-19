using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FrenCircle.Contracts.Auth;
using FrenCircle.Data;

namespace FrenCircle.Data
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user, List<string> roles, List<string> permissions);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateAccessToken(string token);
        bool ValidateRefreshToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SymmetricSecurityKey _key;

        public JwtService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        }

        public string GenerateAccessToken(User user, List<string> roles, List<string> permissions)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, 
                    new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), 
                    ClaimValueTypes.Integer64)
            };

            // Add optional claims
            if (!string.IsNullOrEmpty(user.Username))
                claims.Add(new Claim(ClaimTypes.Name, user.Username));

            if (!string.IsNullOrEmpty(user.FirstName))
                claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));

            if (!string.IsNullOrEmpty(user.LastName))
                claims.Add(new Claim(ClaimTypes.Surname, user.LastName));

            // Add roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add permissions as custom claims
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public ClaimsPrincipal? ValidateAccessToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateRefreshToken(string token)
        {
            try
            {
                // Basic format validation
                var bytes = Convert.FromBase64String(token);
                return bytes.Length == 64;
            }
            catch
            {
                return false;
            }
        }
    }
}
