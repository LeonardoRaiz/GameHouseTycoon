using Interfaces;
using Models;

namespace Controllers
{
    public class GameInfoController
    {
        private readonly IGameInfoRepository _repository;

        public GameInfoController(IGameInfoRepository repository)
        {
            _repository = repository;
        }

        public void AddMessage(string content, float duration = 3f)
        {
            var msg = new GameInfoMessage(content, duration);
            _repository.QueueMessage(msg);
        }

        public GameInfoMessage GetNextMessage()
        {
            return _repository.GetNextMessage();
        }

        public bool HasMessage()
        {
            return _repository.HasPendingMessage();
        }
    }
}