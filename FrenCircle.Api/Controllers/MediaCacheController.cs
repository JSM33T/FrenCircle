using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Api.Controllers
{
    [Route("media/{*cloudinaryPath}")]
    [ApiController]
    public class MediaCacheController(IWebHostEnvironment env, IHttpClientFactory factory) : ControllerBase
    {
        private readonly HttpClient _http = factory.CreateClient();

        //[HttpGet]
        //public async Task<IActionResult> GetCachedMedia(string cloudinaryPath)
        //{
        //    var localPath = Path.Combine(env.WebRootPath, "mediacache", cloudinaryPath.Replace('/', Path.DirectorySeparatorChar));
        //    var localUrl = $"/mediacache/{cloudinaryPath}";

        //    if (System.IO.File.Exists(localPath))
        //    {
        //        return PhysicalFile(localPath, GetMimeType(localPath));
        //    }

        //    var remoteUrl = $"https://res.cloudinary.com/{cloudinaryPath}";
        //    var dir = Path.GetDirectoryName(localPath);
        //    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);

        //    var stream = await _http.GetStreamAsync(remoteUrl);
        //    await using var fs = new FileStream(localPath, FileMode.Create);
        //    await stream.CopyToAsync(fs);

        //    return PhysicalFile(localPath, GetMimeType(localPath));
        //}

        [HttpGet]
        public async Task<IActionResult> GetCachedMedia(string cloudinaryPath)
        {
            if (string.IsNullOrWhiteSpace(cloudinaryPath) || cloudinaryPath.Contains(".."))
                return BadRequest("Invalid path");

            var safePath = cloudinaryPath.Replace('\\', '/').TrimStart('/');
            var localPath = Path.Combine(env.WebRootPath, "mediacache", safePath.Replace('/', Path.DirectorySeparatorChar));
            var localUrl = $"/mediacache/{safePath}";

            if (System.IO.File.Exists(localPath))
            {
                return PhysicalFile(localPath, GetMimeType(localPath));
            }

            var remoteUrl = $"https://res.cloudinary.com/{safePath}";
            var dir = Path.GetDirectoryName(localPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);

            var stream = await _http.GetStreamAsync(remoteUrl);
            await using var fs = new FileStream(localPath, FileMode.Create);
            await stream.CopyToAsync(fs);

            return PhysicalFile(localPath, GetMimeType(localPath));
        }


        private string GetMimeType(string path)
        {
            var ext = Path.GetExtension(path).ToLower();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }
}
