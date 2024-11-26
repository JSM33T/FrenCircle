using FrenCircle.API.Controllers.Base;
using FrenCircle.Entities.Fren;
using FrenCircle.Entities.Shared;
using FrenCircle.Repositories;
using FrenCircle.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;

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

        [HttpPost("login")]
        [AllowAnonymous]
        #region User Login
        public async Task<IActionResult> Login(FrenLoginRequest loginRequest)
        {
            APIResponse<Fren> frenResponse = new(default, "", null, []);
            return await ExecuteActionAsync(async () =>
            {
                frenResponse.Data = await _userRepository.GetUserByCredentials(loginRequest);

                if (frenResponse.Data == null)
                {
                    frenResponse.Status = StatusCodes.Status404NotFound;
                    frenResponse.Message = "Invalid Credendial";
                    return frenResponse;
                }

                if (!frenResponse.Data.IsVerified)
                {
                    return new(StatusCodes.Status400BadRequest, "Account awaits verification", null, []);
                }

                await _userService.LoginFren(frenResponse.Data);

                frenResponse.Status = StatusCodes.Status200OK;
                frenResponse.Message = "Logged in successfully";

                return (frenResponse);
            });
        }
        #endregion


        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(FrenSignUpRequest signUpRequest)
        {
            APIResponse<Fren> resp = new(default, string.Empty, null, []);

            Random random = new();
            int otp = random.Next(1000, 9999);

            return await ExecuteActionAsync(async () =>
            {
                signUpRequest.OTP = otp;

                await _userRepository.SignUpFren(signUpRequest);

                resp.Message = "Signed up. Please verify your account";
                resp.Status = StatusCodes.Status200OK;

                return (resp);
            });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(FrenVerificationRequest verificationRequest)
        {
            APIResponse<Fren> resp = new(default, string.Empty, null, []);

            return await ExecuteActionAsync(async () =>
            {

                int res = await _userRepository.VerifyFren(verificationRequest);

                if (res == 1)
                {
                    resp.Message = "User verified";
                    resp.Status = StatusCodes.Status200OK;
                }
                else
                {
                    resp.Message = "Invalid OTP";
                    resp.Status = StatusCodes.Status400BadRequest;
                }


                return (resp);
            });
        }

        [HttpPost("recover")]
        public async Task<IActionResult> Recovery()
        {
            return Ok("dummy");
        }

    }
}
