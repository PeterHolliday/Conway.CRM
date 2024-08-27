using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.PeoplePages
{
    public partial class PeopleList : ComponentBase
    {
        [Inject] protected IPersonRepository PersonRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected RadzenDataGrid<Person> gridPeople;
        protected List<Person> People;

        protected override async Task OnInitializedAsync()
        {
            People = (await PersonRepository.GetAllPersonsAsync()).ToList();
        }

        protected void AddPerson()
        {
            NavigationManager.NavigateTo("/people/add");
        }

        protected void EditPerson(Guid personId)
        {
            NavigationManager.NavigateTo($"/people/edit/{personId}");
        }

        protected async Task DeletePerson(Guid personId)
        {
            await PersonRepository.DeletePersonAsync(personId);
            People = (await PersonRepository.GetAllPersonsAsync()).ToList();
            await gridPeople.Reload();
        }

    }
}
