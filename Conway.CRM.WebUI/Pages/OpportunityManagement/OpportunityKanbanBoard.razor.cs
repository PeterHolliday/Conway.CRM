using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class OpportunityKanbanBoard : ComponentBase
    {
        [Inject] protected IOpportunityRepository OpportunityRepository { get; set; }
        [Inject] protected IStageRepository StageRepository { get; set; }
        [Inject] NotificationService NotificationService { get; set; }

        protected List<Opportunity> Opportunities = new List<Opportunity>();
        protected List<Stage> Stages = new List<Stage>();

        //Func<Opportunity, RadzenDropZone<Opportunity>, bool> ItemSelector = (item, zone) => ((Guid)zone.Value == Guid.Empty && !item.Matched) || (item.Matched && item.MatchedZone == (Guid)zone.Value);
        Func<Opportunity, RadzenDropZone<Opportunity>, bool> ItemSelector = (item, zone) => item.StageId == (Guid)zone.Value;

        protected override async Task OnInitializedAsync()
        {
            Opportunities = (await OpportunityRepository.GetAllOpportunitiesWithCustomersAsync()).ToList();
            Stages = (await StageRepository.GetAllStagesAsync()).OrderBy(s => s.Order).ToList();
        }

        protected async Task OnDrop(RadzenDropZoneItemEventArgs<Opportunity> args)
        {
            if (args.FromZone != args.ToZone)
            {
                try
                {
                    // Update the opportunity's stage
                    await OpportunityRepository.UpdateOpportunityStatusAsync(args.Item.Id, (Guid)args.ToZone.Value);
                    // Refresh the opportunities list
                    Opportunities = (await OpportunityRepository.GetAllOpportunitiesAsync()).ToList();
                    StateHasChanged();
                }
                catch (InvalidOperationException ex)
                {
                    // Handle business rule violation, e.g., moving backward in the workflow
                    NotificationService.Notify(new NotificationMessage { Summary="Move Failed", Detail="Cannot move an opportunity backwards.", Severity = NotificationSeverity.Error});
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                }
            }
        }
    }
}
