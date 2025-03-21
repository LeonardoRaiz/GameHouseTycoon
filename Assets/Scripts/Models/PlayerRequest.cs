namespace Models
{
    public class PlayerRequest
    {
        public string DesiredStore { get; private set; }

        public PlayerRequest(string desiredStore)
        {
            DesiredStore = desiredStore;
        }
    }
}