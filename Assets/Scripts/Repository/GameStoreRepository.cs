using Interfaces;
using Models;
using UnityEngine.UI;

namespace Repository
{
    public class GameStoreRepository : IGameStoreRepository
    {
        private readonly GameStore _gameStore;

        public GameStoreRepository(string name, int initialCount, float revenuePerStore, string image, float priceHour)
        {
            _gameStore = new GameStore(name, initialCount, revenuePerStore, image, priceHour);
        }

        public GameStore GetStore()
        {
            return _gameStore;
        }
    }
}