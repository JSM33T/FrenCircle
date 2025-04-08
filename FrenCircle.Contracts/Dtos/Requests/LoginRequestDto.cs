using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Dtos.Requests
{
    public class LoginRequestDto
    {
        [Required] public string Email { get; set; } = null!;
        [Required] public string Password { get; set; } = null!;

        public string? DeviceId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }


}
