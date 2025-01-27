using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [Route("/")]
    [ApiController]
    public class AuthController : FcBaseController
    {
        [HttpGet]
        public Task<IActionResult> ServerStat()
        {
            return Task.FromResult<IActionResult>(RESP_Success("something"));
        }
    }
}
