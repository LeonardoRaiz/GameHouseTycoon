using Interfaces;
using Models;

namespace Repository
{
    public class GameEconomyRepository : IGameEconomyRepository
    {
        private readonly GameEconomy _economy;

        public GameEconomyRepository(float startingMoney)
        {
            _economy = new GameEconomy(startingMoney);
        }

        public GameEconomy GetEconomy()
        {
            return _economy;
        }
        
    }
}