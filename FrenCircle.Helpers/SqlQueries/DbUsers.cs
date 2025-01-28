namespace FrenCircle.Helpers.SqlQueries
{
    public static class DbUsers
    {
        public const string Add = @"
        INSERT INTO Users (FirstName, LastName, UserName, Email, Bio, PasswordHash, Salt, TimeSpent, DateUpdated, LastSeen, DateAdded)
        VALUES (@FirstName, @LastName, @UserName, @Email, @Bio, @PasswordHash, @Salt, @TimeSpent, @DateUpdated, @LastSeen, @DateAdded);";

        public const string Login = @"
        SELECT * FROM Users 
        WHERE UserName = @Username;";

        public const string GetAll = @"
        SELECT * FROM Users;";

        public const string CheckByUsername = @"
        SELECT * FROM Users 
        WHERE UserName = @Username;";

        public const string CheckByEmail = @"
        SELECT * FROM Users 
        WHERE Email = @Email;";
    }
}
