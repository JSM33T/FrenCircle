namespace FrenCircle.Helpers.SqlQueries
{
    public static class DbUsers
    {
        //public const string Add = @"
        //INSERT INTO Users 
        //(FirstName, LastName, UserName, Email, Bio, PasswordHash, Salt, TimeSpent, DateUpdated, LastSeen, DateAdded,OTP,OTPTimestamp)
        //VALUES 
        //(@FirstName, @LastName, @UserName, @Email, @Bio, @PasswordHash, @Salt, @TimeSpent, @DateUpdated, @LastSeen, @DateAdded,@otp, @otpTimestamp);";

        public const string Add = @"
        INSERT INTO Users 
        (Id, FirstName, LastName, UserName, Email, Bio, PasswordHash, Salt, TimeSpent, DateUpdated, LastSeen, DateAdded, OTP, OTPTimestamp)
        VALUES 
        ((SELECT COALESCE(MAX(Id), 0) + 1 FROM Users), @FirstName, @LastName, @UserName, @Email, @Bio, @PasswordHash, @Salt, @TimeSpent, @DateUpdated, @LastSeen, @DateAdded, @otp, @otpTimestamp);";


        public const string Login = @"
        SELECT * FROM Users
        WHERE UserName = @Username OR Email = @Username;";

        public const string GetAll = @"
        SELECT * FROM Users WITH(NOLOCK);";

        public const string CheckByUsername = @"
        SELECT * FROM Users WITH(NOLOCK)
        WHERE UserName = @Username;";

        public const string CheckByEmail = @"
        SELECT * FROM Users WITH(NOLOCK)
        WHERE Email = @Email;";
        
        public const string Verify = @"
        SELECT * FROM Users WITH(NOLOCK)
        WHERE Email = @Email OR UserName = @Username;";
        
        public const string GetByEmail = @"
        SELECT * FROM Users WITH(NOLOCK)
        WHERE Email = @Email;";

        public const string GetById = @"
        SELECT * FROM Users WITH(NOLOCK)
        WHERE Id = @Id;";

        public static string GetOtp = @"
        SELECT OTP, OTPTimeStamp FROM Users 
        WHERE Email = @Email";

        public static string GenerateOtp = @"
        UPDATE Users SET OTP = @OTP, OTPTimeStamp = @OTPDate 
        WHERE Email = @Email";

        public static string VerifyOtp = @"
        UPDATE Users SET OTP = NULL, OTPTimeStamp = NULL,IsActive = 1 
        WHERE Email = @Email";

        // 🔹 Store Refresh Token
        public const string StoreRefreshToken = @"
INSERT INTO RefreshTokens (UserId, Token, ExpiresAt, CreatedAt) 
VALUES (@UserId, @RefreshToken, @ExpiryDate, GETUTCDATE());";

        // 🔹 Get Refresh Token
        public const string GetRefreshToken = @"
SELECT * FROM RefreshTokens 
WHERE Token = @RefreshToken AND ExpiresAt > GETUTCDATE();";

        // 🔹 Update Refresh Token (Rotating Tokens)
        public const string UpdateRefreshToken = @"
UPDATE RefreshTokens 
SET Token = @NewRefreshToken, ExpiresAt = @ExpiryDate, CreatedAt = GETUTCDATE() 
WHERE UserId = @UserId AND Token = @OldRefreshToken;";

    }
}
