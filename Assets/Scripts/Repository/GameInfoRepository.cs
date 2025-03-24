using Interfaces;
using Models;
using System.Collections.Generic;

namespace Repository
{
    public class GameInfoRepository : IGameInfoRepository
    {
        private readonly Queue<GameInfoMessage> _messageQueue = new();

        public void QueueMessage(GameInfoMessage message)
        {
            _messageQueue.Enqueue(message);
        }

        public GameInfoMessage GetNextMessage()
        {
            if (_messageQueue.Count > 0)
                return _messageQueue.Dequeue();
            return null;
        }

        public bool HasPendingMessage()
        {
            return _messageQueue.Count > 0;
        }
    }
}