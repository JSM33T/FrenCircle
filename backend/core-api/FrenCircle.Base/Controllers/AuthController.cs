using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FrenCircle.Contracts.Auth;
using FrenCircle.Data.Repositories;
using FrenCircle.Data;
using System.Security.Claims;

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
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthRepository repo, ILogger<AuthController> logger)
        {
            _repo = repo;
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

            // Optionally: create session and return tokens
            var userAgent = Request.Headers["User-Agent"].ToString();
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var session = await _repo.CreateSessionAsync(loginResult.UserId!.Value, req.DeviceFingerprint ?? "", ip, userAgent);
            var refreshToken = await _repo.CreateRefreshTokenAsync(loginResult.UserId!.Value, session.Id, ip);

            return Ok(new
            {
                loginResult.User,
                SessionToken = session.SessionToken,
                RefreshToken = refreshToken.Token
            });
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
        public IActionResult OAuthRedirect(string provider, [FromQuery] string? redirectUri = null)
        {
            // This is a placeholder: actual implementation depends on your OAuth workflow
            // Usually handled on frontend or via a dedicated OAuth middleware
            return BadRequest(new { message = "OAuth flow not implemented here. Use external OAuth endpoint." });
        }

        // 7. OAuth callback (provider login)
        [HttpPost("oauth/callback")]
        [AllowAnonymous]
        public async Task<IActionResult> OAuthCallback([FromBody] OAuthCallbackRequest req)
        {
            // Implement provider user resolution, session creation, etc.
            // Use _repo to link UserProvider and issue tokens
            return Ok(new { message = "OAuth login logic to be implemented" });
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

            var secret = await _repo.GenerateTwoFactorSecretAsync(guid);
            return Ok(new { secret });
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
    }
}
