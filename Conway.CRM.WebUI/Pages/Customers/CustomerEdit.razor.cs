using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Conway.CRM.WebUI.Pages.Customers
{
    public partial class CustomerEdit : ComponentBase
    {
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid? CustomerId { get; set; }
        protected Customer Customer = new Customer();

        protected override async Task OnInitializedAsync()
        {
            if (CustomerId.HasValue)
            {
                Customer = await CustomerRepository.GetCustomerByIdAsync(CustomerId.Value);
            }
        }

        protected async Task OnSubmit()
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
    }
}
