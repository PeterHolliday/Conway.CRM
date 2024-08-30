using Conway.CRM.Application.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Conway.CRM.WebUI.Pages.SystemManagement
{
    public partial class ActiveLocks : ComponentBase
    {
        [Inject]
        protected LockingService LockingService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected List<KeyValuePair<Guid, string>> activeLocks;

        protected override void OnInitialized()
        {
            // Load active locks from the service
            activeLocks = LockingService.GetActiveLocks().ToList();
        }

        protected void ForceUnlock(Guid entityId)
        {
            if (LockingService.ForceUnlock(entityId))
            {
                // Refresh the lock list after unlocking
                activeLocks = LockingService.GetActiveLocks().ToList();
                NotificationService.Notify(NotificationSeverity.Success, "Unlocked", $"Entity {entityId} has been unlocked.", duration: 3000);
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", $"Failed to unlock Entity {entityId}.", duration: 3000);
            }
        }
    }
}
