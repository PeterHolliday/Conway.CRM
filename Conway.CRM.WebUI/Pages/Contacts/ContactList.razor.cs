using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.Contacts
{
    public partial class ContactList : ComponentBase
    {
        [Inject] protected IContactRepository ContactRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected RadzenDataGrid<Contact> grid;
        protected List<Contact> Contacts;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            Contacts = (await ContactRepository.GetAllContactsAsync()).ToList();
        }

        protected void AddContact()
        {
            NavigationManager.NavigateTo("/contacts/add");
        }

        protected void EditContact(Guid contactId)
        {
            NavigationManager.NavigateTo($"/contacts/edit/{contactId}");
        }

        protected async Task DeleteContact(Guid contactId)
        {
            await ContactRepository.DeleteContactAsync(contactId);
            Contacts = (await ContactRepository.GetAllContactsAsync()).ToList();
            await grid.Reload();
        }
    }
}
