namespace FrenCircle.Helpers.SqlQueries
{
    public static class DbUsers
    {
        public const string Add = @"
        INSERT INTO Users (FirstName, LastName, UserName, Email, Bio, PasswordHash, Salt, TimeSpent, DateUpdated, LastSeen, DateAdded,OTP,OTPTimestamp)
        VALUES (@FirstName, @LastName, @UserName, @Email, @Bio, @PasswordHash, @Salt, @TimeSpent, @DateUpdated, @LastSeen, @DateAdded,@otp, @otpTimestamp);";

        public const string Login = @"
        SELECT * FROM Users WITH(NOLOCK)
        WHERE UserName = @Username;";

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
        WHERE Email = @Email;";
    }
}
