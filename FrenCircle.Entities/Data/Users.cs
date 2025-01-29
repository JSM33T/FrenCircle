namespace FrenCircle.Entities.Data
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; } = "anonymous";
        public required string  UserName { get; set; }
        public required string Email { get; set; }
        public required string Bio { get; set; } = "";
        public required string PasswordHash { get; set; }
        public string Salt { get; set; }
        
        public int Otp { get; set; }
        public string Role { get; set; } = "USER";
        
        public bool IsActive { get; set; } = false;
        
        public DateTime OtpTimeStamp { get; set; }
        public DateTime TimeSpent { get; set; } = DateTime.Now.AddMinutes(60);
        public DateTime DateUpdated { get; set; } = DateTime.Now;
        public DateTime LastSeen { get; set; } = DateTime.Now;
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }

    public class AddUserRequest
    {
        public required string FirstName { get; set; }
        public string? LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? Bio { get; set; }
        public required string Password { get; set; }
    }

    public class LoginUserRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
    public class VerifyRequest
    {
        public required string Email { get; set; }
        public int Otp { get; set; }
    }
}
