using FrenCircle.Entities;
using FrenCircle.Entities.Data;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FrenCircle.Base.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController(IAccountRepository accountRepository) : FcBaseController
    {
        [HttpPost("create")]
        public async Task<IActionResult> AddUser(AddUserRequest addUserRequest)
        {
            APIResponse<int> aPIResponse = new(StatusCodes.Status409Conflict, "Conflict", 0, []);

            if (await accountRepository.IsUserPresentByEmail(addUserRequest.Email))
                aPIResponse.Hints.Add("Email is alraedy registered");

            if (await accountRepository.IsUserPresent(addUserRequest.UserName))
                aPIResponse.Hints.Add("Email is alraedy registered");

            if (aPIResponse.Hints.Count != 0)
                return RESP_Custom(aPIResponse);

            await accountRepository.AddUser(addUserRequest);

            return RESP_Success("Succssfylly registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginRequest)
        {
            var user = await accountRepository.LoginUser(loginRequest.UserName, loginRequest.Password);

            if (user == null)
                return RESP_UnauthorizedResponse("Invalid username or password");

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("iureowtueorituowierutoi4354======");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(3000)),
                Issuer = "www.something.com",
                Audience = "www.something.com",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Return the token
            return RESP_Success(new { Token = tokenString });
        }

    }
}
