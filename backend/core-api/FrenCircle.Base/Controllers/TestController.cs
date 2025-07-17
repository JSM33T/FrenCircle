using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            return Ok(

                new
                {
                    Date = DateTime.Now,
                    ServerStat = "Running"
                }
            );
        }
    }
}
