namespace FrenCircle.Helpers.SqlQueries
{
    /// <summary>
    /// Contains SQL queries for login operations.
    /// </summary>
    public static class DbLogin
    {
        /// <summary>
        /// Query to get login information by DeviceId.
        /// </summary>
        public const string GetLoginByDeviceId = @"
            SELECT 
                Id,
                UserId,
                UserAgent,
                DeviceId,
                Latitude,
                Longitude,
                IpAddress,
                LoginMethod,
                IsLoggedIn,
                DateAdded
            FROM Logins
            WHERE DeviceId = @DeviceId";

        /// <summary>
        /// Query to add a new login entry.
        /// </summary>
        public const string AddLoginEntry = @"
            INSERT INTO Logins (
                UserId,
                UserAgent,
                DeviceId,
                Latitude,
                Longitude,
                IpAddress,
                LoginMethod,
                IsLoggedIn
            ) VALUES (
                @UserId,
                @UserAgent,
                @DeviceId,
                @Latitude,
                @Longitude,
                @IpAddress,
                @LoginMethod,
                @IsLoggedIn
            );
            SELECT SCOPE_IDENTITY();";

        /// <summary>
        /// Query to extend the expiry of a login entry.
        /// </summary>
        public const string ExtendExpiry = @"
            UPDATE Logins
            SET DateAdded = GETDATE()
            WHERE DeviceId = @DeviceId";
    }
}
