using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Conway.CRM.WebUI.Pages.Contacts
{
    public partial class ContactEdit : ComponentBase
    {
        [Inject] protected IContactRepository ContactRepository { get; set; }
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }

        [Parameter] public Guid? ContactId { get; set; }
        protected Contact Contact = new Contact();
        List<Customer> CustomerList { get; set; }

        private Dictionary<string, string> validationErrors = new();

        protected override async Task OnInitializedAsync()
        {
            if (ContactId.HasValue)
            {
                Contact = await ContactRepository.GetContactByIdAsync(ContactId.Value);
            }

            await LoadDataAsync();
        }

        protected async Task LoadDataAsync() 
        {
            CustomerList = (await CustomerRepository.GetAllCustomersAsync()).OrderBy(c => c.CompanyName).ToList();
        }

        protected async Task OnSubmit()
        {
            validationErrors.Clear();
            var result = await Validator.ValidateAsync(Contact);

            if (result.IsValid)
            {

                if (ContactId.HasValue)
                {
                    await ContactRepository.UpdateContactAsync(Contact);
                }
                else
                {
                    await ContactRepository.AddContactAsync(Contact);
                }

                NavigationManager.NavigateTo("/contacts");
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
            NavigationManager.NavigateTo("/contacts");
        }
    }
}
