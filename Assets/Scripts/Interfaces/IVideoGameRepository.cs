using System;
using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IVideoGameRepository
    {
        void LoadVideoGamesAsync(Action<List<VideoGameData>> onLoaded);
        List<VideoGameData> GetAllVideoGames();
    }
}