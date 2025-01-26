using FrenCircle.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [ApiController]
    public abstract class FCBaseController : ControllerBase
    {
        protected IActionResult FCResponse<T>(APIResponse<T> apiResponse)
        {
            return StatusCode(apiResponse.Status, apiResponse);
        }

        protected IActionResult RESP_Success<T>(T data, string message = "Success")
        {
            return FCResponse(new APIResponse<T>(200, message, data, []));
        }

        protected IActionResult RESP_BadRequestResponse(string message)
        {
            return FCResponse(new APIResponse<object>(400, message, null, []));
        }

        protected IActionResult RESP_UnauthorizedResponse(string message = "Unauthorized")
        {
            return FCResponse(new APIResponse<object>(401, message, null, []));
        }

        protected IActionResult RESP_ForbiddenResponse(string message = "Forbidden")
        {
            return FCResponse(new APIResponse<object>(403, message, null, []));
        }

        protected IActionResult RESP_NotFoundResponse(string message = "Not Found")
        {
            return FCResponse(new APIResponse<object>(404, message, null, []));
        }

        protected IActionResult RESP_ConflictResponse(string message = "Conflict")
        {
            return FCResponse(new APIResponse<object>(409, message, null, []));
        }

        protected IActionResult RESP_ServerErrorResponse(string message = "Internal Server Error")
        {
            return FCResponse(new APIResponse<object>(500, message, null, []));
        }
    }
}
