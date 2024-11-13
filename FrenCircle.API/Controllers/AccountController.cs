using FrenCircle.API.Controllers.Base;
using FrenCircle.Entities.Shared;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FrenCircle.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(IOptionsMonitor<FCConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor,IGlobalRepository globalRepository)
        : FoundationController(config, logger, httpContextAccessor)
    {
        private readonly IGlobalRepository _globalRepository = globalRepository;

        [HttpGet("login")]
        [AllowAnonymous]
        #region User Login
        public async Task<IActionResult> Login()
        {
            return await ExecuteActionAsync(async () =>{

                int statCode = default;
                string message = string.Empty;
                List<string> hints = [];

                //message = await _globalRepository.GetGLobalValue("test");

                statCode = StatusCodes.Status200OK;
                hints.Add("Please check your creds");

            return (statCode, 0, message, hints);});
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
