using Interfaces;
using Models;
using System.Collections.Generic;

namespace Repository
{
    public class GameSoundRepository : IGameSoundRepository
    {
        private readonly Queue<GameSoundData> _soundQueue = new();

        public void QueueSound(GameSoundData sound)
        {
            _soundQueue.Enqueue(sound);
        }

        public GameSoundData GetNextSound()
        {
            if (_soundQueue.Count > 0)
                return _soundQueue.Dequeue();
            return null;
        }

        public bool HasPendingSound()
        {
            return _soundQueue.Count > 0;
        }
    }
}