using System.Collections.Concurrent;
using VNFarm.Interfaces.Services;

namespace VNFarm.Services
{
    public class UserConnectionService : IUserConnectionService
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>> _connections = new();

        public void AddConnection(string userId, string connectionId)
        {
            var set = _connections.GetOrAdd(userId, _ => new HashSet<string>());
            lock (set) set.Add(connectionId);
        }

        public void RemoveConnection(string connectionId)
        {
            foreach (var kv in _connections)
            {
                if (kv.Value.Contains(connectionId))
                {
                    lock (kv.Value)
                    {
                        kv.Value.Remove(connectionId);
                        if (kv.Value.Count == 0)
                            _connections.TryRemove(kv.Key, out _);
                    }
                    break;
                }
            }
        }

        public bool IsUserOnline(string userId) => _connections.ContainsKey(userId);
    }
}
