using API.Entities.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace API.Base.Middlewares
{
    public class AcValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            responseBody.Seek(0, SeekOrigin.Begin);

            if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
            {
                var originalBodyText = await new StreamReader(responseBody).ReadToEndAsync();

                if (!string.IsNullOrEmpty(originalBodyText) && originalBodyText.Contains("\"traceId\""))
                {
                    var originalResponse = JsonConvert.DeserializeObject<ValidationProblemDetails>(originalBodyText);
                    var errorMessages = new List<string>();

                    foreach (var error in originalResponse!.Errors)
                    {
                        if (error.Value != null && error.Value.Length > 0)
                        {
                            errorMessages.Add(error.Value[0]);
                        }
                    }

                    var customResponse = new
                    {
                        status = StatusCodes.Status400BadRequest,
                        message = "Validation Error",
                        hints = errorMessages,
                        data = 0
                    };

                    var customResponseText = JsonConvert.SerializeObject(customResponse, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                    context.Response.Body = originalBodyStream;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(customResponseText);
                }
                else
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            else if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests || context.Response.StatusCode == StatusCodes.Status503ServiceUnavailable)
            {
                var errorMessages = new List<string>
                {
                    "Too many requests. Please try again later."
                };

                var customResponse = new APIResponse<int>(
                    status: StatusCodes.Status429TooManyRequests,
                    message: "Rate limit exceeded",
                    data: 0,
                    hints: errorMessages
                );

                var customResponseText = JsonConvert.SerializeObject(customResponse, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(customResponseText);
            }
            else if (context.Response.StatusCode == StatusCodes.Status415UnsupportedMediaType)
            {
                var errorMessages = new List<string>
                {
                    "Invalid/Deformed request"
                };

                var customResponse = new APIResponse<int>(
                    status: StatusCodes.Status415UnsupportedMediaType,
                    message: "Invalid Request",
                    data: 0,
                    hints: errorMessages
                );

                var customResponseText = JsonConvert.SerializeObject(customResponse, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(customResponseText);
            }
            else if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                var errorMessages = new List<string>
                {
                    "You are not authorized for this action"
                };

                var customResponse = new APIResponse<int>(
                    status: StatusCodes.Status401Unauthorized,
                    message: "Unauthorized request",
                    data: 0,
                    hints: errorMessages
                );

                var customResponseText = JsonConvert.SerializeObject(customResponse, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                context.Response.Body = originalBodyStream;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(customResponseText);
            }
            else
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
