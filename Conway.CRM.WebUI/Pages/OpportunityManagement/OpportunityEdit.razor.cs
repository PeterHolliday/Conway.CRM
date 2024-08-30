using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Abstractions;
using Conway.CRM.Domain.Entities;
using Conway.CRM.Domain.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Radzen;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class OpportunityEdit : ComponentBase
    {
        [Inject] protected IOpportunityRepository OpportunityRepository { get; set; }
        [Inject] protected IPersonRepository PersonRepository { get; set; }
        [Inject] protected ICustomerRepository CustomerRepository { get; set; }
        [Inject] protected IStageRepository StageRepository { get; set; }

        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] protected DialogService DialogService { get; set; }

        [Parameter] public Guid? OpportunityId { get; set; }
        protected Opportunity Opportunity = new Opportunity();
        protected List<Customer> Customers = new List<Customer>();
        protected List<Stage> Stages = new List<Stage>();
        protected List<Person> People = new List<Person>();

        private Dictionary<string, string> validationErrors = new();

        private HubConnection hubConnection;

        private bool isLocked;
        private string lockedBy;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            await SetupHubs();
        }

        protected async Task SetupHubs()
        {
            await InvokeAsync(async () =>
            {


                var hubUrl = NavigationManager.BaseUri + "editNotificationHub";

                hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl)
                    .Build();



                hubConnection.On<Guid, string>("LockAcquired", (entityId, user) =>
                {
                    if (entityId == Opportunity.Id)
                    {
                        isLocked = true;
                        lockedBy = user;
                        //StateHasChanged(); // Update UI
                    }
                });

                hubConnection.On<Guid, string>("LockFailed", (entityId, user) =>
                {
                    if (entityId == Opportunity.Id)
                    {
                        isLocked = true;
                        lockedBy = user;
                        //StateHasChanged(); // Update UI
                    }
                });

                hubConnection.On<Guid, string>("LockReleased", (entityId, user) =>
                {
                    if (entityId == Opportunity.Id)
                    {
                        isLocked = false;
                        lockedBy = null;
                        //StateHasChanged(); // Update UI
                    }
                });

                await hubConnection.StartAsync();
            });

            // Attempt to lock the entity for editing
            var lockSuccess = await hubConnection.InvokeAsync<bool>("TryLock", Opportunity.Id, "CurrentUser");

            if (!lockSuccess)
            {
                // Handle lock failure (e.g., show a message to the user)
                Console.WriteLine($"This record is currently being edited by {lockedBy}.");
            }
        }

        protected async Task LoadDataAsync()
        {
            Customers = (await CustomerRepository.GetAllCustomersAsync()).OrderBy(c => c.CompanyName).ToList();
            Stages = (await StageRepository.GetAllStagesAsync()).OrderBy(s => s.Order).ToList();

            if (OpportunityId.HasValue)
            {
                Opportunity = await OpportunityRepository.GetOpportunityByIdAsync(OpportunityId.Value);
            }

            People = (await PersonRepository.GetAllPersonsAsync()).OrderBy(a => a.FullName).ToList();

            Opportunity.StageId = Stages.First().Id;
        }

        protected async Task OnSubmit()
        {
            validationErrors.Clear();
            var result = await Validator.ValidateAsync(Opportunity);

            if (result.IsValid)
            {
                bool isUpdate = OpportunityId.HasValue;
                bool operationResult;

                if (isUpdate)
                {
                    operationResult = await OpportunityRepository.UpdateOpportunityAsync(Opportunity);
                    await hubConnection.InvokeAsync("Unlock", Opportunity.Id, "CurrentUser");
                }
                else
                {
                    operationResult = await OpportunityRepository.AddOpportunityAsync(Opportunity);
                    await hubConnection.InvokeAsync("Unlock", Opportunity.Id, "CurrentUser");
                }

                HandleOperationResult(isUpdate, operationResult);

                
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    validationErrors[error.PropertyName] = error.ErrorMessage;
                    NotificationService.Notify(new NotificationMessage() { Summary = "Validation Error", Detail = error.ErrorMessage, Severity = NotificationSeverity.Error });
                }
            }
        }

        private void HandleOperationResult(bool isUpdate, bool operationResult)
        {
            if (operationResult)
            {
                string message = isUpdate ? "Opportunity updated successfully!" : "Opportunity added successfully!";
                NotificationService.Notify(new NotificationMessage() { Summary = "Success!", Detail = message, Severity = NotificationSeverity.Success });
                NavigationManager.NavigateTo("/opportunities");
            }
            else
            {
                string message = isUpdate ? "Failed to update the opportunity." : "Failed to add the opportunity.";
                NotificationService.Notify(new NotificationMessage() { Summary = "Failure!", Detail = message, Severity = NotificationSeverity.Error });
            }
        }

        public void Dispose()
        {
            // Ensure the record is unlocked if the user navigates away
            hubConnection.InvokeAsync("Unlock", Opportunity.Id, "CurrentUser").Wait();
            hubConnection.StopAsync().Wait();
        }

        protected async Task CancelAdd()
        {
            var result = await DialogService.Confirm(
                "Are you sure you want to cancel your changes?", "Confirm Cancellation",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (result.HasValue && result.Value)
            {
                await hubConnection.InvokeAsync("Unlock", Opportunity.Id, "CurrentUser");
                NavigationManager.NavigateTo("/opportunities");
            }
        }
    }
}
