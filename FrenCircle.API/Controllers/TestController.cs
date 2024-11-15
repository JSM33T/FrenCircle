using FrenCircle.API.Controllers.Base;
using FrenCircle.Entities.Mail;
using FrenCircle.Entities.Shared;
using FrenCircle.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FrenCircle.API.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController(
                IOptionsMonitor<FCConfig> config,
                ILogger<FoundationController> logger,
                IHttpContextAccessor httpContextAccessor,
                IMessageService messageService
            )
     : FoundationController(config, logger, httpContextAccessor)
    {
        private readonly IMessageService _messageService= messageService;

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

        [HttpPost("sendmail")]
        public async Task<IActionResult> SendEmail(SendMailRequest mailRequest)
        {
            return await ExecuteActionAsync(async () =>
            {
                int statCode = StatusCodes.Status200OK;
                string message = "Mail Sent";
                List<string> hints = [];

                await _messageService.SendMail(mailRequest);


                return (statCode, 0, message, hints);
            });
        }
    }
}
