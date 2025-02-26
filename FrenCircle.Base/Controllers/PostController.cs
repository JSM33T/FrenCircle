using FrenCircle.Entities.Data;
using FrenCircle.Infra;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController() : FcBaseController
    {
        //dummy methods
        
        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            return RESP_Success("ASAS");
        }

        [HttpPost("addpost")]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            return RESP_Success("ASAS");
        }
    }
}
