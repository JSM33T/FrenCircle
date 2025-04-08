namespace FrenCircle.Contracts.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid RowId { get; set; }
    }

}
