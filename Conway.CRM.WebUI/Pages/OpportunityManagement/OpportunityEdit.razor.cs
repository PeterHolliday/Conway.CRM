using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class OpportunityEdit : ComponentBase
    {
        [Inject] protected IOpportunityRepository OpportunityRepository { get; set; }
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected IStageRepository StageRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Parameter] public Guid? OpportunityId { get; set; }
        protected Opportunity Opportunity = new Opportunity();
        protected List<Customer> Customers = new List<Customer>();
        protected List<Stage> Stages = new List<Stage>();

        protected override async Task OnInitializedAsync()
        {
            Customers = (await CustomerRepository.GetAllCustomersAsync()).ToList();
            Stages = (await StageRepository.GetAllStagesAsync()).ToList();

            if (OpportunityId.HasValue)
            {
                Opportunity = await OpportunityRepository.GetOpportunityByIdAsync(OpportunityId.Value);
            }
        }

        protected async Task OnSubmit()
        {
            if (OpportunityId.HasValue)
            {
                await OpportunityRepository.UpdateOpportunityAsync(Opportunity);
            }
            else
            {
                await OpportunityRepository.AddOpportunityAsync(Opportunity);
            }

            NavigationManager.NavigateTo("/opportunities");
        }
    }
}
