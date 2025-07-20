
using FrenCircle.Contracts.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [ApiController]
    [Route("/test")]
    public class AuthController(ILogger<AuthController> logger) : Base.FcBaseController
    {
        private readonly ILogger<AuthController> _logger = logger;

        [HttpGet(Name = "system-details")]
        public ActionResult<ApiResponse<object>> GetSystemDetails()
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
