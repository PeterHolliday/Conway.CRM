using Conway.CRM.Domain.Entities;
using FluentValidation;

namespace Conway.CRM.Domain.Validations
{
    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator() 
        {
            RuleFor(contact => contact.Email)
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(contact => contact.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Matches("^[a-zA-Z]+$").WithMessage("First name must only contain alphabetic characters")
                .Length(2, 30).WithMessage("First name can only be between 2 and 30 characters");

            RuleFor(contact => contact.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Matches("^[a-zA-Z]+$").WithMessage("Last name must only contain alphabetic characters")
                .Length(2, 30).WithMessage("Last name can only be between 2 and 30 characters");

            RuleFor(contact => contact.CustomerId)
                .Must(guid => guid != Guid.Empty)
                .WithMessage("Customer is required");
        }
    }
}
