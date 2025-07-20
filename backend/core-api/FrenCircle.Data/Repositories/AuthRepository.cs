using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using FrenCircle.Contracts.Auth;

namespace FrenCircle.Data.Repositories
{
    /// <summary>
    /// Repository interface for authentication-related operations including user management, 
    /// session handling, token management, email verification, password reset, two-factor authentication,
    /// security logging, device management, and role/permission management.
    /// </summary>
    public interface IAuthRepository
    {
        #region User Management
        
        /// <summary>
        /// Creates a new user account with the provided registration details.
        /// </summary>
        /// <param name="request">Registration request containing user details</param>
        /// <param name="ipAddress">IP address of the registration request</param>
        /// <param name="userAgent">User agent string from the client browser</param>
        /// <returns>Result indicating success/failure and user ID if successful</returns>
        Task<CreateUserResult> CreateUserAsync(RegisterRequest request, string ipAddress, string? userAgent = null);
        
        /// <summary>
        /// Authenticates a user with email and password credentials.
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <param name="password">User's password</param>
        /// <returns>Login result with user information if successful</returns>
        Task<LoginResult> AuthenticateUserAsync(string email, string password);
        
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique user identifier</param>
        /// <returns>User entity if found, otherwise null</returns>
        Task<User?> GetUserByIdAsync(Guid userId);
        
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <returns>User entity if found, otherwise null</returns>
        Task<User?> GetUserByEmailAsync(string email);
        
        /// <summary>
        /// Retrieves a user by their username.
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <returns>User entity if found, otherwise null</returns>
        Task<User?> GetUserByUsernameAsync(string username);
        
        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="user">User entity with updated information</param>
        /// <returns>True if update was successful, otherwise false</returns>
        Task<bool> UpdateUserAsync(User user);
        
        /// <summary>
        /// Changes a user's password after verifying the current password.
        /// </summary>
        /// <param name="userId">The user's unique identifier</param>
        /// <param name="currentPassword">The user's current password</param>
        /// <param name="newPassword">The new password to set</param>
        /// <returns>True if password change was successful, otherwise false</returns>
        Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
        
        #endregion

        // Session management
        Task<Session> CreateSessionAsync(Guid userId, string deviceFingerprint, string ipAddress, string? userAgent = null);
        Task<Session> CreateOrUpdateSessionAsync(Guid userId, string deviceFingerprint, string ipAddress, string? userAgent = null);
        Task<Session?> GetSessionByTokenAsync(string sessionToken);
        Task<List<SessionDto>> GetUserSessionsAsync(Guid userId);
        Task<bool> UpdateSessionActivityAsync(Guid sessionId);
        Task<bool> RevokeSessionAsync(Guid sessionId, string reason = "logout");
        Task<bool> RevokeAllUserSessionsAsync(Guid userId, Guid? exceptSessionId = null);

