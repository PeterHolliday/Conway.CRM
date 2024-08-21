using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.Customers
{
    public partial class CustomerList : ComponentBase
    {
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected IContactRepository ContactRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected RadzenDataGrid<Customer> gridCustomers;
        protected RadzenDataGrid<Contact> gridContacts;

        protected List<Customer> Customers;
        protected List<Contact> Contacts;
        protected List<Contact> CustomerContacts;

        protected Customer SelectedCustomer;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            Customers = (await CustomerRepository.GetAllCustomersAsync()).ToList();
            Contacts = (await ContactRepository.GetAllContactsAsync()).ToList();
            if (SelectedCustomer != null)
            {
                CustomerContacts = (Contacts.Where(c => c.CustomerId == SelectedCustomer.Id)).ToList();
            }
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
            await LoadDataAsync();
            await gridCustomers.Reload();
        }

        private async Task OnCustomerRowSelect(Customer customer)
        {
            SelectedCustomer = customer;
            CustomerContacts = (Contacts.Where(c => c.CustomerId == SelectedCustomer.Id)).ToList();
            //await gridContacts.Reload();
        }
    }
}
