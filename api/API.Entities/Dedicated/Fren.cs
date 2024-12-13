using API.Entities.Enums;

namespace API.Entities.Dedicated
{
    public class Fren
    {
        public int Id { get; set; }               // The unique identifier for the member
        public string FirstName { get; set; }      // Member's first name
        public string LastName { get; set; }       // Member's last name
        public string Username { get; set; }       // Member's username (unique)
        public string Email { get; set; }          // Member's email address (unique)
        public string Avatar { get; set; }         // URL to the member's avatar/profile picture
        public string Role { get; set; }           // Role of the member (e.g., Admin, User)
        public string GoogleId { get; set; }       // Google ID for OAuth integration
        public string Key { get; set; }            // Additional key or token for security
        public DateTime OTPDate { get; set; } = DateTime.Now;   // Date when OTP was generated
        public bool IsActive { get; set; }         // Indicates if the member is active
        public bool IsVerified { get; set; }       // Indicates if the email is verified
        public int ExpPoints { get; set; }         // Experience points for the member
        public int TimeSpent { get; set; }         // Total time spent by the member
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public DateTime DateEdited { get; set; } = DateTime.Now;   // Date when the member record was last edited
        public string Bio { get; set; }            // Member's biography or description
        public AuthMode AuthMode { get; set; }       // Method used for authentication (e.g., "Local", "Google")
    }
}