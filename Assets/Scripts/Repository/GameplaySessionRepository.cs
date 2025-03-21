using Interfaces;
using Models;

namespace Repository
{
    public class GameplaySessionRepository : IGameplaySessionRepository
    {
        private readonly GameplaySession _session;

        public GameplaySessionRepository(float totalTime)
        {
            _session = new GameplaySession(totalTime);
        }

        public float GetTotalTime() => _session.TotalTime;
        public float GetCurrentTime() => _session.CurrentTime;

        public void SetTotalTime(float time) => _session.TotalTime = time;
        public void SetCurrentTime(float time) => _session.CurrentTime = time;
    }
}