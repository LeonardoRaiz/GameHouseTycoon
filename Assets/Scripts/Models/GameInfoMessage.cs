namespace Models
{
    public class GameInfoMessage
    {
        public string Content { get; set; }
        public float Duration { get; set; }

        public GameInfoMessage(string content, float duration)
        {
            Content = content;
            Duration = duration;
        }
    }
}