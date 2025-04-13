using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Dtos.Internal
{
    public class SignupResultDto
    {
        public int UserId { get; set; }
        public Guid EmailVerificationToken { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

}
