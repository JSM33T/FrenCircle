namespace FrenCircle.Shared.ConfigModels
{
    public class FcConfig
    {
        public SqlConfig? SqlConfig { get; set; }
        public MongoConfig? MongoConfig { get; set; }
        public Toggles? Toggles { get; set; }
        public JwtConfig? JwtConfig { get; set; }
        public TelegramConfig TeleConfig { get; set; }
    }

    public class Toggles
    {
        public bool IncludeResponseTime { get; set; }
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

    public class JwtConfig {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
    public class TelegramConfig {
        public string BotToken { get; set; }
        public double LogChatId { get; set; }
    }
}
