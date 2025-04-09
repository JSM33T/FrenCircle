using FrenCircle.Infra.Background;
using FrenCircle.Infra.Telegram;
using FrenCircle.Shared.Helpers;

namespace FrenCircle.Api.Middlewares
{

        public class FcRequestMiddleware(RequestDelegate next, ILogger<FcRequestMiddleware> logger, IDispatcher backgroundQueue)
        {
            public async Task InvokeAsync(HttpContext context)
            {
                var originalBodyStream = context.Response.Body;

                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                try
                {
                    await next(context);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError(ex, message: ex.Message);

                    await backgroundQueue.EnqueueAsync(async token =>
                    {
                       // await telegramService.SendMessageAsync($"Error || {DateTime.UtcNow}: {ex}");
                    }, jobName: "UnhandledException", triggeredBy: "InterceptorMiddleware");

                    await ResponseHandlers.HandleInternalServerError(context, originalBodyStream, ex);
                    return;
                }


                responseBody.Seek(0, SeekOrigin.Begin);

                var statusCode = context.Response.StatusCode;

                switch (statusCode)
                {
                    case StatusCodes.Status400BadRequest:
                        await ResponseHandlers.HandleBadRequestAsync(context, responseBody, originalBodyStream);
                        break;

                    case StatusCodes.Status429TooManyRequests:
                    case StatusCodes.Status503ServiceUnavailable:
                        await ResponseHandlers.HandleRateLimitExceededAsync(context, originalBodyStream);
                        break;

                    case StatusCodes.Status415UnsupportedMediaType:
                        await ResponseHandlers.HandleInvalidRequestAsync(context, originalBodyStream);
                        break;

                    case StatusCodes.Status401Unauthorized:
                    case StatusCodes.Status403Forbidden:
                        await ResponseHandlers.HandleUnauthorizedAsync(context, originalBodyStream);
                        break;

                    default:
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                        break;
                }
            }
        }
}
