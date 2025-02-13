using FrenCircle.Entities.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FrenCircle.Helpers.Security;

public class JwtTokenHelper
{
    public static string GenerateToken(User user, string secretKey, string issuer, string audience, int expiryMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("FNAME", user.FirstName),
            new Claim("UID", user.UId.ToString()),
            new Claim("LNAME", user.LastName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }),
            Expires = DateTime.Now.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        // Manually add 'kid' to the header
        var jwtToken = (JwtSecurityToken)securityToken;
        jwtToken.Header.Add("kid", "your-key-id");

        return tokenHandler.WriteToken(securityToken);
    }
}