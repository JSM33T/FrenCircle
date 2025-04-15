using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FrenCircle.Shared.ConfigModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FrenCircle.Infra.ImageHost
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(FcConfig config)
        {
            var acc = new Account(
                config.CloudinaryConfig?.CloudName,
                config.CloudinaryConfig?.ApiKey,
                config.CloudinaryConfig?.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task<string?> UploadProfilePictureAsync(IFormFile file, Guid userGuid)
        {
            if (file == null || file.Length == 0) return null;

            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "pfp",
                PublicId = $"user_{userGuid}", // Use GUID here
                Overwrite = true
            };

            var result = await _cloudinary.UploadAsync(uploadParams);
            return result.SecureUrl?.ToString();
        }
    }

}
