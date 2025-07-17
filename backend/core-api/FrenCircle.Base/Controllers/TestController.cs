
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [ApiController]
    [Route("/")]
    public class TestController(ILogger<TestController> logger) : Base.FcBaseController
    {
        private readonly ILogger<TestController> _logger = logger;

        [HttpGet(Name = "system-details")]
        public ActionResult<FrenCircle.Contracts.Shared.ApiResponse<object>> GetSystemDetails()
        {
            var details = new
            {
                ServerTime = DateTime.Now,
                OS = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                MachineName = Environment.MachineName,
                DotNetVersion = Environment.Version.ToString(),
                UserName = Environment.UserName
            };
            return RESP_Success((object)details, "System details fetched successfully");
        }
    }
}
