using API.Contracts.Services;
using API.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Base.Controllers
{
    [Route("")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        private readonly IMailService _mailService;
        public CoreController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpGet("mailkaro")]
        [Authorize]
        public async Task<IActionResult> Falana()
        {

            await _mailService.SendEmailsAsync(["test@mail1"], "something", "test wala mail");


            return Ok("fine");
        }


       
    }
}
