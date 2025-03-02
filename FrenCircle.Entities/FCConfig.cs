﻿namespace FrenCircle.Entities
{
    public class FcConfig
    {
        public required string ConnectionString { get; set; }
        public required string SqliteConnection { get; set; }
        public CryptographySettings Cryptography { get; set; }
        public JwtSettings? JwtSettings { get; set; }
        public SmtpSettings SmtpSettings { get; set; }
        public Paths Paths { get; set; }
        public Logins logins { get; set; }
        public Toggles Toggles { get; set; }
        public TelegramSettings telegramSettings { get; set; }
        public MongoDb mongoDb { get; set; }
        public List<Url> Urls { get; set; }
    }

    public class MongoDb
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
    public class Url
    {
        public string ApiBaseUrl { get; set; }
    }

    public class Toggles
    {
    }

    public class Logins
    {
        public string GoogleClientId { get; set; } = string.Empty;
    }
    public class CryptographySettings
    {
        public string Key { get; set; } = string.Empty;
        public string IV { get; set; } = string.Empty;
    }

    public class JwtSettings
    {
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public string IssuerSigningKey { get; set; } = string.Empty;
    }

    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public bool EnableSSL { get; set; }
    }

    public class Paths
    {
        public string CDNURL { get; set; }
    }
    public class TelegramSettings
    {
        public string BotToken { get; set; }
        public string ChatId { get; set; }
        public string ApiUrl { get; set; }
    }

}
