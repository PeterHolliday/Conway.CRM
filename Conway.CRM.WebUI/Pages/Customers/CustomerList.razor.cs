using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.Customers
{
    public partial class CustomerList : ComponentBase
    {
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected RadzenDataGrid<Customer> grid;
        protected List<Customer> Customers;

        protected override async Task OnInitializedAsync()
        {
            Customers = (await CustomerRepository.GetAllCustomersAsync()).ToList();
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
            await CustomerRepository.DeleteCustomerAsync(customerId);
            Customers = (await CustomerRepository.GetAllCustomersAsync()).ToList();
            await grid.Reload();
        }
    }
}
