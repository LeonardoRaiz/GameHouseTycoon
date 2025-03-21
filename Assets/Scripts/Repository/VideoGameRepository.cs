using System.Collections.Generic;
using System.IO;
using Interfaces;
using Log;
using Models;
using UnityEngine;

namespace Repository
{
    public class VideoGameRepository : IVideoGameRepository
    {
        private readonly string _filePath = Path.Combine(Application.dataPath, "Scripts", "DataJSON", "videogames.json");

        public List<VideoGameData> LoadVideoGames()
        {
            if (File.Exists(_filePath))
            {
                string jsonData = File.ReadAllText(_filePath);
                VideoGameList gameList = JsonUtility.FromJson<VideoGameList>("{\"videoGames\":" + jsonData + "}");

                if (gameList != null && gameList.videoGames.Length > 0)
                {
                    return new List<VideoGameData>(gameList.videoGames);
                }
                else
                {
                    DebugHelper.Error("JSON carregado, mas não contém dados válidos.");
                }
            }
            else
            {
                DebugHelper.Error("Arquivo JSON não encontrado!");
            }
    
            return new List<VideoGameData>();
        }
    }
}
