using FrenCircle.Shared.ConfigModels;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace FrenCircle.Api.Middlewares
{
    public class RequestTimerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Toggles _config;

        public RequestTimerMiddleware(RequestDelegate next, IOptions<FcConfig> config)
        {
            _next = next;
            _config = config.Value.Toggles;
        }

        public async Task Invoke(HttpContext context)
        {
            // Skip timing logic if config says so
            if (!_config.IncludeResponseTime)
            {
                await _next(context);
                return;
            }

            // Swap response stream
            var originalBody = context.Response.Body;
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            var sw = Stopwatch.StartNew();
            await _next(context);
            sw.Stop();

            memStream.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(memStream).ReadToEndAsync();

            // Only attempt modification if it's JSON
            if (context.Response.ContentType?.Contains("application/json") == true &&
                !string.IsNullOrWhiteSpace(responseText))
            {
                try
                {
                    var json = JsonSerializer.Deserialize<Dictionary<string, object>>(responseText, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (json != null)
                    {
                        json["responseTimeMs"] = sw.ElapsedMilliseconds;

                        string modified = JsonSerializer.Serialize(json, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                        });

                        context.Response.Body = originalBody;
                        context.Response.ContentLength = Encoding.UTF8.GetByteCount(modified);
                        await context.Response.WriteAsync(modified);
                        return;
                    }
                }
                catch
                {
                    // On error, fall back to original response
                }
            }

            memStream.Seek(0, SeekOrigin.Begin);
            await memStream.CopyToAsync(originalBody);
        }
    }

}
