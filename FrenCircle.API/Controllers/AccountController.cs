using FrenCircle.API.Controllers.Base;
using FrenCircle.Entities.Fren;
using FrenCircle.Entities.Shared;
using FrenCircle.Repositories;
using FrenCircle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FrenCircle.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController
        (
            IOptionsMonitor<FCConfig> config,
            ILogger<FoundationController> logger,
            IHttpContextAccessor httpContextAccessor,
            IGlobalRepository globalRepository,
            IUserRepository userRepository,
            IUserService userService
        )
        : FoundationController(config, logger, httpContextAccessor)
    {
        private readonly IGlobalRepository _globalRepository = globalRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserService _userService = userService;

        [HttpGet("login")]
        [AllowAnonymous]
        #region User Login
        public async Task<IActionResult> Login(FrenLoginRequest loginRequest)
        {
            return await ExecuteActionAsync(async () =>
            {
                int statCode = default;
                string message = string.Empty;
                List<string> hints = [];
                Fren? fren = null;

                fren = await _userRepository.GetUserByCredentials(loginRequest);

                if (fren == null)
                {
                    statCode = StatusCodes.Status404NotFound;
                    message = "Invalid Credendial";
                }
                else
                {
                    if (!fren.IsVerified)
                    {
                        statCode = StatusCodes.Status401Unauthorized;
                        message = "Account awaits verification ";
                    }
                    else
                    {
                        await _userService.LoginFren(fren);

                        statCode = StatusCodes.Status200OK;
                        message = "Logged in successfully";
                    }
                }


                hints.Add("Please check your creds");

                return (statCode, 0, message, hints);
            });
        }
        #endregion


        [HttpPost("signup")]
        public async Task<IActionResult> Signup()
        {
            return Ok("dummy");
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify()
        {
            return Ok("dummy");
        }

        [HttpPost("recover")]
        public async Task<IActionResult> Recovery()
        {
            return Ok("dummy");
        }

    }
}
