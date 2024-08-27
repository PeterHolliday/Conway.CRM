using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Conway.CRM.WebUI.Pages.PeoplePages
{
    public partial class PersonStatusEdit : ComponentBase
    {

        [Inject] protected IPersonStatusRepository PersonStatusRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid? PersonStatusId { get; set; }
        protected PersonStatus PersonStatus = new PersonStatus();

        protected override async Task OnInitializedAsync()
        {
            if (PersonStatusId.HasValue)
            {
                PersonStatus = await PersonStatusRepository.GetPersonStatusByIdAsync(PersonStatusId.Value);
            }
        }

        protected async Task OnSubmit()
        {
            if (PersonStatusId.HasValue)
            {
                await PersonStatusRepository.UpdatePersonStatusAsync(PersonStatus);
            }
            else
            {
                PersonStatus.Id = Guid.NewGuid();
                await PersonStatusRepository.AddPersonStatusAsync(PersonStatus);
            }

            NavigationManager.NavigateTo("/personstatuses");
        }
    }
}
