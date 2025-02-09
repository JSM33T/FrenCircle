using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Entities.Shared
{
    public class RefreshToken
    {
        public int Id { get; set; }           // Unique ID for the refresh token
        public int UserId { get; set; }       // Associated user ID
        public string Token { get; set; } = string.Empty;  // Refresh token string
        public DateTime ExpiresAt { get; set; }  // Expiration time
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Token creation time
    }

}
