using System.Collections.Concurrent;

namespace Conway.CRM.Application.Services
{
    public class LockingService
    {
        private readonly ConcurrentDictionary<Guid, string> _locks = new ConcurrentDictionary<Guid, string>();

        public bool TryLock(Guid entityId, string user)
        {
            return _locks.TryAdd(entityId, user);
        }

        public bool IsLocked(Guid entityId, out string lockedBy)
        {
            return _locks.TryGetValue(entityId, out lockedBy);
        }

        public bool Unlock(Guid entityId, string user)
        {
            return _locks.TryRemove(new KeyValuePair<Guid, string>(entityId, user));
        }

        public IEnumerable<KeyValuePair<Guid, string>> GetActiveLocks()
        {
            return _locks.ToList();
        }

        public bool ForceUnlock(Guid entityId)
        {
            return _locks.TryRemove(entityId, out _);
        }
    }
}
