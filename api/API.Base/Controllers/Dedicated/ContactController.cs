using API.Contracts.Services;
using API.Entities.Dedicated;
using API.Entities.Shared;
using API.Infra;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Base.Controllers.Dedicated
{
    [Route("api/contact")]
    [ApiController]
    [Authorize]
    public class ContactController : FoundationController
    {
        public readonly IMessageRepository _messageRepo;
        public ContactController(IOptionsMonitor<Jsm33tConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor, ICommonService commonService, IMessageRepository messageRepository) : base(config, logger, httpContextAccessor, commonService)
        {
            _messageRepo = messageRepository;
        }

        [HttpPost("report")]
        [AllowAnonymous]
        public async Task<IActionResult> AddReport(AddReportRequest reportRequest)
        {
            return await ExecuteActionAsync(async () =>
            {
                int statCode = default;
                string message = string.Empty;
                List<string> hints = ["we will look into it"];
                User_ClaimsResponse userClaims = new();

                await _messageRepo.AddReportAsync(reportRequest);

                return (StatusCodes.Status200OK, userClaims, "report submitted", hints);

            }, "GoogleLogin");
        }
    }
}
