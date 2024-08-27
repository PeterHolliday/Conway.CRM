using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Domain.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class OpportunityEdit : ComponentBase
    {
        [Inject] protected IOpportunityRepository OpportunityRepository { get; set; }
        [Inject] protected IPersonRepository PersonRepository { get; set; }
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected IStageRepository StageRepository { get; set; }

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }

        [Parameter] public Guid? OpportunityId { get; set; }
        protected Opportunity Opportunity = new Opportunity();
        protected List<Customer> Customers = new List<Customer>();
        protected List<Stage> Stages = new List<Stage>();
        protected List<Person> People = new List<Person>();

        private Dictionary<string, string> validationErrors = new();

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        protected async Task LoadDataAsync()
        {
            Customers = (await CustomerRepository.GetAllCustomersAsync()).OrderBy(c => c.CompanyName).ToList();
            Stages = (await StageRepository.GetAllStagesAsync()).OrderBy(s => s.Order).ToList();

            if (OpportunityId.HasValue)
            {
                Opportunity = await OpportunityRepository.GetOpportunityByIdAsync(OpportunityId.Value);
            }

            People = (await PersonRepository.GetAllPersonsAsync()).OrderBy(a => a.FullName).ToList();

            Opportunity.StageId = Stages.First().Id;
        }

        protected async Task OnSubmit()
        {
            validationErrors.Clear();
            var result = await Validator.ValidateAsync(Opportunity);

            if (result.IsValid)
            {
                if (OpportunityId.HasValue)
                {
                    await OpportunityRepository.UpdateOpportunityAsync(Opportunity);
                }
                else
                {
                    await OpportunityRepository.AddOpportunityAsync(Opportunity);
                }

                NavigationManager.NavigateTo("/opportunities");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    validationErrors[error.PropertyName] = error.ErrorMessage;
                    NotificationService.Notify(new NotificationMessage() { Summary = "Validation Error", Detail = error.ErrorMessage, Severity = NotificationSeverity.Error });
                }
            }
        }

        private string GetValidationClass(string propertyName)
        {
            // Returns a CSS class if the property has a validation error
            return validationErrors.ContainsKey(propertyName) ? "field-validation-error" : string.Empty;
        }
    }
}
