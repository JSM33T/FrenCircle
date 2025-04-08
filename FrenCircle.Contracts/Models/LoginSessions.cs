namespace FrenCircle.Contracts.Models
{
    public class LoginSession
    {
        public int Id { get; set; }
        public int UserLoginId { get; set; }
        public string AccessToken { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime? LoggedOutAt { get; set; }
        public Guid DeviceId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public Guid RowId { get; set; }
    }

}
