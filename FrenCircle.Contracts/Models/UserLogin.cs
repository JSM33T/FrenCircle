namespace FrenCircle.Contracts.Models
{
    public class UserLogin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProviderId { get; set; }
        public string? ProviderUserId { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Salt { get; set; }
        public bool IsPasswordLogin { get; set; }
        public DateTime LinkedAt { get; set; }
        public Guid RowId { get; set; }
    }
}
