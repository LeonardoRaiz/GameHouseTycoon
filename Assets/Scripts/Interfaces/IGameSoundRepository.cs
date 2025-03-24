using Models;

namespace Interfaces
{
    public interface IGameSoundRepository
    {
        void QueueSound(GameSoundData sound);
        GameSoundData GetNextSound();
        bool HasPendingSound();
    }
}