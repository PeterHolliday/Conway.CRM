using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class LeadList : ComponentBase
    {
        [Inject] protected ILeadRepository LeadRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }

        protected RadzenDataGrid<Lead> gridLeads;
        protected List<Lead> leads;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            leads = (await LeadRepository.GetAllLeadsAsync()).ToList();
        }

        protected void AddLead()
        {
            NavigationManager.NavigateTo("/leads/add");
        }

        protected void EditLead(Guid leadId)
        {
            NavigationManager.NavigateTo($"/leads/edit/{leadId}");
        }

        protected async Task DeleteLead(Guid leadId)
        {
            var result = await DialogService.Confirm(
                "Are you sure you want to delete this Lead?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (result.HasValue && result.Value)
            {
                await LeadRepository.DeleteLeadAsync(leadId);
                await LoadDataAsync();
                await gridLeads.Reload();
            }
        }
    }
}
