using API.Contracts.Services;
using API.Entities.Dedicated;
using API.Entities.Enums;
using API.Entities.Shared;
using API.Infra;
using API.Repositories;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace API.Base.Controllers.Dedicated
{
    [Route("api/account")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController(IOptionsMonitor<Jsm33tConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository,ICommonService commonService, IRateLimitService rateLimitService) : FoundationController(config, logger, httpContextAccessor, commonService)
    {
        private readonly IUserRepository _userRepo = userRepository;
        private readonly IRateLimitService _rateLimitService = rateLimitService;


        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            return await ExecuteActionAsync(async () =>
            {
                int statCode = default;
                string message = string.Empty;
                List<string> hints = [];
                User_ClaimsResponse userClaims = new();

                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { config.CurrentValue.logins.GoogleClientId }
                };

                Payload payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);


                Fren member = await _userRepo.GetUserByProp("Email", payload.Email);

                if (member == null)
                {
                    member = new()
                    {
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName ?? string.Empty,
                        Username = payload.Email.Replace("@", "_"),
                        Email = payload.Email,
                        Avatar = payload.Picture,
                        AuthMode = AuthMode.Google,
                        Role = "member",
                        GoogleId = payload.Subject
                    };
                    member = await _userRepo.AddUserAsync(member);
                }


                statCode = StatusCodes.Status200OK;
                var claims = new[]
                   {
                            new Claim(ClaimTypes.Email, member.Email),
                            new Claim(ClaimTypes.NameIdentifier,member.Username),
                            new Claim(ClaimTypes.Role, member.Role),
                            new Claim("id", member.Id.ToString()),
                            new Claim("username", member.Username),
                            new Claim("role", member.Role),
                            new Claim("firstname", member.FirstName),
                            new Claim("lastname", member.LastName ?? string.Empty),
                            new Claim("avatar", member.Avatar),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.CurrentValue.JwtSettings.IssuerSigningKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config.CurrentValue.JwtSettings.ValidIssuer,
                    audience: _config.CurrentValue.JwtSettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(7),
                    signingCredentials: creds);

                userClaims.Token = new JwtSecurityTokenHandler().WriteToken(token);

                statCode = StatusCodes.Status200OK;
                message = "Logged In..";

                return (statCode, userClaims, message, hints);
            }, "GoogleLogin");
        }

        [HttpGet("time")]
        [Authorize]
        public async Task<IActionResult> UpdateTimeSpent()
        {
            return await ExecuteActionAsync(async () =>
            {
                int statCode = default;
                string message = string.Empty;
                List<string> hints = [];
                User_ClaimsResponse userClaims = new();

                //var userId = User.Identity.Name;
                var userId = 1;

                if (_rateLimitService.IsRateLimited(userId.ToString(), 5))
                {
                    statCode = StatusCodes.Status429TooManyRequests;
                    await  _userRepo.UpdateTimeSpent(userId);
                    return (statCode, null, message, hints);
                }
                else
                {
                    return (statCode, userClaims, message, hints);
                }

            }, "GoogleLogin");
        }

    }
}
