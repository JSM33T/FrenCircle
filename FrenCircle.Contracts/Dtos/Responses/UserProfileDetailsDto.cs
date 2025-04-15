using Microsoft.AspNetCore.Http;

namespace FrenCircle.Contracts.Dtos.Responses
{
    public class UserProfileDetailsDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string Bio { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;

    }
    public class EditUserProfileDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Bio { get; set; }
        public string? Gender { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
