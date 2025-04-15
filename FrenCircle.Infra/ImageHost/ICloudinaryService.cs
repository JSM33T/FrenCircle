using Microsoft.AspNetCore.Http;

namespace FrenCircle.Infra.ImageHost
{
    public interface ICloudinaryService
    {
        Task<string?> UploadProfilePictureAsync(IFormFile file, Guid userId);
    }
}