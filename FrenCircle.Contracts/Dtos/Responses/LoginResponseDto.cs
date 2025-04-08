using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Dtos.Responses
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public string AccessToken { get; set; } = null!;
        //public string RefreshToken { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }

}
