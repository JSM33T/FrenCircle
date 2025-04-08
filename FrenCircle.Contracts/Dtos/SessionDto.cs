using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Dtos
{
    public class SessionDto
    {
        public int SessionId { get; set; }
        public bool IsActive { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime? LoggedOutAt { get; set; }
        public string? DeviceFingerprint { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }

}
