using API.Contracts.Services;
using API.Entities.Dedicated;
using API.Infra;
using API.Repositories;
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
        private readonly IUserRepository _userRepo;
        public CoreController(IMailService mailService,IUserRepository userRepository)
        {
            _mailService = mailService;
            _userRepo = userRepository;
        }

        [HttpGet("mailkaro")]
        [Authorize]
        public async Task<IActionResult> Falana()
        {

            await _mailService.SendEmailsAsync(["test@mail1"], "something", "test wala mail");


            return Ok("fine");
        }

        [HttpGet("test")]
        public async Task<IActionResult> Dhimkana()
        {
            Fren fren = new Fren();
            await _userRepo.AddUserAsync(fren);
            return Ok();
        }
       
    }
}
