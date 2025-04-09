using FrenCircle.Contracts.Dtos;
using FrenCircle.Infra.Background;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Api.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController(IDispatcher dispatcher) : FcBaseController
    {
        public readonly IDispatcher _dispatcher = dispatcher;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<bool>>> StartSomething()
        {
            await _dispatcher.EnqueueAsync(async ct =>
            {
                await Task.Delay(2000, ct);
                Console.WriteLine("Background job executed.");
            }, jobName: "TestJob", triggeredBy: "TestController");

            return RESP_Success(true, "Job enqueued.");
        }
    }
}
