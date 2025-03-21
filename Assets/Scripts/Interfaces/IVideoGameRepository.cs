namespace Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IVideoGameRepository
    {
        List<VideoGameData> LoadVideoGames();
    }
}