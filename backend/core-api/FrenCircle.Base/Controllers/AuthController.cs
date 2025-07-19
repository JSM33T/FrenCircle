using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FrenCircle.Contracts.Auth;
using FrenCircle.Data.Repositories;
using FrenCircle.Data.Services;
using FrenCircle.Data;
using System.Security.Claims;
using System.Security;
using System.Text.Json;

namespace FrenCircle.Api.Controllers
{
    public class LogoutRequest
    {
        public Guid SessionId { get; set; }
    }
    public class RevokeAllSessionsRequest
    {
        public Guid? ExceptSessionId { get; set; }
    }
    public class OAuthCallbackRequest
    {
        public string Provider { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? RedirectUri { get; set; }
        public string? State { get; set; }
    }
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
    public class TwoFactorEnableRequest
    {
        public string Secret { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
    public class VerifyEmailRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
    public class EmailRequest
    {
        public string Email { get; set; } = string.Empty;
    }
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IJwtService _jwtService;
        private readonly IOAuthService _oauthService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthRepository repo, IJwtService jwtService, IOAuthService oauthService, ILogger<AuthController> logger)
        {
            _repo = repo;
            _jwtService = jwtService;
            _oauthService = oauthService;
            _logger = logger;
        }

        // 1. Register (email+password)
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = Request.Headers["User-Agent"].ToString();
            var result = await _repo.CreateUserAsync(req, ip, userAgent);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // 2. Login (email+password)
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var loginResult = await _repo.AuthenticateUserAsync(req.Email, req.Password);

            if (!loginResult.Success)
                return Unauthorized(loginResult);

            // Create session and refresh token
            var userAgent = Request.Headers["User-Agent"].ToString();
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var session = await _repo.CreateSessionAsync(loginResult.UserId!.Value, req.DeviceFingerprint ?? "", ip, userAgent);
            var refreshToken = await _repo.CreateRefreshTokenAsync(loginResult.UserId!.Value, session.Id, ip);

            // Get user roles and permissions for JWT
            var roles = await _repo.GetUserRolesAsync(loginResult.UserId.Value);
            var permissions = await _repo.GetUserPermissionsAsync(loginResult.UserId.Value);
            var user = await _repo.GetUserByIdAsync(loginResult.UserId.Value);

            // Generate JWT access token
            var accessToken = _jwtService.GenerateAccessToken(user!, roles, permissions);

            // Set refresh token in httpOnly cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use only in HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(30),
                Path = "/"
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            return Ok(new
            {
                User = loginResult.User,
                AccessToken = accessToken,
                TokenType = "Bearer",
                ExpiresInMinutes = 30
            });
        }

