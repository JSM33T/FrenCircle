using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Dtos.Responses
{
    public class UserProfileDetailsDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string Bio { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Gender { get; set; } = string.Empty;

    }
}
