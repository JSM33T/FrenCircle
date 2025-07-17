using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrenCircle.Base.Middlewares
{
    public class RequestTimerMiddleware(RequestDelegate next, FcConfig config)
    {
        private readonly Toggles _config = config.Toggles ?? new Toggles();

        public async Task Invoke(HttpContext context)
        {
            if (!_config.IncludeResponseTime)
            {
                await next(context);
                return;
            }

            // Swap response stream
            var originalBody = context.Response.Body;
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            var sw = Stopwatch.StartNew();
            await next(context);
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

                        var modified = JsonSerializer.Serialize(json, new JsonSerializerOptions
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
