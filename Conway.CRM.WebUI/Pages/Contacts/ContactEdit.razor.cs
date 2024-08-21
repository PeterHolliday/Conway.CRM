using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Conway.CRM.WebUI.Pages.Contacts
{
    public partial class ContactEdit : ComponentBase
    {
        [Inject] protected IContactRepository ContactRepository { get; set; }
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid? ContactId { get; set; }
        protected Contact Contact = new Contact();
        IEnumerable<Customer> CustomerList { get; set; }

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
            CustomerList = await CustomerRepository.GetAllCustomersAsync();
        }

        protected async Task OnSubmit()
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
    }
}
