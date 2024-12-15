using API.Contracts.Repositories;
using API.Contracts.Services;
using API.Entities.Dedicated;
using API.Entities.Dedicated.Posts;
using API.Entities.Shared;
using API.Infra;
using API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Base.Controllers
{
    [Route("")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IUserRepository _userRepo;
        private readonly IPostRepository _postRepo;
        public CoreController(IMailService mailService,IUserRepository userRepository,IPostRepository postRepository)
        {
            _mailService = mailService;
            _userRepo = userRepository;
            _postRepo = postRepository;
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
           // await _userRepo.AddUserAsync(fren);
           
            List<GetAllPosts> posts =  await _postRepo.GetPosts();

            APIResponse<List<GetAllPosts>> postss = new(200, "", posts, []);

            return Ok(postss);
        }
       
    }
}
