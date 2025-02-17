using FrenCircle.Entities.Data;
using FrenCircle.Infra;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController(IPostRepository postRepository,IClaimsService claimsService) : FcBaseController
    {
        private readonly IPostRepository _postRepository = postRepository;
        private readonly IClaimsService _claimsService = claimsService;

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            return RESP_Success(await _postRepository.GetAllPosts(),"ASAS");
        }

        [HttpPost("addpost")]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            addPostRequest.AuthorId = _claimsService.GetUserId(User);
            await _postRepository.AddPost(addPostRequest);
            return RESP_Success("ASAS");
        }
    }
}
