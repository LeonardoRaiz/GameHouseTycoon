using Interfaces;
using Models;

namespace Controllers
{
    public class GameplaySessionController
    {
        private readonly IGameplaySessionRepository _repository;

        public GameplaySessionController(IGameplaySessionRepository repository)
        {
            _repository = repository;
        }

        public void StartNewSession(float duration)
        {
            _repository.SetTotalTime(duration);
            _repository.SetCurrentTime(0);
            _repository.SetSessionActive(true);
        }

        public void CancelSession()
        {
            _repository.SetSessionActive(false);
            _repository.SetCurrentTime(0);
        }

        public void UpdateSession(float deltaTime)
        {
            if (!_repository.GetSessionActive()) return;

            float current = _repository.GetCurrentTime();
            float total = _repository.GetTotalTime();

            current += deltaTime;
            _repository.SetCurrentTime(current);

            if (current >= total)
            {
                _repository.SetSessionActive(false);
            }
        }

        public float GetSessionProgress()
        {
            float total = _repository.GetTotalTime();
            return total == 0 ? 0 : _repository.GetCurrentTime() / total;
        }

        public float GetPlayedHours()
        {
            return _repository.GetTotalTime() / 60f;
        }

        public bool IsSessionComplete()
        {
            return !_repository.GetSessionActive() && _repository.GetCurrentTime() >= _repository.GetTotalTime();
        }

        public bool IsSessionActive()
        {
            return _repository.GetSessionActive();
        }
    }
}