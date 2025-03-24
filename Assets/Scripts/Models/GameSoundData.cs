namespace Models
{
    public class GameSoundData
    {
        public string SoundName { get; }
        public float Volume { get; }

        public GameSoundData(string soundName, float volume = 1f)
        {
            SoundName = soundName;
            Volume = volume;
        }
    }
}