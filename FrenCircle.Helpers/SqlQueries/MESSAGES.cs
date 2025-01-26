namespace FrenCircle.Helpers.SqlQueries
{
    public static class DB_MESSAGE
    {
        public static string ADD => @"
        INSERT INTO Messages (Name, Email, Text, DateAdded)
        VALUES (@Name, @Email, @Text, GETDATE());
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public static string GETALL => @"
        SELECT * FROM Messages";

        public static string CHECK_BY_EMAIL => @"
        SELECT TOP 1 * FROM Messages WHERE Email = @Email AND Text = @Text";
    }
}
