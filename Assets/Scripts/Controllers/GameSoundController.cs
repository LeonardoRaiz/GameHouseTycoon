using Interfaces;
using Models;

namespace Controllers
{
    public class GameSoundController
    {
        private readonly IGameSoundRepository _repository;

        public GameSoundController(IGameSoundRepository repository)
        {
            _repository = repository;
        }

        public void PlaySound(string soundName, float volume = 1f)
        {
            var sound = new GameSoundData(soundName, volume);
            _repository.QueueSound(sound);
        }

        public bool HasSound() => _repository.HasPendingSound();

        public GameSoundData GetNextSound() => _repository.GetNextSound();
    }
}