        // 3. Refresh Token
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            // Get refresh token from cookie
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { message = "Refresh token not found" });
            }

            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var (newAccessToken, newRefreshToken) = await _repo.RefreshTokensAsync(refreshToken, ip);

                // Update refresh token cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(30),
                    Path = "/"
                };
                Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);

                return Ok(new
                {
                    AccessToken = newAccessToken,
                    TokenType = "Bearer",
                    ExpiresInMinutes = 30
                });
            }
            catch (SecurityException)
            {
                // Clear invalid refresh token cookie
                Response.Cookies.Delete("refreshToken");
                return Unauthorized(new { message = "Invalid refresh token" });
            }
        }

        //// 3. Logout (revoke session)
        //[HttpPost("logout")]
        //[Authorize]
        //public async Task<IActionResult> Logout([FromBody] LogoutRequest req)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (!Guid.TryParse(userId, out var guid)) return Unauthorized();

        //    await _repo.RevokeSessionAsync(req.SessionId, "logout");
        //    return Ok(new { success = true });
        //}

        // 4. Get current user's sessions
        [HttpGet("sessions")]
        [Authorize]
        public async Task<IActionResult> GetSessions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var guid)) return Unauthorized();

            var sessions = await _repo.GetUserSessionsAsync(guid);
            return Ok(sessions);
        }

        //// 5. Revoke all sessions except current
        //[HttpPost("revoke-all-sessions")]
        //[Authorize]
        //public async Task<IActionResult> RevokeAllSessions([FromBody] RevokeAllSessionsRequest req)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (!Guid.TryParse(userId, out var guid)) return Unauthorized();

        //    await _repo.RevokeAllUserSessionsAsync(guid, req.ExceptSessionId);
        //    return Ok(new { success = true });
        //}

        // 6. Start OAuth login (redirect to provider)
        [HttpGet("oauth/{provider}")]
        [AllowAnonymous]
        public IActionResult OAuthRedirect(string provider, [FromQuery] string? redirectUri = null, [FromQuery] string? state = null)
        {
            if (provider.ToLowerInvariant() != "google")
            {
                return BadRequest(new { message = "Unsupported OAuth provider" });
            }

            // Generate a secure state parameter if not provided
            if (string.IsNullOrEmpty(state))
            {
                state = Guid.NewGuid().ToString();
            }

            // Default redirect URI if not provided
            if (string.IsNullOrEmpty(redirectUri))
            {
                redirectUri = $"{Request.Scheme}://{Request.Host}/api/auth/oauth/callback";
            }

            var authUrl = _oauthService.GetGoogleAuthUrl(redirectUri, state);
            
            return Ok(new { 
                authUrl,
                state,
                provider = "google"
            });
        }

        // 7. OAuth callback (provider login)
        [HttpPost("oauth/callback")]
        [AllowAnonymous]
        public async Task<IActionResult> OAuthCallback([FromBody] OAuthCallbackRequest req)
        {
            try
            {
                if (req.Provider.ToLowerInvariant() != "google")
                {
                    return BadRequest(new { message = "Unsupported OAuth provider" });
                }

                // Exchange authorization code for access token
                var tokenResponse = await ExchangeCodeForTokenAsync(req.Code, req.RedirectUri);
                
                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    return BadRequest(new { message = "Failed to obtain access token" });
                }

                // Get user info from Google
                var googleUserInfo = await _oauthService.GetGoogleUserInfoAsync(tokenResponse.AccessToken);
                
                // Find or create user
                var user = await _oauthService.FindOrCreateOAuthUserAsync(googleUserInfo, "google");
                
                if (user == null)
                {
                    return BadRequest(new { message = "Failed to create or find user" });
                }

                // Create session and tokens
                var userAgent = Request.Headers["User-Agent"].ToString();
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var session = await _repo.CreateSessionAsync(user.Id, req.State ?? "", ip, userAgent);
                var refreshToken = await _repo.CreateRefreshTokenAsync(user.Id, session.Id, ip);

                // Get user roles and permissions for JWT
                var roles = await _repo.GetUserRolesAsync(user.Id);
                var permissions = await _repo.GetUserPermissionsAsync(user.Id);

                // Generate JWT access token
                var accessToken = _jwtService.GenerateAccessToken(user, roles, permissions);

                // Set refresh token in httpOnly cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(30),
                    Path = "/"
                };
                Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

                // Log successful OAuth login
                await _repo.LogLoginAttemptAsync(user.Email, ip, "success", userAgent, null, user.Id);

                return Ok(new
                {
                    User = new
                    {
                        user.Id,
                        user.Email,
                        user.Username,
                        user.FirstName,
                        user.LastName,
                        user.Avatar
                    },
                    AccessToken = accessToken,
                    TokenType = "Bearer",
                    ExpiresInMinutes = 30,
                    LoginMethod = "oauth_google"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "OAuth callback failed for provider {Provider}", req.Provider);
                return BadRequest(new { message = "OAuth login failed", error = ex.Message });
            }
        }

        // 8. Email verification request (send token)
        [HttpPost("verify-email/request")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestEmailVerification([FromBody] EmailRequest req)
        {
            var user = await _repo.GetUserByEmailAsync(req.Email);
            if (user == null) return NotFound();

            var token = await _repo.CreateEmailVerificationTokenAsync(user.Id, req.Email);
            // TODO: Send email with token here
            return Ok(new { success = true, token });
        }

        // 9. Email verification (confirm token)
        [HttpPost("verify-email/confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] VerifyEmailRequest req)
        {
            var valid = await _repo.VerifyEmailTokenAsync(req.Token, req.Email);
            if (!valid) return BadRequest(new { success = false });

            return Ok(new { success = true });
        }

        // 10. Password reset request
        [HttpPost("reset-password/request")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordReset([FromBody] EmailRequest req)
        {
            var user = await _repo.GetUserByEmailAsync(req.Email);
            if (user == null) return NotFound();

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var token = await _repo.CreatePasswordResetTokenAsync(user.Id, req.Email, ip);
            // TODO: Send email with token here
            return Ok(new { success = true, token });
        }

        // 11. Password reset confirm
        [HttpPost("reset-password/confirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPasswordReset([FromBody] ResetPasswordRequest req)
        {
            var valid = await _repo.ResetPasswordAsync(req.Token, req.Email, req.NewPassword);
            if (!valid) return BadRequest(new { success = false });

            return Ok(new { success = true });
        }

        // 12. Enable two-factor authentication (returns secret for QR)
        [HttpPost("2fa/setup")]
        [Authorize]
        public async Task<IActionResult> SetupTwoFactor()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var guid)) return Unauthorized();

            var user = await _repo.GetUserByIdAsync(guid);
            if (user == null) return Unauthorized();

            var secret = await _repo.GenerateTwoFactorSecretAsync(guid);
            var backupCodes = await _repo.GenerateBackupCodesAsync(guid);

            // Generate QR code URI for authenticator apps
            var appName = "FrenCircle";
            var qrCodeUri = $"otpauth://totp/{Uri.EscapeDataString(appName)}:{Uri.EscapeDataString(user.Email)}?secret={secret}&issuer={Uri.EscapeDataString(appName)}";

            return Ok(new TwoFactorSetupResponse
            {
                Secret = secret,
                QrCodeUri = qrCodeUri,
                BackupCodes = backupCodes
            });
        }

        [HttpGet("testauth")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Test()
        {
            return Ok("ok");
        }
        // 13. Enable 2FA with code
        [HttpPost("2fa/enable")]
        [Authorize]
        public async Task<IActionResult> EnableTwoFactor([FromBody] TwoFactorEnableRequest req)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var guid)) return Unauthorized();

            var ok = await _repo.EnableTwoFactorAsync(guid, req.Secret, req.Code);
            if (!ok) return BadRequest(new { success = false });

            return Ok(new { success = true });
        }

        // 14. Disable 2FA
        [HttpPost("2fa/disable")]
        [Authorize]
        public async Task<IActionResult> DisableTwoFactor()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var guid)) return Unauthorized();

            await _repo.DisableTwoFactorAsync(guid);
            return Ok(new { success = true });
        }

        // 15. Get user roles & permissions
        [HttpGet("me/roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out var guid)) return Unauthorized();

            var roles = await _repo.GetUserRolesAsync(guid);
            var perms = await _repo.GetUserPermissionsAsync(guid);

            return Ok(new { roles, perms });
        }

        // Helper method to exchange authorization code for access token
        private async Task<GoogleTokenResponse?> ExchangeCodeForTokenAsync(string code, string? redirectUri)
        {
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var googleConfig = configuration.GetSection("Authentication:Google");
            
            using var httpClient = new HttpClient();
            
            var parameters = new Dictionary<string, string>
            {
                {"client_id", googleConfig["ClientId"]!},
                {"client_secret", googleConfig["ClientSecret"]!},
                {"code", code},
                {"grant_type", "authorization_code"},
                {"redirect_uri", redirectUri ?? $"{Request.Scheme}://{Request.Host}/api/auth/oauth/callback"}
            };

            var content = new FormUrlEncodedContent(parameters);
            var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Google token exchange failed: {Error}", errorContent);
                return null;
            }

            var jsonContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse>(jsonContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });

            return tokenResponse;
        }
    }

    // Helper classes for Google OAuth responses
    public class GoogleTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public string TokenType { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public string? Scope { get; set; }
    }
}