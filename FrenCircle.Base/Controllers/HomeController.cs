using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [Route("/api/common")]
    [ApiController]
    public class HomeController : FcBaseController
    {
        [HttpGet]
        public Task<IActionResult> ServerStat()
        {
            return Task.FromResult<IActionResult>(RESP_Success("||", "Server is up!!"));
        }
    }
}