using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class OpportunityList : ComponentBase

    {
        [Inject] protected IOpportunityRepository OpportunityRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }

        protected RadzenDataGrid<Opportunity> grid;
        protected List<Opportunity> Opportunities;

        protected override async Task OnInitializedAsync()
        {
            Opportunities = (await OpportunityRepository.GetAllOpportunitiesWithCustomersAsync()).ToList();
        }

        protected void AddOpportunity()
        {
            NavigationManager.NavigateTo("/opportunities/add");
        }

        protected void EditOpportunity(Guid opportunityId)
        {
            NavigationManager.NavigateTo($"/opportunities/edit/{opportunityId}");
        }

        protected async Task DeleteOpportunity(Guid opportunityId)
        {
            var result = await DialogService.Confirm(
                "Are you sure you want to delete this Opportunity?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (result.HasValue && result.Value)
            {
                await OpportunityRepository.DeleteOpportunityAsync(opportunityId);
                Opportunities = (await OpportunityRepository.GetOpportunitiesByCustomerIdAsync(Guid.Empty)).ToList();
                await grid.Reload();
            }
        }
    }
}
