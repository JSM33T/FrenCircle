using FrenCircle.Helpers;
using FrenCircle.Infra;

namespace FrenCircle.Base.Middlewares
{
    public class FcRequestMiddleware(RequestDelegate next,ILogger<FcRequestMiddleware> logger,ITelegramService telegramService)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<FcRequestMiddleware> _logger = logger;
        private readonly ITelegramService _telegramService = telegramService;
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
                _logger.LogError(ex,ex.Message);
                _ = _telegramService.SendMessageAsync($"Error || {DateTime.Now}: {ex}");
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
