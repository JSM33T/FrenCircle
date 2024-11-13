using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.API.Controllers.Base
{
    [ApiController]
    [Route("/api/tick")]
    public class TickController(ILogger<TickController> logger) : ControllerBase
    {
        private readonly ILogger<TickController> _logger = logger;

        [HttpGet("serverstat")]
        public IActionResult GetServerStat() => Ok($"server stat on {DateTime.Now} : Healthy");
    }
}
