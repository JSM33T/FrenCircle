using FrenCircle.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;

namespace FrenCircle.Helpers
{
    public static class ResponseHandler
    {
        public static async Task HandleBadRequestAsync(HttpContext context, MemoryStream responseBody, Stream originalBodyStream)
        {
            var originalBodyText = await new StreamReader(responseBody).ReadToEndAsync();

            if (!string.IsNullOrEmpty(originalBodyText) && originalBodyText.Contains("\"traceId\""))
            {
                var originalResponse = JsonConvert.DeserializeObject<ValidationProblemDetails>(originalBodyText);
                List<string> errorMessages = originalResponse!.Errors
                    .Where(error => error.Value != null && error.Value.Length > 0)
                    .Select(error => error.Value[0])
                    .ToList();

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

        public static async Task HandleRateLimitExceededAsync(HttpContext context, Stream originalBodyStream)
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

        public static async Task HandleInvalidRequestAsync(HttpContext context, Stream originalBodyStream)
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

        public static async Task HandleUnauthorizedAsync(HttpContext context, Stream originalBodyStream)
        {
            var errorMessages = new List<string>
            {
                "You are not authorized for this action",
                "Repetitive suspicious actions from an account might lead to temporary ban of your account"
            };

            var customResponse = new APIResponse<int>(
                status: StatusCodes.Status401Unauthorized,
                message: "Unauthorized request / Forbidden",
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

        public static async Task HandleInternalServerError(HttpContext context, Stream originalBodyStream, Exception exception)
        {
            var errorMessages = new List<string>
            {
                "TEMPORARY CRITICAL DEV EXCEPTION",
                exception.Message.ToString()
            };

            var customResponse = new APIResponse<int>(
                status: StatusCodes.Status500InternalServerError,
                message: "Internal server error",
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
    }
}