        // Token management
        Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, Guid sessionId, string ipAddress);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task<bool> RevokeRefreshTokenAsync(string token, string reason = "used");
        Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshToken, string ipAddress);

        // Email verification
        Task<string> CreateEmailVerificationTokenAsync(Guid userId, string email);
        Task<bool> VerifyEmailTokenAsync(string token, string email);
        Task<bool> IsEmailVerificationTokenValidAsync(string token);

        // Password reset
        Task<string> CreatePasswordResetTokenAsync(Guid userId, string email, string ipAddress);
        Task<bool> ValidatePasswordResetTokenAsync(string token, string email);
        Task<bool> ResetPasswordAsync(string token, string email, string newPassword);

        // Two-factor authentication
        Task<string> GenerateTwoFactorSecretAsync(Guid userId);
        Task<bool> EnableTwoFactorAsync(Guid userId, string secret, string code);
        Task<bool> DisableTwoFactorAsync(Guid userId);
        Task<bool> VerifyTwoFactorCodeAsync(Guid userId, string code);
        Task<List<string>> GenerateBackupCodesAsync(Guid userId);
        Task<bool> UseBackupCodeAsync(Guid userId, string code, string ipAddress);

        // Security & logging
        Task LogLoginAttemptAsync(string email, string ipAddress, string result, string? userAgent = null, string? failureReason = null, Guid? userId = null);
        Task<List<LoginAttemptDto>> GetRecentLoginAttemptsAsync(string email, int hours = 24);
        Task<int> GetFailedLoginAttemptsCountAsync(string email, int minutes = 15);
        Task<bool> IsAccountLockedAsync(string email);

        // Device management
        Task<Device> GetOrCreateDeviceAsync(string fingerprint, string? userAgent = null);
        Task<bool> TrustDeviceAsync(Guid deviceId);

        // Role & permission management
        Task<List<string>> GetUserRolesAsync(Guid userId);
        Task<List<string>> GetUserPermissionsAsync(Guid userId);
        Task<bool> AssignRoleToUserAsync(Guid userId, string roleName, Guid? assignedBy = null);
        Task<bool> RemoveRoleFromUserAsync(Guid userId, string roleName);
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly AuthDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthRepository(AuthDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        #region User Management

        public async Task<CreateUserResult> CreateUserAsync(RegisterRequest request, string ipAddress, string? userAgent = null)
        {
            try
            {
                // Check if user already exists
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return new CreateUserResult { Success = false, ErrorMessage = "Email already registered" };
                }

                if (!string.IsNullOrEmpty(request.Username) && 
                    await _context.Users.AnyAsync(u => u.Username == request.Username))
                {
                    return new CreateUserResult { Success = false, ErrorMessage = "Username already taken" };
                }

                var userId = Guid.NewGuid(); // Generate GUID manually
                var user = new User
                {
                    Id = userId, // Set the ID explicitly
                    Email = request.Email.ToLowerInvariant(),
                    Username = request.Username?.ToLowerInvariant(),
                    PasswordHash = HashPassword(request.Password),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Timezone = request.Timezone ?? "UTC",
                    Locale = request.Locale ?? "en",
                    EmailVerified = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                // Assign default user role
                var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                if (userRole != null)
                {
                    _context.UserRoles.Add(new UserRole
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        RoleId = userRole.Id,
                        AssignedAt = DateTime.UtcNow
                    });
                }

                // Create email verification token
                var verificationToken = GenerateSecureToken();
                _context.EmailVerifications.Add(new EmailVerification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Email = user.Email,
                    Token = verificationToken,
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    IPAddress = ipAddress,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                return new CreateUserResult
                {
                    Success = true,
                    UserId = userId,
                    EmailVerificationToken = verificationToken
                };
            }
            catch (Exception)
            {
                return new CreateUserResult { Success = false, ErrorMessage = "Registration failed" };
            }
        }

        public async Task<LoginResult> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return new LoginResult { Success = false, ErrorMessage = "Invalid credentials" };
            }

            if (user.IsSuspended)
            {
                return new LoginResult { Success = false, ErrorMessage = "Account suspended" };
            }

            if (!user.IsActive)
            {
                return new LoginResult { Success = false, ErrorMessage = "Account deactivated" };
            }

            if (!user.EmailVerified)
            {
                return new LoginResult 
                { 
                    Success = false, 
                    ErrorMessage = "Email not verified",
                    RequiresEmailVerification = true,
                    UserId = user.Id
                };
            }

            if (user.TwoFactorEnabled)
            {
                return new LoginResult
                {
                    Success = false,
                    ErrorMessage = "Two-factor authentication required",
                    RequiresTwoFactor = true,
                    UserId = user.Id
                };
            }

            var userDto = MapUserToDto(user);
            return new LoginResult
            {
                Success = true,
                UserId = user.Id,
                User = userDto
            };
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant());
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username.ToLowerInvariant());
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null || !VerifyPassword(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = HashPassword(newPassword);
            return await UpdateUserAsync(user);
        }

        #endregion

        #region Session Management

        public async Task<Session> CreateOrUpdateSessionAsync(Guid userId, string deviceFingerprint, string ipAddress, string? userAgent = null)
        {
            var device = await GetOrCreateDeviceAsync(deviceFingerprint, userAgent);
            
            // Find existing active session for this user and device
            var existingSession = await _context.Sessions
                .Include(s => s.Device)
                .FirstOrDefaultAsync(s => s.UserId == userId && 
                                         s.DeviceId == device.Id && 
                                         s.IsActive && 
                                         s.ExpiresAt > DateTime.UtcNow);

            if (existingSession != null)
            {
                // Update existing session
                existingSession.SessionToken = GenerateJwtToken();
                existingSession.IPAddress = ipAddress;
                existingSession.LastActivityAt = DateTime.UtcNow;
                existingSession.UpdatedAt = DateTime.UtcNow;
                existingSession.ExpiresAt = DateTime.UtcNow.AddDays(30);

                await _context.SaveChangesAsync();
                return existingSession;
            }

            // Create new session if none exists
            var newSession = await CreateSessionAsync(userId, deviceFingerprint, ipAddress, userAgent);
            
            // Load the Device navigation property
            await _context.Entry(newSession)
                .Reference(s => s.Device)
                .LoadAsync();
                
            return newSession;
        }

        public async Task<Session> CreateSessionAsync(Guid userId, string deviceFingerprint, string ipAddress, string? userAgent = null)
        {
            var device = await GetOrCreateDeviceAsync(deviceFingerprint, userAgent);
            
            var session = new Session
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DeviceId = device.Id,
                SessionToken = GenerateJwtToken(), // You'll need to implement JWT generation
                IPAddress = ipAddress,
                ExpiresAt = DateTime.UtcNow.AddDays(30), // Adjust as needed
                CreatedAt = DateTime.UtcNow,
                LastActivityAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session?> GetSessionByTokenAsync(string sessionToken)
        {
            return await _context.Sessions
                .Include(s => s.User)
                .Include(s => s.Device)
                .FirstOrDefaultAsync(s => s.SessionToken == sessionToken && 
                                         s.IsActive && 
                                         s.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<List<SessionDto>> GetUserSessionsAsync(Guid userId)
        {
            return await _context.Sessions
                .Include(s => s.Device)
                .Where(s => s.UserId == userId && s.IsActive)
                .Select(s => new SessionDto
                {
                    Id = s.Id,
                    DeviceName = s.Device.DeviceName ?? "Unknown Device",
                    DeviceType = s.Device.DeviceType,
                    Browser = s.Device.Browser,
                    OS = s.Device.OS,
                    IPAddress = s.IPAddress,
                    Location = s.Location,
                    CreatedAt = s.CreatedAt,
                    LastActivityAt = s.LastActivityAt
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateSessionActivityAsync(Guid sessionId)
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null) return false;

            session.LastActivityAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RevokeSessionAsync(Guid sessionId, string reason = "logout")
        {
            var session = await _context.Sessions.FindAsync(sessionId);
            if (session == null) return false;

            session.IsActive = false;
            session.RevokedAt = DateTime.UtcNow;
            session.RevokedReason = reason;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RevokeAllUserSessionsAsync(Guid userId, Guid? exceptSessionId = null)
        {
            var sessions = await _context.Sessions
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync();

            if (exceptSessionId.HasValue)
            {
                sessions = sessions.Where(s => s.Id != exceptSessionId.Value).ToList();
            }

            foreach (var session in sessions)
            {
                session.IsActive = false;
                session.RevokedAt = DateTime.UtcNow;
                session.RevokedReason = "revoked_all";
            }

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Token Management

        public async Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, Guid sessionId, string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SessionId = sessionId,
                Token = GenerateSecureToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IPAddress = ipAddress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .Include(rt => rt.Session)
                .FirstOrDefaultAsync(rt => rt.Token == token && 
                                          !rt.IsUsed && 
                                          !rt.IsRevoked && 
                                          rt.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token, string reason = "used")
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token);
            if (refreshToken == null) return false;

            if (reason == "used")
            {
                refreshToken.IsUsed = true;
                refreshToken.UsedAt = DateTime.UtcNow;
            }
            else
            {
                refreshToken.IsRevoked = true;
                refreshToken.RevokedAt = DateTime.UtcNow;
                refreshToken.RevokedReason = reason;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokensAsync(string refreshToken, string ipAddress)
        {
            var storedToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                    .ThenInclude(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                            .ThenInclude(r => r.RolePermissions)
                                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && 
                                          !rt.IsUsed && 
                                          !rt.IsRevoked && 
                                          rt.ExpiresAt > DateTime.UtcNow);

            if (storedToken == null)
                throw new SecurityException("Invalid refresh token");

            // Mark the old refresh token as used
            storedToken.IsUsed = true;
            storedToken.UsedAt = DateTime.UtcNow;

            // Get user roles and permissions
            var roles = await GetUserRolesAsync(storedToken.UserId);
            var permissions = await GetUserPermissionsAsync(storedToken.UserId);

            // Generate new tokens
            var newAccessToken = _jwtService.GenerateAccessToken(storedToken.User, roles, permissions);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            // Create new refresh token in database
            var newRefreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = storedToken.UserId,
                SessionId = storedToken.SessionId,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IPAddress = ipAddress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);
            await _context.SaveChangesAsync();

            return (newAccessToken, newRefreshToken);
        }

        #endregion

        #region Email Verification

        public async Task<string> CreateEmailVerificationTokenAsync(Guid userId, string email)
        {
            var token = GenerateSecureToken();
            
            _context.EmailVerifications.Add(new EmailVerification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<bool> VerifyEmailTokenAsync(string token, string email)
        {
            var verification = await _context.EmailVerifications
                .FirstOrDefaultAsync(ev => ev.Token == token && 
                                          ev.Email == email && 
                                          !ev.IsUsed && 
                                          ev.ExpiresAt > DateTime.UtcNow);

            if (verification == null) return false;

            var user = await GetUserByIdAsync(verification.UserId);
            if (user == null) return false;

            user.EmailVerified = true;
            user.EmailVerifiedAt = DateTime.UtcNow;

            verification.IsUsed = true;
            verification.UsedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsEmailVerificationTokenValidAsync(string token)
        {
            return await _context.EmailVerifications
                .AnyAsync(ev => ev.Token == token && 
                               !ev.IsUsed && 
                               ev.ExpiresAt > DateTime.UtcNow);
        }

        #endregion

        #region Password Reset

        public async Task<string> CreatePasswordResetTokenAsync(Guid userId, string email, string ipAddress)
        {
            var token = GenerateSecureToken();
            
            _context.PasswordResets.Add(new PasswordReset
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                IPAddress = ipAddress,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<bool> ValidatePasswordResetTokenAsync(string token, string email)
        {
            return await _context.PasswordResets
                .AnyAsync(pr => pr.Token == token && 
                               pr.Email == email && 
                               !pr.IsUsed && 
                               pr.ExpiresAt > DateTime.UtcNow);
        }

        public async Task<bool> ResetPasswordAsync(string token, string email, string newPassword)
        {
            var reset = await _context.PasswordResets
                .FirstOrDefaultAsync(pr => pr.Token == token && 
                                          pr.Email == email && 
                                          !pr.IsUsed && 
                                          pr.ExpiresAt > DateTime.UtcNow);

            if (reset == null) return false;

            var user = await GetUserByIdAsync(reset.UserId);
            if (user == null) return false;

            user.PasswordHash = HashPassword(newPassword);
            reset.IsUsed = true;
            reset.UsedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Two-Factor Authentication

        public async Task<string> GenerateTwoFactorSecretAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) throw new ArgumentException("User not found");

            var secret = GenerateSecureToken(32);
            user.TwoFactorSecret = secret;
            
            await UpdateUserAsync(user);
            return secret;
        }

        public async Task<bool> EnableTwoFactorAsync(Guid userId, string secret, string code)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null || user.TwoFactorSecret != secret) return false;

            if (!VerifyTotpCode(secret, code)) return false;

            user.TwoFactorEnabled = true;
            return await UpdateUserAsync(user);
        }

        public async Task<bool> DisableTwoFactorAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) return false;

            user.TwoFactorEnabled = false;
            user.TwoFactorSecret = null;

            // Remove backup codes
            var backupCodes = await _context.BackupCodes.Where(bc => bc.UserId == userId).ToListAsync();
            _context.BackupCodes.RemoveRange(backupCodes);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyTwoFactorCodeAsync(Guid userId, string code)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null || !user.TwoFactorEnabled || string.IsNullOrEmpty(user.TwoFactorSecret))
                return false;

            return VerifyTotpCode(user.TwoFactorSecret, code);
        }

        public async Task<List<string>> GenerateBackupCodesAsync(Guid userId)
        {
            // Remove existing backup codes
            var existingCodes = await _context.BackupCodes.Where(bc => bc.UserId == userId).ToListAsync();
            _context.BackupCodes.RemoveRange(existingCodes);

            var codes = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var code = GenerateBackupCode();
                codes.Add(code);
                
                _context.BackupCodes.Add(new BackupCode
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CodeHash = HashPassword(code),
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            return codes;
        }

        public async Task<bool> UseBackupCodeAsync(Guid userId, string code, string ipAddress)
        {
            var backupCodes = await _context.BackupCodes
                .Where(bc => bc.UserId == userId && !bc.IsUsed)
                .ToListAsync();

            foreach (var backupCode in backupCodes)
            {
                if (VerifyPassword(code, backupCode.CodeHash))
                {
                    backupCode.IsUsed = true;
                    backupCode.UsedAt = DateTime.UtcNow;
                    backupCode.UsedFromIP = ipAddress;
                    
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Security & Logging

        public async Task LogLoginAttemptAsync(string email, string ipAddress, string result, string? userAgent = null, string? failureReason = null, Guid? userId = null)
        {
            _context.LoginAttempts.Add(new LoginAttempt
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Email = email.ToLowerInvariant(),
                IPAddress = ipAddress,
                UserAgent = userAgent,
                Result = result,
                FailureReason = failureReason,
                AuthMethod = "password",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<LoginAttemptDto>> GetRecentLoginAttemptsAsync(string email, int hours = 24)
        {
            var since = DateTime.UtcNow.AddHours(-hours);
            
            return await _context.LoginAttempts
                .Where(la => la.Email == email.ToLowerInvariant() && la.CreatedAt >= since)
                .OrderByDescending(la => la.CreatedAt)
                .Select(la => new LoginAttemptDto
                {
                    Email = la.Email,
                    IPAddress = la.IPAddress,
                    Result = la.Result,
                    FailureReason = la.FailureReason,
                    AuthMethod = la.AuthMethod,
                    CreatedAt = la.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<int> GetFailedLoginAttemptsCountAsync(string email, int minutes = 15)
        {
            var since = DateTime.UtcNow.AddMinutes(-minutes);
            
            return await _context.LoginAttempts
                .CountAsync(la => la.Email == email.ToLowerInvariant() && 
                                 la.CreatedAt >= since && 
                                 la.Result.StartsWith("failed"));
        }

        public async Task<bool> IsAccountLockedAsync(string email)
        {
            var failedAttempts = await GetFailedLoginAttemptsCountAsync(email, 15);
            return failedAttempts >= 5; // Lock after 5 failed attempts in 15 minutes
        }

        #endregion

        #region Device Management

        public async Task<Device> GetOrCreateDeviceAsync(string fingerprint, string? userAgent = null)
        {
            var device = await _context.Devices.FirstOrDefaultAsync(d => d.Fingerprint == fingerprint);
            
            if (device == null)
            {
                device = new Device
                {
                    Id = Guid.NewGuid(),
                    Fingerprint = fingerprint,
                    UserAgent = userAgent,
                    DeviceType = ParseDeviceType(userAgent),
                    Browser = ParseBrowser(userAgent),
                    OS = ParseOS(userAgent),
                    DeviceName = ParseDeviceName(userAgent), // Use parsed name from UserAgent
                    FirstSeenAt = DateTime.UtcNow,
                    LastSeenAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Devices.Add(device);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Update device info if UserAgent has changed
                if (!string.IsNullOrEmpty(userAgent) && device.UserAgent != userAgent)
                {
                    device.UserAgent = userAgent;
                    device.DeviceType = ParseDeviceType(userAgent);
                    device.Browser = ParseBrowser(userAgent);
                    device.OS = ParseOS(userAgent);
                    device.DeviceName = ParseDeviceName(userAgent);
                }
                
                device.LastSeenAt = DateTime.UtcNow;
                device.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return device;
        }

        public async Task<bool> TrustDeviceAsync(Guid deviceId)
        {
            var device = await _context.Devices.FindAsync(deviceId);
            if (device == null) return false;

            device.IsTrusted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Role & Permission Management

        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId && ur.Role.IsActive)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
        }

        public async Task<List<string>> GetUserPermissionsAsync(Guid userId)
        {
            return await _context.UserRoles
                .Include(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .Where(ur => ur.UserId == userId && ur.Role.IsActive)
                .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Name))
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, string roleName, Guid? assignedBy = null)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) return false;

            var existingRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == role.Id);
            if (existingRole != null) return true; // Already assigned

            _context.UserRoles.Add(new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RoleId = role.Id,
                AssignedBy = assignedBy,
                AssignedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, string roleName)
        {
            var userRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.Role.Name == roleName);

            if (userRole == null) return false;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Helper Methods

        private string HashPassword(string password)
        {
            // Use BCrypt or similar for production
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "salt")); // Add proper salt
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string? hash)
        {
            if (string.IsNullOrEmpty(hash)) return false;
            return HashPassword(password) == hash;
        }

        private string GenerateSecureToken(int length = 64)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }

        private string GenerateJwtToken()
        {
            return _jwtService.GenerateRefreshToken();
        }

        private string GenerateBackupCode()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[5];
            rng.GetBytes(bytes);
            return Convert.ToHexString(bytes).ToLower();
        }

        private bool VerifyTotpCode(string secret, string code)
        {
            // Implement TOTP verification here
            // You'll need a library like OtpNet
            return true; // Placeholder
        }

        private UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                EmailVerified = user.EmailVerified,
                TwoFactorEnabled = user.TwoFactorEnabled,
                Timezone = user.Timezone,
                Locale = user.Locale,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
                Permissions = user.UserRoles.SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Name)).Distinct().ToList(),
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }

        private string? ParseDeviceName(string? userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown Device";
            
            // Extract a meaningful device name from User Agent
            var browser = ParseBrowser(userAgent);
            var os = ParseOS(userAgent);
            var deviceType = ParseDeviceType(userAgent);
            
            // Create a descriptive name combining the parsed information
            var parts = new List<string>();
            
            if (!string.IsNullOrEmpty(browser) && browser != "Unknown")
                parts.Add(browser);
            
            if (!string.IsNullOrEmpty(os) && os != "Unknown")
                parts.Add($"on {os}");
            
            if (!string.IsNullOrEmpty(deviceType) && deviceType != "Unknown")
                parts.Add($"({deviceType})");
            
            if (parts.Count == 0)
                return "Unknown Device";
            
            return string.Join(" ", parts);
        }

        private string? ParseDeviceType(string? userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";
            
            var ua = userAgent.ToLowerInvariant();
            
            if (ua.Contains("mobile") || ua.Contains("android") || ua.Contains("iphone") || ua.Contains("ipod"))
                return "Mobile";
            if (ua.Contains("tablet") || ua.Contains("ipad"))
                return "Tablet";
            if (ua.Contains("tv") || ua.Contains("smart-tv"))
                return "Smart TV";
            
            return "Desktop";
        }

        private string? ParseBrowser(string? userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";
            
            var ua = userAgent.ToLowerInvariant();
            
            // Order matters - check more specific browsers first
            if (ua.Contains("edg/") || ua.Contains("edge/"))
                return "Microsoft Edge";
            if (ua.Contains("chrome/") && !ua.Contains("chromium"))
                return "Google Chrome";
            if (ua.Contains("firefox/"))
                return "Mozilla Firefox";
            if (ua.Contains("safari/") && !ua.Contains("chrome"))
                return "Safari";
            if (ua.Contains("opera/") || ua.Contains("opr/"))
                return "Opera";
            if (ua.Contains("chromium/"))
                return "Chromium";
            
            return "Unknown";
        }

        private string? ParseOS(string? userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";
            
            var ua = userAgent.ToLowerInvariant();
            
            if (ua.Contains("windows nt 10"))
                return "Windows 10/11";
            if (ua.Contains("windows nt 6.3"))
                return "Windows 8.1";
            if (ua.Contains("windows nt 6.2"))
                return "Windows 8";
            if (ua.Contains("windows nt 6.1"))
                return "Windows 7";
            if (ua.Contains("windows"))
                return "Windows";
            if (ua.Contains("mac os x") || ua.Contains("macos"))
                return "macOS";
            if (ua.Contains("iphone os") || ua.Contains("ios"))
                return "iOS";
            if (ua.Contains("android"))
                return "Android";
            if (ua.Contains("linux"))
                return "Linux";
            if (ua.Contains("ubuntu"))
                return "Ubuntu";
            
            return "Unknown";
        }

        #endregion
    }
}
