using FluentValidation;

namespace FrenCircle.Entities.Fren
{
    public class FrenSignUpRequest
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class FrenSignUpRequestValidator : AbstractValidator<FrenSignUpRequest>
    {
        public FrenSignUpRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters.");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords don't match.");
        }
    }
}
