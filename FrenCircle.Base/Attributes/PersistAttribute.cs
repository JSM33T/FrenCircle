using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FrenCircle.Base.Attributes
{
    /// <summary>
    /// Caches API responses to JSON files for improved performance.
    /// Uses request-specific hash keys for POST requests (based on body content)
    /// and query string parameters for GET requests.
    /// Serializes all responses in camelCase format to maintain API consistency.
    /// 
    /// Usage: [Persist("unique-endpoint-name")] on controller actions
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PersistAttribute(string cacheKeyPrefix) : Attribute, IAsyncActionFilter
    {
        private readonly string _cachePath = "Cache";
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheKey = cacheKeyPrefix;

            // Handle POST requests
            if (context.HttpContext.Request.Method == "POST")
            {
                context.HttpContext.Request.EnableBuffering();
                var bodyJson = await GetRequestJsonAsync(context.HttpContext.Request);

                if (!string.IsNullOrEmpty(bodyJson))
                {
                    cacheKey += "_" + ComputeSha256Hash(bodyJson);
                }
            }
            else if (context.HttpContext.Request is { Method: "GET", QueryString.HasValue: true })
            {
                cacheKey += "_" + ComputeSha256Hash(context.HttpContext.Request.QueryString.Value);
            }

            var filePath = Path.Combine(_cachePath, $"{cacheKey}.json");

            Directory.CreateDirectory(_cachePath);

            if (File.Exists(filePath))
            {
                string cachedJson = await File.ReadAllTextAsync(filePath);
                context.Result = new ContentResult
                {
                    Content = cachedJson,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult { Value: not null } objectResult)
            {
                var jsonResponse = JsonSerializer.Serialize(objectResult.Value, _jsonOptions);
                await File.WriteAllTextAsync(filePath, jsonResponse);
            }
            else if (executedContext.Result is JsonResult { Value: not null } jsonResult)
            {
                var jsonResponse = JsonSerializer.Serialize(jsonResult.Value, _jsonOptions);
                await File.WriteAllTextAsync(filePath, jsonResponse);
            }
            else if (executedContext.Result is ContentResult contentResult &&
                    !string.IsNullOrEmpty(contentResult.Content) &&
                    contentResult.ContentType?.Contains("application/json") == true)
            {
                // Try to reserialize content to ensure camelCase
                try
                {
                    var obj = JsonSerializer.Deserialize<object>(contentResult.Content);
                    string formattedJson = JsonSerializer.Serialize(obj, _jsonOptions);
                    await File.WriteAllTextAsync(filePath, formattedJson);
                }
                catch
                {
                    await File.WriteAllTextAsync(filePath, contentResult.Content);
                }
            }
        }

        private async Task<string> GetRequestJsonAsync(HttpRequest request)
        {
            request.Body.Position = 0;

            using var streamReader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var bodyText = await streamReader.ReadToEndAsync();

            request.Body.Position = 0;

            return bodyText;
        }

        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}