using Conway.CRM.Domain.Entities;
using FluentValidation;

namespace Conway.CRM.Domain.Validations
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator() 
        {
            RuleFor(customer => customer.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .Length(3, 50).WithMessage("Company name must be between 3 and 50 characters");

            RuleFor(customer => customer.Address1)
                .NotEmpty().WithMessage("The first line of the address is required")
                .Length(3, 50).WithMessage("Address line 1 must be between 3 and 50 characters");

            RuleFor(customer => customer.Postcode)
                .NotEmpty().WithMessage("Postcode is required");

            RuleFor(customer => customer.InvoiceAccountNo)
                .NotEmpty().WithMessage("Invoice account number is required")
                .InclusiveBetween(0, 999999)
            .WithMessage("The invoice account number must be 6 digits or less.");     
        }
    }
}
