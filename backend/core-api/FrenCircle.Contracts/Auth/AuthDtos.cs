using System.ComponentModel.DataAnnotations;

namespace FrenCircle.Contracts.Auth
{
    // Request DTOs moved from AuthController
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

    public class TwoFactorEnableRequest
    {
        public string Secret { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class EmailRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    // GoogleTokenResponse is a helper class for OAuth
    public class GoogleTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public string TokenType { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public string? Scope { get; set; }
    }
    // Request DTOs
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Username { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(50)]
        public string? Timezone { get; set; } = "UTC";

        [MaxLength(10)]
        public string? Locale { get; set; } = "en";
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string? DeviceFingerprint { get; set; }
        public bool RememberMe { get; set; } = false;
        public string? TwoFactorCode { get; set; }
    }

    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        public string NewPassword { get; set; } = string.Empty;
    }

    public class Enable2FARequest
    {
        [Required]
        public string TwoFactorCode { get; set; } = string.Empty;
    }

    // Response DTOs
    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; } = new();
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
        public bool EmailVerified { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string Timezone { get; set; } = "UTC";
        public string Locale { get; set; } = "en";
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class TwoFactorSetupResponse
    {
        public string Secret { get; set; } = string.Empty;
        public string QrCodeUri { get; set; } = string.Empty;
        public List<string> BackupCodes { get; set; } = new();
    }

    public class SessionDto
    {
        public Guid Id { get; set; }
        public string DeviceName { get; set; } = string.Empty;
        public string? DeviceType { get; set; }
        public string? Browser { get; set; }
        public string? OS { get; set; }
        public string? IPAddress { get; set; }
        public string? Location { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
    }

    public class LoginAttemptDto
    {
        public string Email { get; set; } = string.Empty;
        public string IPAddress { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string? FailureReason { get; set; }
        public string? AuthMethod { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // Internal DTOs for operations
    public class CreateUserResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public Guid? UserId { get; set; }
        public string? EmailVerificationToken { get; set; }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public Guid? UserId { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public bool RequiresEmailVerification { get; set; }
        public UserDto? User { get; set; }
    }
}
