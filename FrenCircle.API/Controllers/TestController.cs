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
            APIResponse<string> resp = new(default, "", "", []);
            return await ExecuteActionAsync(async () =>
            {
                resp.Status = StatusCodes.Status200OK;
                resp.Message = "API server is up and healthy"; ;

                return (resp);
            });
        }

        [HttpPost("sendmail")]
        public async Task<IActionResult> SendEmail(SendMailRequest mailRequest)
        {
            APIResponse<string> resp = new(default, "", "", []);

            return await ExecuteActionAsync(async () =>
            {
                resp.Status = StatusCodes.Status200OK;
                resp.Message = "Mail Sent";

                await _messageService.SendMail(mailRequest);

                return (resp);
            });
        }
    }
}
