using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Entities.Dedicated
{
    public class MemberSignup
    {
        public int Id { get; set; }               // The unique identifier for the member
        public string FirstName { get; set; }      // Member's first name
        public string LastName { get; set; }       // Member's last name
        public string Username { get; set; }       // Member's username (unique)
        public string Email { get; set; }          // Member's email address (unique)
    }
}
