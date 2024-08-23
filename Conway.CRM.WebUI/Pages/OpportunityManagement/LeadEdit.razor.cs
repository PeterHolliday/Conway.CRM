using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class LeadEdit : ComponentBase
    {
        [Inject] protected ILeadRepository LeadRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid? LeadId { get; set; }
        protected Lead Lead = new Lead();

        protected override async Task OnInitializedAsync()
        {
            if (LeadId.HasValue)
            {
                Lead = await LeadRepository.GetLeadByIdAsync(LeadId.Value);
            }
        }

        protected async Task OnSubmit()
        {
            if (LeadId.HasValue)
            {
                await LeadRepository.UpdateLeadAsync(Lead);
            }
            else
            {
                await LeadRepository.AddLeadAsync(Lead);
            }

            NavigationManager.NavigateTo("/leads");
        }
    }
}
