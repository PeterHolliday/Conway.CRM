using Conway.CRM.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace Conway.CRM.WebUI.Hubs
{
    public class EditNotificationHub : Hub
    {
        private readonly LockingService _lockingService;

        public EditNotificationHub(LockingService lockingService)
        {
            _lockingService = lockingService;
        }

        public async Task<bool> TryLock(Guid entityId, string user)
        {
            if (_lockingService.TryLock(entityId, user))
            {
                await Clients.Others.SendAsync("LockAcquired", entityId, user);
                return true;
            }
            else
            {
                string lockedBy;
                _lockingService.IsLocked(entityId, out lockedBy);
                await Clients.Caller.SendAsync("LockFailed", entityId, lockedBy);
                return false;
            }
        }

        public async Task Unlock(Guid entityId, string user)
        {
            if (_lockingService.Unlock(entityId, user))
            {
                await Clients.Others.SendAsync("LockReleased", entityId, user);
            }
        }
    }
}
