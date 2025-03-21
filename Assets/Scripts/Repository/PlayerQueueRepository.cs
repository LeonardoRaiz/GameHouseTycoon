using Interfaces;
using Models;
using System.Collections.Generic;

namespace Repository
{
    public class PlayerQueueRepository : IPlayerQueueRepository
    {
        private readonly Dictionary<string, Queue<PlayerRequest>> _queuesByStore = new();

        public void Enqueue(PlayerRequest player)
        {
            if (!_queuesByStore.ContainsKey(player.DesiredStore))
                _queuesByStore[player.DesiredStore] = new Queue<PlayerRequest>();

            _queuesByStore[player.DesiredStore].Enqueue(player);
        }

        public PlayerRequest Dequeue(string storeName)
        {
            if (_queuesByStore.ContainsKey(storeName) && _queuesByStore[storeName].Count > 0)
                return _queuesByStore[storeName].Dequeue();

            return null;
        }

        public PlayerRequest Peek(string storeName)
        {
            if (_queuesByStore.ContainsKey(storeName) && _queuesByStore[storeName].Count > 0)
                return _queuesByStore[storeName].Peek();

            return null;
        }

        public int GetQueueSize(string storeName)
        {
            if (_queuesByStore.ContainsKey(storeName))
                return _queuesByStore[storeName].Count;

            return 0;
        }

        public Dictionary<string, Queue<PlayerRequest>> GetAllQueues()
        {
            return _queuesByStore;
        }
    }
}