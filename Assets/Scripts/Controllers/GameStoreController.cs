using Interfaces;
using Models;
using Log;

namespace Controllers
{
    public class GameStoreController
    {
        private readonly IGameStoreRepository _repository;

        public GameStoreController(IGameStoreRepository repository)
        {
            _repository = repository;
        }

        public void BuyStore()
        {
            var store = _repository.GetStore();
            store.SetStoreCount(store.StoreCount + 1);
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
    }
}