using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Conway.CRM.WebUI.Pages.Customers
{
    public partial class CustomerEdit : ComponentBase
    {
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }

        [Parameter] public Guid? CustomerId { get; set; }
        protected Customer Customer = new Customer();

        private Dictionary<string, string> validationErrors = new();

        protected override async Task OnInitializedAsync()
        {
            if (CustomerId.HasValue)
            {
                Customer = await CustomerRepository.GetCustomerByIdAsync(CustomerId.Value);
            }
        }

        protected async Task OnSubmit()
        {
            validationErrors.Clear();
            var result = await Validator.ValidateAsync(Customer);

            if (result.IsValid)
            {
                if (CustomerId.HasValue)
                {
                    await CustomerRepository.UpdateCustomerAsync(Customer);
                }
                else
                {
                    await CustomerRepository.AddCustomerAsync(Customer);
                }
                NavigationManager.NavigateTo("/customers");
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

        protected async Task CancelAdd()
        {
            NavigationManager.NavigateTo("/customers");
        }
    }
}
