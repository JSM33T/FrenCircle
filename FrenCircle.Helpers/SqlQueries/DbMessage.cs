namespace FrenCircle.Helpers.SqlQueries
{
    public static class DbMessage
    {
        public static string Add => @"
        INSERT INTO Messages (Name, Email, Text, DateAdded)
        VALUES (@Name, @Email, @Text, GETDATE());
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

        public static string Getall => @"
        SELECT * FROM Messages";

        public static string CheckByEmail => @"
        SELECT TOP 1 * FROM Messages WHERE Email = @Email AND Text = @Text";
    }
}
