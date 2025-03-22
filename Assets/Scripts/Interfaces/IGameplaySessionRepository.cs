namespace Interfaces
{
    using Models;

    public interface IGameplaySessionRepository
    {
        float GetTotalTime();
        float GetCurrentTime();
        void SetTotalTime(float time);
        void SetCurrentTime(float time);
        void SetSessionActive(bool isActive);
        bool GetSessionActive();
    }
}