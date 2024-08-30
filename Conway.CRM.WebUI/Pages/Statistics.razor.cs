using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Conway.CRM.WebUI.Pages
{
    public partial class Statistics : ComponentBase
    {
        [Inject] protected IStageRepository StageRepository { get; set; }
        [Inject] protected IOpportunityStatusChangeRepository StageChangeRepository { get; set; }

        private List<Stage> Stages;
        private List<OpportunityStatusChange> StatusChanges;

        private int TotalCount = 0;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();

            var stageCounts = StatusChanges
                .GroupBy(osc => osc.StageId)
                .Select(group => new
                {
                    StageId = group.Key,
                    Count = group.Count()
                })
                .ToList();

            // Now join with the Stages to get the stage names along with the counts
            var result = stageCounts
                .Join(Stages, // Assuming you have a list of stages
                      sc => sc.StageId,
                      stage => stage.Id,
                      (sc, stage) => new
                      {
                          StageName = stage.Name,
                          OpportunityCount = sc.Count,
                          StageOrder = stage.Order,
                      })
                .OrderBy(x => x.StageOrder)
                .ToList();

            // Display the results
            foreach (var item in result)
            {
                Console.WriteLine($"Stage: {item.StageName}, Opportunities: {item.OpportunityCount}");
            }
        }

        protected async Task LoadDataAsync()
        {
            Stages = (await StageRepository.GetAllStagesAsync()).ToList();
            StatusChanges = (await StageChangeRepository.GetAllOpportunityStatusChangesAsync()).ToList();
            TotalCount = StatusChanges .Count;
        }
    }
}
