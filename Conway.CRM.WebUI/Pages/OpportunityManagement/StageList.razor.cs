using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class StageList : ComponentBase
    {
        [Inject] protected IStageRepository StageRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }

        protected RadzenDataGrid<Stage> gridStages;

        protected List<Stage> stages;
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            stages =(await StageRepository.GetAllStagesAsync()).ToList();
        }

        protected void AddStage()
        {
            NavigationManager.NavigateTo("/stages/add");
        }

        protected void EditStage(Guid stageId)
        {
            NavigationManager.NavigateTo($"/stages/edit/{stageId}");
        }

        protected async Task DeleteStage(Guid stageId)
        {
            var result = await DialogService.Confirm(
                "Are you sure you want to delete this customer?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (result.HasValue && result.Value)
            {
                await StageRepository.DeleteStageAsync(stageId);
                stages = (await StageRepository.GetAllStagesAsync()).OrderBy(s => s.Order).ToList();
                await gridStages.Reload();
            }
        }
    }
}
