using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Conway.CRM.WebUI.Pages.PeoplePages
{
    public partial class PersonEdit : ComponentBase
    {
        [Inject] protected IPersonRepository PersonRepository { get; set; }
        [Inject] protected IPersonStatusRepository PersonStatusRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid? PersonId { get; set; }

        protected Person Person = new Person();

        protected List<PersonStatus> Statuses = new List<PersonStatus>();
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        protected async Task LoadDataAsync()
        {
            Statuses = (await PersonStatusRepository.GetAllPersonStatusesAsync()).ToList();

            if (PersonId.HasValue)
            {
                Person = await PersonRepository.GetPersonByIdAsync(PersonId.Value);
            }
        }

            protected async Task OnSubmit()
        {
            if (PersonId.HasValue)
            {
                await PersonRepository.UpdatePersonAsync(Person);
            }
            else
            {
                Person.Id = Guid.NewGuid();
                await PersonRepository.AddPersonAsync(Person);
            }

            NavigationManager.NavigateTo("/people");
        }
    }
}
