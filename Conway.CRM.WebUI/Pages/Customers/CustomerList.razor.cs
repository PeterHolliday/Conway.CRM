using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.Customers
{
    public partial class CustomerList : ComponentBase
    {
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }

        protected RadzenDataGrid<Customer> gridCustomers;
        protected RadzenDataGrid<Contact> gridContacts;

        protected List<Customer> customers;

        protected Customer SelectedCustomer;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            customers = (await CustomerRepository.GetAllCustomersWithContactsAsync()).ToList();
        }

        protected void AddCustomer()
        {
            NavigationManager.NavigateTo("/customers/add");
        }

        protected void EditCustomer(Guid customerId)
        {
            NavigationManager.NavigateTo($"/customers/edit/{customerId}");
        }

        protected async Task DeleteCustomer(Guid customerId)
        {
            var result = await DialogService.Confirm(
                "Are you sure you want to delete this customer?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (result.HasValue && result.Value)
            {
                await CustomerRepository.DeleteCustomerAsync(customerId);
                await LoadDataAsync();
                await gridCustomers.Reload();
            }
        }

        private async Task OnCustomerRowSelect(Customer customer)
        {

        }
    }
}
