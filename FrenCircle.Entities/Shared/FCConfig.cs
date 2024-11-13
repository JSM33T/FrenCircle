namespace FrenCircle.Entities.Shared
{
    public class FCConfig
    {
        public string ConnectionString { get; set; }
        public CryptographySettings Cryptography { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public SmtpSettings SmtpSettings { get; set; }
        public Logins logins {get;set;}
    }

    public class Logins {
        public string GoogleClientId { get; set; }
    }
    public class CryptographySettings
    {
        public string Key { get; set; }
        public string IV { get; set; }
    }

    public class JwtSettings
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string IssuerSigningKey { get; set; }
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
}
