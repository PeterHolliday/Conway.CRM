using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class StageEdit : ComponentBase
    {
        [Inject] protected IStageRepository StageRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }

        [Parameter] public Guid? StageId { get; set; }

        protected Stage Stage = new Stage();

        IEnumerable<Stage> StageList { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (StageId.HasValue)
            {
                Stage = await StageRepository.GetStageByIdAsync(StageId.Value);
            }

            await LoadDataAsync();
        }

        protected async Task LoadDataAsync()
        {
            StageList = await StageRepository.GetAllStagesAsync();
        }

        protected async Task OnSubmit()
        {
            if (StageId.HasValue)
            {
                await StageRepository.UpdateStageAsync(Stage);
            }
            else
            {
                bool stageExists = await StageRepository.IsStageOrderUniqueAsync(Stage.Order);
                if (!stageExists)
                {
                    await StageRepository.AddStageAsync(Stage);
                }
                else
                {
                    var result = await DialogService.Alert($"Stage order {Stage.Order} already exists!", $"Can't add stage with order {Stage.Order}");
                    DialogService.Close();
                }
            }

            NavigationManager.NavigateTo("/stages");
        }
    }
}
