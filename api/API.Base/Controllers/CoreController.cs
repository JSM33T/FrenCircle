using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Base.Controllers
{
    [Route("s")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Falana()
        {
            return Ok("fine");
        }


       
    }
}
