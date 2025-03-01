using FrenCircle.Entities.Data;
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
            return RESP_Success("them");
        }

        [HttpPost("addpost")]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            return RESP_Success("ASAS");
        }
    }
}
