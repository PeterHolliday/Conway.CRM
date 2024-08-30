using Conway.CRM.Application.Interfaces;
using Conway.CRM.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using Radzen.Blazor;

namespace Conway.CRM.WebUI.Pages.OpportunityManagement
{
    public partial class OpportunityList : ComponentBase

    {
        [Inject] protected IOpportunityRepository OpportunityRepository { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected DialogService DialogService { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }

        protected RadzenDataGrid<Opportunity> grid;
        protected List<Opportunity> Opportunities;

        private bool isLoading = false;
        private bool isLocked;
        private string lockedBy;

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        protected async Task LoadDataAsync()
        {
            isLoading = true;
            Opportunities = (await OpportunityRepository.GetAllOpportunitiesWithCustomersAsync()).ToList();
            isLoading = false;
        }

        protected void AddOpportunity()
        {
            NavigationManager.NavigateTo("/opportunities/add");
        }

        protected async void EditOpportunity(Guid opportunityId)
        {
            var lockSuccess = await SetupHubs(opportunityId);

            if (lockSuccess)
            {
                NavigationManager.NavigateTo($"/opportunities/edit/{opportunityId}");
            }
            else
            {
                NotificationService.Notify(new NotificationMessage() { Summary = "Locked Record", Detail = "That Opportunity is currently locked", Severity=NotificationSeverity.Error });
            }
        }

        protected async Task DeleteOpportunity(Guid opportunityId)
        {
            var result = await DialogService.Confirm(
                "Are you sure you want to delete this Opportunity?", "Confirm Delete",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (result.HasValue && result.Value)
            {
                await OpportunityRepository.DeleteOpportunityAsync(opportunityId);
                Opportunities = (await OpportunityRepository.GetOpportunitiesByCustomerIdAsync(Guid.Empty)).ToList();
                await grid.Reload();
            }
        }

        protected async Task<bool> SetupHubs(Guid opportunityId)
        {
            await InvokeAsync(async () =>
            {


                var hubUrl = NavigationManager.BaseUri + "editNotificationHub";

                hubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl)
                    .Build();



                hubConnection.On<Guid, string>("LockAcquired", (entityId, user) =>
                {
                    if (entityId == opportunityId)
                    {
                        isLocked = true;
                        lockedBy = user;
                        //StateHasChanged(); // Update UI
                    }
                });

                hubConnection.On<Guid, string>("LockFailed", (entityId, user) =>
                {
                    if (entityId == opportunityId)
                    {
                        isLocked = true;
                        lockedBy = user;
                        //StateHasChanged(); // Update UI
                    }
                });

                hubConnection.On<Guid, string>("LockReleased", (entityId, user) =>
                {
                    if (entityId == opportunityId)
                    {
                        isLocked = false;
                        lockedBy = null;
                        //StateHasChanged(); // Update UI
                    }
                });

                await hubConnection.StartAsync();
            });

            // Attempt to lock the entity for editing
            var lockSuccess = await hubConnection.InvokeAsync<bool>("TryLock", opportunityId, "CurrentUser");

            if (!lockSuccess)
            {
                // Handle lock failure (e.g., show a message to the user)
                Console.WriteLine($"This record is currently being edited by {lockedBy}.");
            }

            return lockSuccess;
        }
    }
}
