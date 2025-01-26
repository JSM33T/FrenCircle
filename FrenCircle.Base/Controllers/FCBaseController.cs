using FrenCircle.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [ApiController]
    public abstract class FcBaseController : ControllerBase
    {
        private IActionResult FcResponse<T>(APIResponse<T?> apiResponse)
        {
            return StatusCode(apiResponse.Status, apiResponse);
        }

        protected IActionResult RESP_Success<T>(T data, string message = "Success")
        {
            return FcResponse(new APIResponse<T?>(200, message, data, []));
        }

        protected IActionResult RESP_BadRequestResponse(string message)
        {
            return FcResponse(new APIResponse<object?>(400, message, null, []));
        }

        protected IActionResult RESP_UnauthorizedResponse(string message = "Unauthorized")
        {
            return FcResponse(new APIResponse<object?>(401, message, null, []));
        }

        protected IActionResult RESP_ForbiddenResponse(string message = "Forbidden")
        {
            return FcResponse(new APIResponse<object?>(403, message, null, []));
        }

        protected IActionResult RESP_NotFoundResponse(string message = "Not Found")
        {
            return FcResponse(new APIResponse<object?>(404, message, null, []));
        }

        protected IActionResult RESP_ConflictResponse(string message = "Conflict")
        {
            return FcResponse(new APIResponse<object?>(409, message, null, []));
        }

        protected IActionResult RESP_ServerErrorResponse(string message = "Internal Server Error")
        {
            return FcResponse(new APIResponse<object?>(500, message, null, []));
        }
    }
}
