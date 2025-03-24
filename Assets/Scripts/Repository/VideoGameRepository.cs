using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Models;
using UnityEngine;
using System;
using UnityEngine.Networking;

namespace Repository
{
    public class VideoGameRepository : IVideoGameRepository
    {
        private const string JsonFileName = "videogames.json";
        private List<VideoGameData> _videoGames = new();

        public void LoadVideoGamesAsync(Action<List<VideoGameData>> onLoaded)
        {
            CoroutineRunner.Run(LoadFromStreamingAssets(onLoaded));
        }

        private IEnumerator LoadFromStreamingAssets(Action<List<VideoGameData>> onLoaded)
        {
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, JsonFileName);

#if UNITY_WEBGL && !UNITY_EDITOR
    UnityWebRequest request = UnityWebRequest.Get(path);
    yield return request.SendWebRequest();

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError($"[ERROR] Erro ao carregar JSON: {request.error}");
        yield break;
    }

    string jsonContent = request.downloadHandler.text;
#else
            string jsonContent = System.IO.File.ReadAllText(path);
            yield return null;
#endif

            VideoGameList videoGameList = JsonUtility.FromJson<VideoGameList>(jsonContent);
            _videoGames = videoGameList.games;
            onLoaded?.Invoke(_videoGames);

            yield break; 
        }

        public List<VideoGameData> GetAllVideoGames()
        {
            return _videoGames;
        }
    }
}