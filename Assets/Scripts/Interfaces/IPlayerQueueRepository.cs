using Models;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IPlayerQueueRepository
    {
        void Enqueue(PlayerRequest player);
        PlayerRequest Dequeue(string storeName);
        PlayerRequest Peek(string storeName);
        int GetQueueSize(string storeName);
        Dictionary<string, Queue<PlayerRequest>> GetAllQueues();
    }
}