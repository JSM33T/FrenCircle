using FrenCircle.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Api.Controllers
{
    [ApiController]
    public abstract class FcBaseController : ControllerBase
    {
        protected ActionResult<ApiResponse<T>> FcResponse<T>(ApiResponse<T?> apiResponse)
        {
            if (HttpContext?.Items?.TryGetValue("__elapsedMs_final", out var msObj) == true && msObj is long ms)
            {
                apiResponse.ResponseTimeMs = ms;
            }

            return StatusCode(apiResponse.Status, apiResponse);
        }

        protected ActionResult<ApiResponse<T>> RESP_Custom<T>(ApiResponse<T?> apiResponse) =>
            FcResponse(apiResponse);

        protected ActionResult<ApiResponse<T>> RESP_Success<T>(T data, string message = "Success") =>
            FcResponse(new ApiResponse<T?>(200, message, data));

        // ✅ Generic error overloads
        protected ActionResult<ApiResponse<T>> RESP_BadRequestResponse<T>(string message) =>
            FcResponse(new ApiResponse<T?>(400, message, default));

        protected ActionResult<ApiResponse<T>> RESP_UnauthorizedResponse<T>(string message = "Unauthorized") =>
            FcResponse(new ApiResponse<T?>(401, message, default));

        protected ActionResult<ApiResponse<T>> RESP_ForbiddenResponse<T>(string message = "Forbidden") =>
            FcResponse(new ApiResponse<T?>(403, message, default));

        protected ActionResult<ApiResponse<T>> RESP_NotFoundResponse<T>(string message = "Not Found") =>
            FcResponse(new ApiResponse<T?>(404, message, default));

        protected ActionResult<ApiResponse<T>> RESP_ConflictResponse<T>(string message = "Conflict") =>
            FcResponse(new ApiResponse<T?>(409, message, default));

        protected ActionResult<ApiResponse<T>> RESP_ServerErrorResponse<T>(string message = "Internal Server Error") =>
            FcResponse(new ApiResponse<T?>(500, message, default));
    }
}
