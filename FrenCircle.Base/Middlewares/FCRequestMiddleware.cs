using FrenCircle.Helpers;

namespace FrenCircle.Base.Middlewares
{
    public class FcRequestMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await ResponseHandler.HandleInternalServerError(context, originalBodyStream, ex);
                return;
            }

            responseBody.Seek(0, SeekOrigin.Begin);

            var statusCode = context.Response.StatusCode;

            switch (statusCode)
            {
                case StatusCodes.Status400BadRequest:
                    await ResponseHandler.HandleBadRequestAsync(context, responseBody, originalBodyStream);
                    break;

                case StatusCodes.Status429TooManyRequests:
                case StatusCodes.Status503ServiceUnavailable:
                    await ResponseHandler.HandleRateLimitExceededAsync(context, originalBodyStream);
                    break;

                case StatusCodes.Status415UnsupportedMediaType:
                    await ResponseHandler.HandleInvalidRequestAsync(context, originalBodyStream);
                    break;

                case StatusCodes.Status401Unauthorized:
                case StatusCodes.Status403Forbidden:
                    await ResponseHandler.HandleUnauthorizedAsync(context, originalBodyStream);
                    break;

                default:
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                    break;
            }
        }
    }
}
