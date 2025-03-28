using FrenCircle.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Api.Controllers
{
    [ApiController]
    public abstract class FcBaseController : ControllerBase
    {
        private ObjectResult FcResponse<T>(ApiResponse<T?> ApiResponse) =>
            StatusCode(ApiResponse.Status, ApiResponse);

        protected ObjectResult RESP_Custom<T>(ApiResponse<T> ApiResponse) =>
            FcResponse(ApiResponse);

        protected ObjectResult RESP_Success<T>(T data, string message = "Success") =>
            FcResponse(new ApiResponse<T?>(200, message, data));

        protected ObjectResult RESP_BadRequestResponse(string message) =>
            FcResponse(new ApiResponse<object?>(400, message, null));

        protected ObjectResult RESP_UnauthorizedResponse(string message = "Unauthorized") =>
            FcResponse(new ApiResponse<object?>(401, message, null));

        protected ObjectResult RESP_ForbiddenResponse(string message = "Forbidden") =>
            FcResponse(new ApiResponse<object?>(403, message, null));

        protected ObjectResult RESP_NotFoundResponse(string message = "Not Found") =>
            FcResponse(new ApiResponse<object?>(404, message, null));

        protected ObjectResult RESP_ConflictResponse(string message = "Conflict") =>
            FcResponse(new ApiResponse<object?>(409, message, null));

        protected ObjectResult RESP_ServerErrorResponse(string message = "Internal Server Error") =>
            FcResponse(new ApiResponse<object?>(500, message, null));
    }
}
