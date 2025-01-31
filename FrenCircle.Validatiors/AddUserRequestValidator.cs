using FluentValidation;
using FrenCircle.Entities.Data;

namespace FrenCircle.Validators
{
    public class AddUserRequestValidators : AbstractValidator<AddUserRequest>
    {
        public AddUserRequestValidators()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Firstname is required.");
            
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username too short");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

        }
    }
}
