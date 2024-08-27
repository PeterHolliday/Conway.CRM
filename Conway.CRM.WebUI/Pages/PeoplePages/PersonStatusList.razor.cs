using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.PeoplePages
{
    public partial class PersonStatusList : ComponentBase
    {
        [Inject] protected IPersonStatusRepository PersonStatusRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected RadzenDataGrid<PersonStatus> grid;
        protected List<PersonStatus> PersonStatuses;

        protected override async Task OnInitializedAsync()
        {
            PersonStatuses = (await PersonStatusRepository.GetAllPersonStatusesAsync()).ToList();
        }

        protected void AddPersonStatus()
        {
            NavigationManager.NavigateTo("/personstatuses/add");
        }

        protected void EditPersonStatus(Guid personStatusId)
        {
            NavigationManager.NavigateTo($"/personstatuses/edit/{personStatusId}");
        }

        protected async Task DeletePersonStatus(Guid personStatusId)
        {
            await PersonStatusRepository.DeletePersonStatusAsync(personStatusId);
            PersonStatuses = (await PersonStatusRepository.GetAllPersonStatusesAsync()).ToList();
            await grid.Reload();
        }
    }
}
