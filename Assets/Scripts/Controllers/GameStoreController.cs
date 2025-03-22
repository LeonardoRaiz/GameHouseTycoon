using Interfaces;
using Models;
using Log;
using Managers;

namespace Controllers
{
    public class GameStoreController
    {
        private readonly IGameStoreRepository _repository;

        public GameStoreController(IGameStoreRepository repository)
        {
            _repository = repository;
        }

        public void BuyStore(float totalGames)
        {
            var store = _repository.GetStore();
            store.SetStoreCount(store.StoreCount + 1);
            _repository.GetStore().TotalGames += totalGames;
            DebugHelper.Log($"[CONTROLLER] Loja comprada! Total: {store.StoreCount}");
        }

        public bool SellStore()
        {
            if (CanSellStore())
            {
                var store = _repository.GetStore();
                store.SetStoreCount(store.StoreCount - 1);
                DebugHelper.Log($"[CONTROLLER] Loja vendida! Total: {store.StoreCount}");
                return true;
            }

            DebugHelper.Warn($"[CONTROLLER] Tentativa de venda falhou. Estoque zerado.");
            return false;
        }
        
        private bool CanSellStore()
        {
            return _repository.GetStore().StoreCount > 0;
        }

        public GameStore GetStore()
        {
            return _repository.GetStore();
        }
        
        public bool TryBuyStore()
        {
            var store = _repository.GetStore();
            var priceGames = (store.RevenuePerStore / 3);
            if (GameEconomyManager.Instance.GetCurrentMoney() >= priceGames)
            {
                GameEconomyManager.Instance.SpendMoneyUI(priceGames);
                BuyStore(priceGames);
                DebugHelper.Log($"Um novo game para a loja {store.Name} foi comprado.");
                return true;
            }

            DebugHelper.Warn($"Dinheiro insuficiente para comprar um novo game para a loja {store.Name}!");
            return false;
        }
        
        public bool TrySellStore()
        {
            var store = _repository.GetStore();

            if (store.StoreCount > 0)
            {
                GameEconomyManager.Instance.AddMoneyUI(store.CalculateTotalRevenue());
                return SellStore();
            }

            DebugHelper.Warn($"Tentativa de vender {store.Name}, mas nenhuma loja restante.");
            return false;
        }
        
    }
}