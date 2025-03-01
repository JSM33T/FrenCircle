#region File Cache
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.Json;

//namespace FrenCircle.Base.Attributes
//{
//    /// <summary>
//    /// Caches API responses to JSON files for improved performance.
//    /// Uses request-specific hash keys for POST requests (based on body content)
//    /// and query string parameters for GET requests.
//    /// BUG : Doesnt work for POSTs + FormData
//    /// 
//    /// Usage: [Persist("unique-endpoint-name")] on controller actions
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Method)]
//    public class PersistAttribute(string cacheKeyPrefix) : Attribute, IAsyncActionFilter
//    {
//        private readonly string _cachePath = "Cache";
//        private readonly JsonSerializerOptions _jsonOptions = new()
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//            WriteIndented = true
//        };

//        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//        {
//            var cacheKey = cacheKeyPrefix;

//            // Handle POST requests
//            if (context.HttpContext.Request.Method == "POST")
//            {
//                context.HttpContext.Request.EnableBuffering();
//                var bodyJson = await GetRequestJsonAsync(context.HttpContext.Request);

//                if (!string.IsNullOrEmpty(bodyJson))
//                {
//                    cacheKey += "_" + ComputeSha256Hash(bodyJson);
//                }
//            }
//            else if (context.HttpContext.Request is { Method: "GET", QueryString.HasValue: true })
//            {
//                cacheKey += "_" + ComputeSha256Hash(context.HttpContext.Request.QueryString.Value);
//            }

//            var filePath = Path.Combine(_cachePath, $"{cacheKey}.json");

//            Directory.CreateDirectory(_cachePath);

//            if (File.Exists(filePath))
//            {
//                string cachedJson = await File.ReadAllTextAsync(filePath);
//                context.Result = new ContentResult
//                {
//                    Content = cachedJson,
//                    ContentType = "application/json",
//                    StatusCode = 200
//                };
//                return;
//            }

//            var executedContext = await next();

//            if (executedContext.Result is ObjectResult { Value: not null } objectResult)
//            {
//                var jsonResponse = JsonSerializer.Serialize(objectResult.Value, _jsonOptions);
//                await File.WriteAllTextAsync(filePath, jsonResponse);
//            }   
//            else if (executedContext.Result is JsonResult { Value: not null } jsonResult)
//            {
//                var jsonResponse = JsonSerializer.Serialize(jsonResult.Value, _jsonOptions);
//                await File.WriteAllTextAsync(filePath, jsonResponse);
//            }
//            else if (executedContext.Result is ContentResult contentResult &&
//                    !string.IsNullOrEmpty(contentResult.Content) &&
//                    contentResult.ContentType?.Contains("application/json") == true)
//            {
//                // Try to reserialize content to ensure camelCase
//                try
//                {
//                    var obj = JsonSerializer.Deserialize<object>(contentResult.Content);
//                    string formattedJson = JsonSerializer.Serialize(obj, _jsonOptions);
//                    await File.WriteAllTextAsync(filePath, formattedJson);
//                }
//                catch
//                {
//                    await File.WriteAllTextAsync(filePath, contentResult.Content);
//                }
//            }
//        }

//        private static async Task<string> GetRequestJsonAsync(HttpRequest request)
//        {
//            request.Body.Position = 0;

//            using var streamReader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
//            var bodyText = await streamReader.ReadToEndAsync();

//            request.Body.Position = 0;

//            return bodyText;
//        }

//        private static string ComputeSha256Hash(string rawData)
//        {
//            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));
//            return Convert.ToHexStringLower(bytes);
//        }
//    }
//}
#endregion

#region Dist Sql server cache

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Caching.Distributed;
//using System.Security.Cryptography;
//using System.Text;
//using System.Text.Json;

//namespace FrenCircle.Base.Attributes
//{
//    [AttributeUsage(AttributeTargets.Method)]
//    public class PersistAttribute(string cacheKeyPrefix) : Attribute, IAsyncActionFilter
//    {
//        private readonly JsonSerializerOptions _jsonOptions = new()
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//            WriteIndented = true
//        };

//        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//        {
//            var cache = context.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
//            var cacheKey = cacheKeyPrefix;

