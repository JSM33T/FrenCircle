namespace FrenCircle.Contracts.Models
{
    public class LoginProvider
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Guid RowId { get; set; }
    }
}
