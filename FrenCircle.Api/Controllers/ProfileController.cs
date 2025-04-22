using FrenCircle.Contracts.Dtos;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Services;
using FrenCircle.Infra.ImageHost;
using FrenCircle.Shared.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Api.Controllers
{
    [Route("api/profile")]
    [Authorize] 
    [ApiController]
    public class ProfileController(IProfileService profileService,ICloudinaryService cloudinaryService) : FcBaseController
    {
        [HttpGet("get")]
        public async Task<ActionResult<ApiResponse<UserProfileDetailsDto>>> GetProfile()
        {
            var userId = HttpContextHelper.GetUserId(HttpContext!);

            var ss = await profileService.GetUserProfileById(userId);

            return RESP_Success(ss);
        }

        [HttpPost("update")]
        public async Task<ActionResult<ApiResponse<UserProfileDetailsDto>>> EditProfile([FromForm] EditUserProfileDto dto)
        {
            dto.Id = HttpContextHelper.GetUserId(HttpContext!);
            var userGuid = await profileService.GetUserProfileById(dto.Id);

            string? avatarUrl = null;
            if (dto.Avatar != null)
                avatarUrl = await cloudinaryService.UploadProfilePictureAsync(dto.Avatar, userGuid.UserId);
           
            var result = await profileService.UpdateUserProfile(dto, avatarUrl);

            if (result == 1)
                return RESP_ConflictResponse<UserProfileDetailsDto>("Username already exists");

            var updated = await profileService.GetUserProfileById(dto.Id);
            return RESP_Success(updated);
        }

    }
}
