using FrenCircle.Contracts.Dtos;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Services;
using FrenCircle.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Api.Controllers
{
    [Route("api/profile")]
    [Authorize]
    [ApiController]
    public class ProfileController(IProfileService profileService) : FcBaseController
    {
        [HttpGet("get")]
        public async Task<ActionResult<ApiResponse<UserProfileDetailsDto>>> GetProfile()
        {
            var userId = HttpContextHelper.GetUserId(HttpContext!);

            var ss = await profileService.GetUserProfileById(userId);

            return RESP_Success(ss);
        }
    }
}
