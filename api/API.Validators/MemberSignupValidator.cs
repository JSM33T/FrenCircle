using API.Entities.Dedicated;
using FluentValidation;

namespace API.Validators
{
    public class MemberSignupValidator : AbstractValidator<MemberSignup>
    {
        public MemberSignupValidator()
        {
            RuleFor(m => m.FirstName)
                .NotEmpty()
                .WithMessage("First name is required.")
                .Length(1, 50)
                .WithMessage("First name must be between 1 and 50 characters.");

            RuleFor(m => m.LastName)
                .NotEmpty()
                .WithMessage("Last name is required.")
                .Length(1, 50)
                .WithMessage("Last name must be between 1 and 50 characters.");

            RuleFor(m => m.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email format is invalid.");

            RuleFor(m => m.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .Length(1, 50)
                .WithMessage("Username must be between 1 and 50 characters.");
        }
    }
}
