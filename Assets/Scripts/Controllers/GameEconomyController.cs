using Interfaces;
using Log;

namespace Controllers
{
    public class GameEconomyController
    {
        private readonly IGameEconomyRepository _repository;

        public GameEconomyController(IGameEconomyRepository repository)
        {
            _repository = repository;
        }

        public float GetMoney()
        {
            return _repository.GetEconomy().CurrentMoney;
        }

        public void SetMoney(float amount)
        {
            _repository.GetEconomy().CurrentMoney = amount;
            DebugHelper.Log($"Dinheiro definido: ${amount}");
        }

        public void AddMoney(float amount)
        {
            _repository.GetEconomy().CurrentMoney += amount;
            DebugHelper.Log($"+${amount} adicionados. Total: ${GetMoney()}");
        }

        public bool SpendMoney(float amount)
        {
            var economy = _repository.GetEconomy();

            if (economy.CurrentMoney >= amount)
            {
                economy.CurrentMoney -= amount;
                DebugHelper.Log($"-${amount} gastos. Total: ${GetMoney()}");
                return true;
            }

            DebugHelper.Warn("Dinheiro insuficiente!");
            return false;
        }
    }
}