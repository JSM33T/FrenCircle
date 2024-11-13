using FrenCircle.API.Controllers.Base;
using FrenCircle.Entities.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Reflection;

namespace FrenCircle.API.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : FoundationController
    {
        public TestController(IOptionsMonitor<FCConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor) : base(config, logger, httpContextAccessor)
        {
        }

        [HttpGet("tick")]
        public async Task<IActionResult> HeartBeat()
        {
            return await ExecuteActionAsync(async () =>
            {
                int statCode = StatusCodes.Status200OK;
                string message = "API server is up and healthy";
                List<string> errors = [];
                //User_ClaimsResponse userClaims = null;

                return (statCode, 0, message, errors);
            });
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult> SendEmail([FromBody] string ss)
        {
            return Ok($"Email has been sent,{ss}");
        }
    }
}
