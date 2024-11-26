namespace FrenCircle.Entities.Fren
{
    public class FrenSignUpRequest
    {
        public  required string  Username { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public int OTP { get; set; }
    }
    
}
