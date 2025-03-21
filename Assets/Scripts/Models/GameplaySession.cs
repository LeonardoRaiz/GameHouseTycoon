namespace Models
{
    public class GameplaySession
    {
        public float TotalTime { get; set; }
        public float CurrentTime { get; set; }

        public GameplaySession(float totalTime)
        {
            TotalTime = totalTime;
            CurrentTime = 0;
        }
    }
}