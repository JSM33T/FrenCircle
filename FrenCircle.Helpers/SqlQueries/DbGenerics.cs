namespace FrenCircle.Helpers.SqlQueries
{
    public static class DbGenerics
    {
        public static string GetSelectQuery => @"
        SELECT * FROM {0} WHERE {1} = {2};";
    }
}
