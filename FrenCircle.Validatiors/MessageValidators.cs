using FluentValidation;
using FrenCircle.Entities.Data;

namespace FrenCircle.Validators
{
    public class AddMessageRequestValidators : AbstractValidator<AddMessageRequest>
    {
        public AddMessageRequestValidators()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Message is required.")
                .MinimumLength(3).WithMessage("Message too short");
        }
    }
}
