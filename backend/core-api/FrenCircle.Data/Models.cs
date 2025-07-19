using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace FrenCircle.Data
{
    // 2. Role - Role-based access control
    [Table("Roles")]
    [Index(nameof(Name), IsUnique = true)]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsSystem { get; set; } = false;
        public int Priority { get; set; } = 0;

        public DateTime CreatedAt { get; set; } // Removed dynamic default value
        public DateTime UpdatedAt { get; set; } // Removed dynamic default value

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    // 3. UserRole - User-role assignment (many-to-many)
    [Table("UserRoles")]
    [Index(nameof(UserId), nameof(RoleId), IsUnique = true)]
    public class UserRole
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        public DateTime? ExpiresAt { get; set; }
        public DateTime AssignedAt { get; set; } // Removed dynamic default value
        public Guid? AssignedBy { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;
    }

    // 4. Permission - Granular permissions
    [Table("Permissions")]
    [Index(nameof(Name), IsUnique = true)]
    public class Permission
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? Category { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } // Removed dynamic default value
        public DateTime UpdatedAt { get; set; } // Added UpdatedAt property

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    [Table("RolePermissions")]
    [Index(nameof(RoleId), nameof(PermissionId), IsUnique = true)]
    public class RolePermission
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public Guid PermissionId { get; set; }

        public DateTime GrantedAt { get; set; } // Removed dynamic default value
        public Guid? GrantedBy { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; } = null!;

        [ForeignKey(nameof(PermissionId))]
        public virtual Permission Permission { get; set; } = null!;
    }

    // 6. AuthProvider - OAuth provider configurations
    [Table("AuthProviders")]
    [Index(nameof(Name), IsUnique = true)]
    public class AuthProvider
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // google, github, facebook, etc.

        [Required]
        [MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Icon { get; set; } // Icon URL or class

        [Required]
        [MaxLength(255)]
        public string ClientId { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string ClientSecret { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? AuthorizeUrl { get; set; }

        [MaxLength(500)]
        public string? TokenUrl { get; set; }

        [MaxLength(500)]
        public string? UserInfoUrl { get; set; }

        [MaxLength(500)]
        public string? Scopes { get; set; } // Comma-separated scopes

        public bool IsEnabled { get; set; } = true;
        public bool IsVisible { get; set; } = true; // Show on login page
        public int SortOrder { get; set; } = 0;

        [Column(TypeName = "jsonb")]
        public string? Configuration { get; set; } // Additional provider-specific config

        public DateTime CreatedAt { get; set; } // Removed dynamic default value
        public DateTime UpdatedAt { get; set; } // Removed dynamic default value

        // Navigation properties
        public virtual ICollection<UserProvider> UserProviders { get; set; } = new List<UserProvider>();
    }

    // 7. UserProvider - User's connected OAuth accounts
    [Table("UserProviders")]
    [Index(nameof(ProviderId), nameof(ProviderUserId), IsUnique = true)]
    public class UserProvider
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProviderId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProviderUserId { get; set; } = string.Empty; // ID from OAuth provider

        [MaxLength(255)]
        public string? ProviderUsername { get; set; }

        [MaxLength(255)]
        public string? ProviderEmail { get; set; }

        [MaxLength(500)]
        public string? AccessToken { get; set; }

        [MaxLength(500)]
        public string? RefreshToken { get; set; }

        public DateTime? TokenExpiresAt { get; set; }

        [Column(TypeName = "jsonb")]
        public string? ProviderData { get; set; } // Additional data from provider

        public DateTime ConnectedAt { get; set; } // Removed dynamic default value
        public DateTime? LastSyncAt { get; set; }
        public DateTime UpdatedAt { get; set; } // Added UpdatedAt property

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(ProviderId))]
        public virtual AuthProvider Provider { get; set; } = null!;
    }

    // 8. Device - Device fingerprinting for sessions
    [Table("Devices")]
    [Index(nameof(Fingerprint), IsUnique = true)]
    public class Device
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        [MaxLength(255)]
        public string Fingerprint { get; set; } = string.Empty; // Device fingerprint hash

        [MaxLength(255)]
        public string? UserAgent { get; set; }

        [MaxLength(100)]
        public string? DeviceType { get; set; } // mobile, desktop, tablet

        [MaxLength(100)]
        public string? Browser { get; set; }

        [MaxLength(50)]
        public string? BrowserVersion { get; set; }

        [MaxLength(100)]
        public string? OS { get; set; }

        [MaxLength(50)]
        public string? OSVersion { get; set; }

        [MaxLength(100)]
        public string? DeviceName { get; set; } // Custom name set by user

        public bool IsTrusted { get; set; } = false; // User-marked trusted device

        public DateTime FirstSeenAt { get; set; } // Removed dynamic default value
        public DateTime LastSeenAt { get; set; } // Removed dynamic default value
        public DateTime UpdatedAt { get; set; } // Added UpdatedAt property

        // Navigation properties
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
    }

    // 9. Session - Active user sessions with device tracking
    [Table("Sessions")]
    [Index(nameof(SessionToken), IsUnique = true)]
    [Index(nameof(UserId))]
    public class Session
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string SessionToken { get; set; } = string.Empty; // JWT or session ID

        [MaxLength(45)]
        public string? IPAddress { get; set; }

        [MaxLength(255)]
        public string? Location { get; set; } // City, Country from IP

        public DateTime CreatedAt { get; set; } // Removed dynamic default value
        public DateTime LastActivityAt { get; set; } // Removed dynamic default value
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; } // Added UpdatedAt property

        public bool IsActive { get; set; } = true;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedReason { get; set; } // logout, security, expired

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(DeviceId))]
        public virtual Device Device { get; set; } = null!;
    }

    // 10. RefreshToken - JWT refresh token management
    [Table("RefreshTokens")]
    [Index(nameof(Token), IsUnique = true)]
    public class RefreshToken
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        public Guid UserId { get; set; }

        public Guid? SessionId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } // Removed dynamic default value
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; } // Added UpdatedAt property

        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; }

        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedReason { get; set; }

        [MaxLength(45)]
        public string? IPAddress { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(SessionId))]
        public virtual Session? Session { get; set; }
    }

    // 11. EmailVerification - Email verification tokens
    [Table("EmailVerifications")]
    [Index(nameof(Token), IsUnique = true)]
    public class EmailVerification
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } // Removed dynamic default value
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; } // Added UpdatedAt property

        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; }

        [MaxLength(45)]
        public string? IPAddress { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }

    // 12. PasswordReset - Password recovery tokens
    [Table("PasswordResets")]
    [Index(nameof(Token), IsUnique = true)]
    public class PasswordReset
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } // Removed dynamic default value
        public DateTime ExpiresAt { get; set; }
        public DateTime UpdatedAt { get; set; } // Added UpdatedAt property

        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; }

        [MaxLength(45)]
        public string? IPAddress { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }

    // 13. LoginAttempt - Security logging and rate limiting
    [Table("LoginAttempts")]
    [Index(nameof(Email))]
    [Index(nameof(IPAddress))]
    public class LoginAttempt
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        public Guid? UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(45)]
        public string IPAddress { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? UserAgent { get; set; }

        [Required]
        [MaxLength(50)]
        public string Result { get; set; } = string.Empty; // success, failed_password, failed_2fa, blocked

        [MaxLength(255)]
        public string? FailureReason { get; set; }

        [MaxLength(50)]
        public string? AuthMethod { get; set; } // password, google, github, etc.

        public DateTime CreatedAt { get; set; } // Removed dynamic default value

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }

    // 14. BackupCode - Two-factor backup codes
    [Table("BackupCodes")]
    [Index(nameof(UserId))]
    public class BackupCode
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string CodeHash { get; set; } = string.Empty; // Hashed backup code

        public bool IsUsed { get; set; } = false;
        public DateTime? UsedAt { get; set; }

        [MaxLength(45)]
        public string? UsedFromIP { get; set; }

        public DateTime CreatedAt { get; set; } // Removed dynamic default value

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}