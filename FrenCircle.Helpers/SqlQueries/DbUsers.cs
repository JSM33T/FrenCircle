namespace FrenCircle.Helpers.SqlQueries
{
    public static class DbUsers
    {
        public const string Add = @"
        INSERT INTO Users 
        (FirstName, LastName, UserName, Email, Bio, PasswordHash, Salt, TimeSpent, DateUpdated, LastSeen, DateAdded, OTP, OTPTimestamp)
        VALUES 
        (@FirstName, @LastName, @UserName, @Email, @Bio, @PasswordHash, @Salt, @TimeSpent, @DateUpdated, @LastSeen, @DateAdded, @otp, @otpTimestamp);";

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

        //// 🔹 Store Refresh Token
        //public const string StoreRefreshToken = @"
        //INSERT INTO RefreshTokens (DeviceId,UserId, Token, ExpiresAt, CreatedAt) 
        //VALUES (@DeviceId,@UserId, @RefreshToken, @ExpiryDate, GETUTCDATE());";

        public const string StoreRefreshToken = @"
            MERGE INTO RefreshTokens AS target
            USING (SELECT @DeviceId AS DeviceId, @UserId AS UserId) AS source
            ON target.DeviceId = source.DeviceId AND target.UserId = source.UserId
            WHEN MATCHED THEN
                UPDATE SET ExpiresAt = @ExpiryDate, Token = @RefreshToken
            WHEN NOT MATCHED THEN
                INSERT (DeviceId, UserId, Token, ExpiresAt, CreatedAt)
                VALUES (@DeviceId, @UserId, @RefreshToken, @ExpiryDate, GETUTCDATE());";

        // 🔹 Get Refresh Token
        public const string GetRefreshToken = @"
        SELECT * FROM RefreshTokens 
        WHERE Token = @RefreshToken AND ExpiresAt > GETUTCDATE();";

        // 🔹 Update Refresh Token (Rotating Tokens)
        public const string UpdateRefreshToken = @"
        UPDATE RefreshTokens 
        SET Token = @NewRefreshToken, ExpiresAt = @ExpiryDate, CreatedAt = GETUTCDATE() 
        WHERE UserId = @UserId AND Token = @OldRefreshToken;";

        public static string EditProfile => @"
        UPDATE Users 
        SET 
            FirstName = @FirstName,
            LastName = @LastName,
            Bio = @Bio,
            UserName = @UserName,
            DateUpdated = @DateUpdated
        WHERE 
            Id = @UserId;
        ";

        public static string GetProfile => @"
        SELECT * FROM Users 
        WHERE Id = @UserId;
        ";

        public static string GetLoginInfo => @"
        SELECT * FROM Logins
        WHERE UserId = 4
        ";

    }
}
