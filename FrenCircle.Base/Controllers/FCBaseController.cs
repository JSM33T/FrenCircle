using FrenCircle.Entities;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable HeapView.ObjectAllocation.Evident

namespace FrenCircle.Base.Controllers
{
    [ApiController]
    public abstract class FcBaseController : ControllerBase
    {
        private ObjectResult FcResponse<T>(APIResponse<T?> apiResponse) =>
            StatusCode(apiResponse.Status, apiResponse);

        protected ObjectResult RESP_Custom<T>(APIResponse<T> apiResponse) =>
            FcResponse(apiResponse!);

        protected ObjectResult RESP_Success<T>(T data, string message = "Success") =>
            FcResponse(new APIResponse<T?>(200, message, data, []));

        protected ObjectResult RESP_BadRequestResponse(string message) =>
            FcResponse(new APIResponse<object?>(400, message, null, []));

        protected ObjectResult RESP_UnauthorizedResponse(string message = "Unauthorized") =>
            FcResponse(new APIResponse<object?>(401, message, null, []));

        protected ObjectResult RESP_ForbiddenResponse(string message = "Forbidden") =>
            FcResponse(new APIResponse<object?>(403, message, null, []));

        protected ObjectResult RESP_NotFoundResponse(string message = "Not Found") =>
            FcResponse(new APIResponse<object?>(404, message, null, []));

        protected ObjectResult RESP_ConflictResponse(string message = "Conflict") =>
            FcResponse(new APIResponse<object?>(409, message, null, []));

        protected ObjectResult RESP_ServerErrorResponse(string message = "Internal Server Error") =>
            FcResponse(new APIResponse<object?>(500, message, null, []));
    }
}