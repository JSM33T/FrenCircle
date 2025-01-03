using API.Entities.Dedicated.Contact;
using FluentValidation;

namespace API.Validators
{
    public class ContactRequestValidator : AbstractValidator<ContactRequest>
    {
        public ContactRequestValidator()
        {
            RuleFor(m => m.Message)
                .NotEmpty()
                .WithMessage("Message is required.");

            RuleFor(m => m.Name)
               .NotEmpty()
               .WithMessage("Name is required.")
               .Length(1, 255)
               .WithMessage("First name must be between 1 and 256 characters.");
        }
    }
}
