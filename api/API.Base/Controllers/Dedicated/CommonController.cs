using API.Contracts.Repositories;
using API.Contracts.Services;
using API.Entities.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace API.Base.Controllers.Dedicated
{
    [Route("/api/common")]
    [ApiController]
    [AllowAnonymous]
    public class CommonController : FoundationController
    {
        private readonly IMessageRepository _messageRepo;

        public CommonController(
            IOptionsMonitor<Jsm33tConfig> config,
            ILogger<FoundationController> logger,
            IHttpContextAccessor httpContextAccessor,
            ICommonService commonService,
            IMessageRepository messageRepository) : base(config, logger, httpContextAccessor, commonService)
        {
            _messageRepo = messageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Contact()
        {

            // Fetch real OS and CPU information
            var osDescription = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            var osArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();
            var processArchitecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString();

            var serverDetails = new
            {
                ServerName = Environment.MachineName,
                ServerStatus = "Running",
                RequestTime = DateTime.UtcNow,
                Uptime = GetSystemUptime(),
                Details = new
                {
                    OSDescription = osDescription,
                    OSArchitecture = osArchitecture,
                    ProcessArchitecture = processArchitecture,
                    TotalMemory = GetTotalMemory(),
                    ActiveConnections = GetActiveConnections()
                }
            };
            return await ExecuteActionAsync(async () =>
            {
                APIResponse<object> apiResponse = new(StatusCodes.Status200OK, "Server is up and running", serverDetails, []);

                return (apiResponse);

            }, "Contact method");
        }

        private string GetSystemUptime()
        {
            using var uptime = new PerformanceCounter("System", "System Up Time");
            uptime?.NextValue();
            TimeSpan ts = TimeSpan.FromSeconds(uptime.NextValue());
            return $"{ts.Days}d {ts.Hours}h {ts.Minutes}m";
        }

        private string GetTotalMemory()
        {
            var totalMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes / (1024 * 1024);
            return $"{totalMemory} MB";
        }

        private int GetActiveConnections()
        {
            return 120;
        }
    }
}
