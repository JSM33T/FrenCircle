using API.Contracts.Services;
using API.Entities.Shared;
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
       
        public ContactController(IOptionsMonitor<Jsm33tConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor, ICommonService commonService) : base(config, logger, httpContextAccessor, commonService)
        {
          
        }
    }
}
