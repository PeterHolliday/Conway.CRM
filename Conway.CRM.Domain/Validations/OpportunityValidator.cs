using Conway.CRM.Domain.Entities;
using FluentValidation;

namespace Conway.CRM.Domain.Validations
{
    public class OpportunityValidator : AbstractValidator<Opportunity>
    {
        public OpportunityValidator() 
        {
            RuleFor(opportunity => opportunity.AccountManagerId)
                .Must(guid => guid != Guid.Empty)
                .WithMessage("Account Manager is required");

            RuleFor(opportunity => opportunity.CustomerId)
                .Must(guid => guid != Guid.Empty)
                .WithMessage("Customer is required");

            RuleFor(opportunity => opportunity)
                .Must(HaveAtLeastOneVolume)
                .WithMessage("Either Aggregates Volume or Ashphalt Volume must be greater than zero");

            RuleFor(opportunity => opportunity.AggregatesVolume)
                .GreaterThan(0)
                .When(opportunity => opportunity.AsphaltVolume <= 0)
                .WithMessage("Either Aggregates Volume or Ashphalt Volume must be greater than zero");

            RuleFor(opportunity => opportunity.AsphaltVolume)
                .GreaterThan(0)
                .When(opportunity => opportunity.AggregatesVolume <= 0)
                .WithMessage("Either Aggregates Volume or Ashphalt Volume must be greater than zero");

            RuleFor(opportunity => opportunity.Comments)
                .NotEmpty().WithMessage("Comments are required");

            RuleFor(opportunity => opportunity.Site)
                .NotEmpty().WithMessage("Site is required");
        }

        private bool HaveAtLeastOneVolume(Opportunity opportunity)
        {
            return !(opportunity.AggregatesVolume > 0) || !(opportunity.AsphaltVolume > 0);
        }
    }
}
