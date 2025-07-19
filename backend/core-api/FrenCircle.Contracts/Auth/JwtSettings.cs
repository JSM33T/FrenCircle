namespace FrenCircle.Contracts.Auth
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; } = 30;
        public int RefreshTokenExpirationDays { get; set; } = 30;
    }
}
