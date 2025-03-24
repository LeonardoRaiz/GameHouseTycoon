using Models;

namespace Interfaces
{
    public interface IGameInfoRepository
    {
        GameInfoMessage GetNextMessage();
        void QueueMessage(GameInfoMessage message);
        bool HasPendingMessage();
    }
}