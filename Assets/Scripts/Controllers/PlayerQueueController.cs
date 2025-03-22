using Interfaces;
using Models;
using System.Collections.Generic;

namespace Controllers
{
    public class PlayerQueueController
    {
        private readonly IPlayerQueueRepository _repository;

        public PlayerQueueController(IPlayerQueueRepository repository)
        {
            _repository = repository;
        }

        public void AddPlayer(PlayerRequest player)
        {
            _repository.Enqueue(player);
        }

        public PlayerRequest GetNextPlayerForStore(string storeName)
        {
            return _repository.Dequeue(storeName);
        }

        public PlayerRequest PeekNextRequest(string storeName)
        {
            return _repository.Peek(storeName);
        }

        public int GetQueueSizeForStore(string storeName)
        {
            return _repository.GetQueueSize(storeName);
        }

        public Dictionary<string, Queue<PlayerRequest>> GetAllQueues()
        {
            return _repository.GetAllQueues();
        }
    }
}