using API.Contracts.Repositories;
using API.Contracts.Services;
using API.Entities.Dedicated.Contact;
using API.Entities.Enums;
using API.Entities.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Base.Controllers.Dedicated
{
    [Route("api/contact")]
    [ApiController]
    [AllowAnonymous]
    public class ContactController(
        IOptionsMonitor<Jsm33tConfig> config,
        ILogger<FoundationController> logger,
        IHttpContextAccessor httpContextAccessor,
        IRateLimitService rateLimitService,
        ICommonService commonService,
        IMessageRepository messageRepository)
        : FoundationController(config, logger, httpContextAccessor, commonService)
    {
        private readonly IMessageRepository _messageRepo = messageRepository;
        private readonly IRateLimitService _rateLimitService = rateLimitService;

        [HttpPost]
        public async Task<IActionResult> Contact(ContactRequest request)
        {
            return await ExecuteActionAsync(async () =>
            {

                if (_rateLimitService.IsRateLimited("falana", 5, 60))
                {
                    return new APIResponse<bool>(StatusCodes.Status429TooManyRequests, "", false, []);
                }

                DBResult res = await _messageRepo.AddContactMessage(request);

                return res switch
                {
                    DBResult.Conflict => new APIResponse<bool>(StatusCodes.Status409Conflict, "Message already exists", false, []),
                    DBResult.Success => new APIResponse<bool>(StatusCodes.Status200OK, "Message sent", true, []),
                    _ => new APIResponse<bool>(StatusCodes.Status500InternalServerError, "Something went wrong", false, [])
                };
            }, "Contact method");
        }

    }
}
