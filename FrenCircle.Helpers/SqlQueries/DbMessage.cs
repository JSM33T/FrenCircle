namespace FrenCircle.Helpers.SqlQueries
{
    public static class DbMessage
    {
        public static string Add => @"
        INSERT INTO Messages (Id,Name, Email, Text, DateAdded)
        VALUES ((SELECT COALESCE(MAX(Id), 0) + 1 FROM Messages),@Name, @Email, @Text, GETDATE());";

        public static string Getall => @"
        SELECT * FROM Messages WITH(NOLOCK)";

        public static string CheckByEmail => @"
        SELECT TOP 1 * FROM Messages WHERE Email = @Email AND Text = @Text";
    }
}
