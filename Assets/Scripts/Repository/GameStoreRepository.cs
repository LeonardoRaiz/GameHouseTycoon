using Interfaces;
using Models;
using UnityEngine.UI;

namespace Repository
{
    public class GameStoreRepository : IGameStoreRepository
    {
        private readonly GameStore _gameStore;

        public GameStoreRepository(string name, int initialCount, float revenuePerStore, string image)
        {
            _gameStore = new GameStore(name, initialCount, revenuePerStore, image);
        }

        public GameStore GetStore()
        {
            return _gameStore;
        }
    }
}