namespace FrenCircle.Shared.ConfigModels
{
    public class FcConfig
    {
        public SqlConfig? SqlConfig { get; set; }
        public MongoConfig? MongoConfig { get; set; }
    }

    public class SqlConfig
    {
        public string? ConnectionString { get; set; }
    }

    public class MongoConfig
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
