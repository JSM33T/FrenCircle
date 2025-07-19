using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using FrenCircle.Data.Repositories;

namespace FrenCircle.Data.Services
{
    public interface IOAuthService
    {
        Task<OAuthUserInfo> GetGoogleUserInfoAsync(string accessToken);
        Task<User?> FindOrCreateOAuthUserAsync(OAuthUserInfo userInfo, string providerId);
        string GetGoogleAuthUrl(string redirectUri, string state);
    }

    public class OAuthUserInfo
    {
        public string ProviderId { get; set; } = string.Empty;
        public string ProviderUserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public string? Username { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpiresAt { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; } = new();
    }

    public class OAuthService : IOAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly AuthDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OAuthService(
            IAuthRepository authRepository, 
            AuthDbContext context, 
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _authRepository = authRepository;
            _context = context;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<OAuthUserInfo> GetGoogleUserInfoAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.googleapis.com/oauth2/v2/userinfo");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonContent = await response.Content.ReadAsStringAsync();
            var userInfo = JsonSerializer.Deserialize<GoogleUserInfo>(jsonContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (userInfo == null)
                throw new InvalidOperationException("Failed to deserialize Google user info");

            return new OAuthUserInfo
            {
                ProviderId = "google",
                ProviderUserId = userInfo.Id,
                Email = userInfo.Email,
                FirstName = userInfo.GivenName ?? string.Empty,
                LastName = userInfo.FamilyName ?? string.Empty,
                ProfilePicture = userInfo.Picture,
                Username = userInfo.Email.Split('@')[0], // Use email prefix as username
                AccessToken = accessToken,
                AdditionalData = new Dictionary<string, object>
                {
                    ["verified_email"] = userInfo.VerifiedEmail,
                    ["locale"] = userInfo.Locale ?? "en"
                }
            };
        }

        public async Task<User?> FindOrCreateOAuthUserAsync(OAuthUserInfo userInfo, string providerId)
        {
            // First, check if user exists by provider
            var existingUserProvider = await _context.UserProviders
                .Include(up => up.User)
                .FirstOrDefaultAsync(up => up.Provider.Name == userInfo.ProviderId && 
                                         up.ProviderUserId == userInfo.ProviderUserId);

            if (existingUserProvider != null)
            {
                // Update the existing provider data
                existingUserProvider.AccessToken = userInfo.AccessToken;
                existingUserProvider.RefreshToken = userInfo.RefreshToken;
                existingUserProvider.TokenExpiresAt = userInfo.TokenExpiresAt;
                existingUserProvider.LastSyncAt = DateTime.UtcNow;
                existingUserProvider.ProviderData = JsonSerializer.Serialize(userInfo.AdditionalData);

                await _context.SaveChangesAsync();
                return existingUserProvider.User;
            }

            // Check if user exists by email
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userInfo.Email);
            
            if (existingUser != null)
            {
                // Link this OAuth provider to the existing user
                await LinkProviderToUserAsync(existingUser.Id, userInfo, providerId);
                return existingUser;
            }

            // Create new user
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = userInfo.Email.ToLowerInvariant(),
                Username = await GenerateUniqueUsernameAsync(userInfo.Username ?? userInfo.Email.Split('@')[0]),
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                Avatar = userInfo.ProfilePicture,
                EmailVerified = true, // OAuth emails are considered verified
                EmailVerifiedAt = DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);

            // Assign default "User" role
            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (userRole != null)
            {
                _context.UserRoles.Add(new UserRole
                {
                    Id = Guid.NewGuid(),
                    UserId = newUser.Id,
                    RoleId = userRole.Id,
                    AssignedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            // Link OAuth provider
            await LinkProviderToUserAsync(newUser.Id, userInfo, providerId);

            return newUser;
        }

        public string GetGoogleAuthUrl(string redirectUri, string state)
        {
            var googleConfig = _configuration.GetSection("Authentication:Google");
            var clientId = googleConfig["ClientId"];
            
            var scopes = Uri.EscapeDataString("openid profile email");
            
            return $"https://accounts.google.com/o/oauth2/v2/auth?" +
                   $"client_id={clientId}&" +
                   $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
                   $"response_type=code&" +
                   $"scope={scopes}&" +
                   $"state={Uri.EscapeDataString(state)}&" +
                   $"access_type=offline&" +
                   $"prompt=consent";
        }

        private async Task LinkProviderToUserAsync(Guid userId, OAuthUserInfo userInfo, string providerId)
        {
            var provider = await _context.AuthProviders.FirstOrDefaultAsync(p => p.Name == userInfo.ProviderId);
            if (provider == null)
            {
                throw new InvalidOperationException($"OAuth provider '{userInfo.ProviderId}' not found in database");
            }

            var userProvider = new UserProvider
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProviderId = provider.Id,
                ProviderUserId = userInfo.ProviderUserId,
                ProviderUsername = userInfo.Username,
                ProviderEmail = userInfo.Email,
                AccessToken = userInfo.AccessToken,
                RefreshToken = userInfo.RefreshToken,
                TokenExpiresAt = userInfo.TokenExpiresAt,
                ProviderData = JsonSerializer.Serialize(userInfo.AdditionalData),
                ConnectedAt = DateTime.UtcNow,
                LastSyncAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.UserProviders.Add(userProvider);
            await _context.SaveChangesAsync();
        }

        private async Task<string> GenerateUniqueUsernameAsync(string baseUsername)
        {
            var username = baseUsername.ToLowerInvariant();
            var originalUsername = username;
            var counter = 1;

            while (await _context.Users.AnyAsync(u => u.Username == username))
            {
                username = $"{originalUsername}{counter}";
                counter++;
            }

            return username;
        }
    }

    // Helper class for Google user info deserialization
    public class GoogleUserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool VerifiedEmail { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public string? Picture { get; set; }
        public string? Locale { get; set; }
    }
}
