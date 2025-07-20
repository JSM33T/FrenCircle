using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace FrenCircle.Data
{
    // 1. User - Main user entity
    [Table("Users")]
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid Id { get; set; } // Removed dynamic default value

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Username { get; set; }

        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(500)]
        public string? Avatar { get; set; }

        public bool EmailVerified { get; set; } = false;
        public DateTime? EmailVerifiedAt { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsSuspended { get; set; } = false;
        public DateTime? SuspendedAt { get; set; }

        [MaxLength(500)]
        public string? SuspendedReason { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [MaxLength(45)]
        public string? LastLoginIP { get; set; }

        public int LoginCount { get; set; } = 0;

        public bool TwoFactorEnabled { get; set; } = false;

        [MaxLength(255)]
        public string? TwoFactorSecret { get; set; }

        [MaxLength(50)]
        public string Timezone { get; set; } = "UTC";

        [MaxLength(10)]
        public string Locale { get; set; } = "en";

        [Column(TypeName = "jsonb")]
        public string? Preferences { get; set; } // JSON string for preferences

        [Column(TypeName = "jsonb")]
        public string? Metadata { get; set; } // JSON string for metadata

        public DateTime CreatedAt { get; set; } // Removed dynamic default value
        public DateTime UpdatedAt { get; set; } // Removed dynamic default value

        // Navigation properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<UserProvider> UserProviders { get; set; } = new List<UserProvider>();
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<EmailVerification> EmailVerifications { get; set; } = new List<EmailVerification>();
        public virtual ICollection<PasswordReset> PasswordResets { get; set; } = new List<PasswordReset>();
        public virtual ICollection<LoginAttempt> LoginAttempts { get; set; } = new List<LoginAttempt>();
        public virtual ICollection<BackupCode> BackupCodes { get; set; } = new List<BackupCode>();
    }

    // DbContext for the authentication system
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<AuthProvider> AuthProviders { get; set; }
        public DbSet<UserProvider> UserProviders { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<BackupCode> BackupCodes { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                // Remove database-generated UUID default since we generate manually
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("NOW()"); // Use database function for timestamp

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NOW()")
                    .ValueGeneratedOnAddOrUpdate();
            });

            // Role configurations
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NOW()")
                    .ValueGeneratedOnAddOrUpdate();
            });

            // AuthProvider configurations
            modelBuilder.Entity<AuthProvider>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("NOW()")
                    .ValueGeneratedOnAddOrUpdate();
            });

            // UserRole configurations
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RolePermission configurations
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasOne(rp => rp.Role)
                    .WithMany(r => r.RolePermissions)
                    .HasForeignKey(rp => rp.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rp => rp.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // UserProvider configurations
            modelBuilder.Entity<UserProvider>(entity =>
            {
                entity.HasOne(up => up.User)
                    .WithMany(u => u.UserProviders)
                    .HasForeignKey(up => up.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(up => up.Provider)
                    .WithMany(p => p.UserProviders)
                    .HasForeignKey(up => up.ProviderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Session configurations
            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasOne(s => s.User)
                    .WithMany(u => u.Sessions)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Device)
                    .WithMany(d => d.Sessions)
                    .HasForeignKey(s => s.DeviceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // RefreshToken configurations
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(rt => rt.Session)
                    .WithMany()
                    .HasForeignKey(rt => rt.SessionId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // EmailVerification configurations
            modelBuilder.Entity<EmailVerification>(entity =>
            {
                entity.HasOne(ev => ev.User)
                    .WithMany(u => u.EmailVerifications)
                    .HasForeignKey(ev => ev.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // PasswordReset configurations
            modelBuilder.Entity<PasswordReset>(entity =>
            {
                entity.HasOne(pr => pr.User)
                    .WithMany(u => u.PasswordResets)
                    .HasForeignKey(pr => pr.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // LoginAttempt configurations
            modelBuilder.Entity<LoginAttempt>(entity =>
            {
                entity.HasOne(la => la.User)
                    .WithMany(u => u.LoginAttempts)
                    .HasForeignKey(la => la.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // BackupCode configurations
            modelBuilder.Entity<BackupCode>(entity =>
            {
                entity.HasOne(bc => bc.User)
                    .WithMany(u => u.BackupCodes)
                    .HasForeignKey(bc => bc.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed default data
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var userRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var moderatorRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // Static datetime for seed data
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    Description = "System administrator with full access",
                    IsSystem = true,
                    Priority = 100,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Role
                {
                    Id = moderatorRoleId,
                    Name = "Moderator",
                    Description = "Content moderator with limited admin access",
                    IsSystem = true,
                    Priority = 50,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Role
                {
                    Id = userRoleId,
                    Name = "User",
                    Description = "Regular user with basic access",
                    IsSystem = true,
                    Priority = 1,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );

            modelBuilder.Entity<Permission>().HasData(
                new Permission
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "users.create",
                    Description = "Create users",
                    Category = "Users",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Permission
                {
                    Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Name = "users.read",
                    Description = "View users",
                    Category = "Users",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Permission
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Name = "users.update",
                    Description = "Update users",
                    Category = "Users",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Permission
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Name = "users.delete",
                    Description = "Delete users",
                    Category = "Users",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Permission
                {
                    Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    Name = "roles.manage",
                    Description = "Manage roles",
                    Category = "Roles",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Permission
                {
                    Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"),
                    Name = "system.admin",
                    Description = "System administration",
                    Category = "System",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new Permission
                {
                    Id = Guid.Parse("11111111-2222-3333-4444-555555555555"),
                    Name = "content.moderate",
                    Description = "Moderate content",
                    Category = "Content",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );

            modelBuilder.Entity<AuthProvider>().HasData(
                new AuthProvider
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-000000000001"),
                    Name = "google",
                    DisplayName = "Google",
                    ClientId = "your-google-client-id",
                    ClientSecret = "your-google-client-secret",
                    AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth",
                    TokenUrl = "https://oauth2.googleapis.com/token",
                    UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo",
                    Scopes = "openid,profile,email",
                    SortOrder = 1,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new AuthProvider
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-000000000002"),
                    Name = "github",
                    DisplayName = "GitHub",
                    ClientId = "your-github-client-id",
                    ClientSecret = "your-github-client-secret",
                    AuthorizeUrl = "https://github.com/login/oauth/authorize",
                    TokenUrl = "https://github.com/login/oauth/access_token",
                    UserInfoUrl = "https://api.github.com/user",
                    Scopes = "user:email",
                    SortOrder = 2,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                },
                new AuthProvider
                {
                    Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-000000000003"),
                    Name = "microsoft",
                    DisplayName = "Microsoft",
                    ClientId = "your-microsoft-client-id",
                    ClientSecret = "your-microsoft-client-secret",
                    AuthorizeUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize",
                    TokenUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token",
                    UserInfoUrl = "https://graph.microsoft.com/v1.0/me",
                    Scopes = "openid,profile,email",
                    SortOrder = 3,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate
                }
            );
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is User || e.Entity is Role || e.Entity is AuthProvider)
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Property("CreatedAt") != null)
                        entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                }

                if (entry.Property("UpdatedAt") != null)
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }
    }

    // Email Template entity
    [Table("EmailTemplates")]
    public class EmailTemplate
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(500)]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Body { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
    }

    // Job status enum
    public enum JobStatus
    {
        Pending = 0,
        Running = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4
    }

    // Job priority enum
    public enum JobPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Critical = 3
    }

    // Job entity
    [Table("Jobs")]
    public class Job
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string JobName { get; set; } = string.Empty;
        
        public JobStatus Status { get; set; } = JobStatus.Pending;
        public JobPriority Priority { get; set; } = JobPriority.Normal;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? StartedDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        
        [MaxLength(100)]
        public string? TriggeredBy { get; set; }
        
        public string? Metadata { get; set; }
        
        [MaxLength(2000)]
        public string? ErrorMessage { get; set; }
        
        public int RetryCount { get; set; } = 0;
        public int MaxRetries { get; set; } = 3;
    }
}