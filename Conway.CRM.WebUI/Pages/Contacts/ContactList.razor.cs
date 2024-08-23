using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.Contacts
{
    public partial class ContactList : ComponentBase
    {
        [Inject] protected IContactRepository ContactRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }

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
            var result = await DialogService.Confirm(
                "Are you sure you want to delete this Contact?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (result.HasValue && result.Value)
            {
                await ContactRepository.DeleteContactAsync(contactId);
                await LoadDataAsync();
                await grid.Reload();
            }
        }
    }
}