//            // Handle POST requests
//            if (context.HttpContext.Request.Method == "POST")
//            {
//                context.HttpContext.Request.EnableBuffering();
//                var bodyJson = await GetRequestJsonAsync(context.HttpContext.Request);
//                if (!string.IsNullOrEmpty(bodyJson))
//                {
//                    cacheKey += "_" + ComputeSha256Hash(bodyJson);
//                }
//            }
//            else if (context.HttpContext.Request is { Method: "GET", QueryString.HasValue: true })
//            {
//                cacheKey += "_" + ComputeSha256Hash(context.HttpContext.Request.QueryString.Value);
//            }

//            // Try to get cached response from DistributedCache
//            var cachedJson = await cache.GetStringAsync(cacheKey);
//            if (!string.IsNullOrEmpty(cachedJson))
//            {
//                context.Result = new ContentResult
//                {
//                    Content = cachedJson,
//                    ContentType = "application/json",
//                    StatusCode = 200
//                };
//                return;
//            }

//            // Execute the action and cache the result
//            var executedContext = await next();

//            if (executedContext.Result is ObjectResult { Value: not null } objectResult)
//            {
//                var jsonResponse = JsonSerializer.Serialize(objectResult.Value, _jsonOptions);
//                await cache.SetStringAsync(cacheKey, jsonResponse, new DistributedCacheEntryOptions
//                {
//                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7) // Cache for 7 days
//                });
//            }
//            else if (executedContext.Result is JsonResult { Value: not null } jsonResult)
//            {
//                var jsonResponse = JsonSerializer.Serialize(jsonResult.Value, _jsonOptions);
//                await cache.SetStringAsync(cacheKey, jsonResponse, new DistributedCacheEntryOptions
//                {
//                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
//                });
//            }
//        }

//        private static async Task<string> GetRequestJsonAsync(HttpRequest request)
//        {
//            request.Body.Position = 0;
//            using var streamReader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
//            var bodyText = await streamReader.ReadToEndAsync();
//            request.Body.Position = 0;
//            return bodyText;
//        }

//        private static string ComputeSha256Hash(string rawData)
//        {
//            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));
//            return Convert.ToHexStringLower(bytes);
//        }
//    }
//}

#endregion

#region MongoDb

using FrenCircle.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FrenCircle.Base.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PersistAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _cacheKeyPrefix;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public PersistAttribute(string cacheKeyPrefix)
        {
            _cacheKeyPrefix = cacheKeyPrefix;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IOptions<FcConfig>>().Value;
            var client = new MongoClient(config.mongoDb.ConnectionString);
            var database = client.GetDatabase(config.mongoDb.Database);
            var collection = database.GetCollection<BsonDocument>(config.mongoDb.Collection);

            var cacheKey = _cacheKeyPrefix;

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

            var filter = Builders<BsonDocument>.Filter.Eq("_id", cacheKey);
            var cachedDoc = await collection.Find(filter).FirstOrDefaultAsync();

            if (cachedDoc != null)
            {
                context.Result = new ContentResult
                {
                    Content = cachedDoc["value"].AsString,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult { Value: not null } objectResult)
            {
                var jsonResponse = JsonSerializer.Serialize(objectResult.Value, _jsonOptions);
                await CacheResponseAsync(collection, cacheKey, jsonResponse);
            }
            else if (executedContext.Result is JsonResult { Value: not null } jsonResult)
            {
                var jsonResponse = JsonSerializer.Serialize(jsonResult.Value, _jsonOptions);
                await CacheResponseAsync(collection, cacheKey, jsonResponse);
            }
            else if (executedContext.Result is ContentResult contentResult &&
                    !string.IsNullOrEmpty(contentResult.Content) &&
                    contentResult.ContentType?.Contains("application/json") == true)
            {
                await CacheResponseAsync(collection, cacheKey, contentResult.Content);
            }
        }

        private async Task CacheResponseAsync(IMongoCollection<BsonDocument> collection, string cacheKey, string jsonResponse)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", cacheKey);
            var update = Builders<BsonDocument>.Update
                .Set("value", jsonResponse)
                .Set("expiresAt", DateTime.UtcNow.AddDays(7));

            await collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        private static async Task<string> GetRequestJsonAsync(HttpRequest request)
        {
            request.Body.Position = 0;
            using var streamReader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var bodyText = await streamReader.ReadToEndAsync();
            request.Body.Position = 0;
            return bodyText;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexStringLower(bytes);
        }
    }

}

#endregion