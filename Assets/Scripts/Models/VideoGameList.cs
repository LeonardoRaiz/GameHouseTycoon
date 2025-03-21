namespace Models
{
    using UnityEngine;

    [System.Serializable]
    public class VideoGameList
    {
        public VideoGameData[] videoGames;

        public static VideoGameList FromJson(string json)
        {
            return JsonUtility.FromJson<VideoGameList>("{\"videoGames\":" + json + "}");
        }
    }
}