using FluentValidation;
using FrenCircle.Entities.Fren;

namespace FrenCircle.Validators
{

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
