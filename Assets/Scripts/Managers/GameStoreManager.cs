using Controllers;
using Interfaces;
using Log;
using Models;
using Repository;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameStoreManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI storeCountText;
        [SerializeField] private TextMeshProUGUI revenueText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image imageGameStore;
        [SerializeField] private Slider sessionSlider;
        [SerializeField] private Button startSessionButton;

        private GameStoreController _gameStoreController;
        private GameplaySessionController _sessionController;
        private GameObject _storeInstance;
        private bool sessionHandled = false;

        private void Update()
        {
            if (_sessionController == null) return;

            _sessionController.UpdateSession(Time.deltaTime);
            sessionSlider.value = _sessionController.GetSessionProgress();

            if (_sessionController.IsSessionComplete() && !sessionHandled)
            {
                HandleSessionComplete();
                sessionHandled = true;
            }
            
        }

        public void BuyStoreOnClick()
        {
            var store = _gameStoreController.GetStore();
            DebugHelper.Log($"Botão de comprar {store.Name} clicado.");

            if (GameEconomyManager.Instance.GetCurrentMoney() > store.RevenuePerStore)
            {
                GameEconomyManager.Instance.SpendMoneyUI(store.RevenuePerStore);
                _gameStoreController.BuyStore();
                UpdateUI();
            }
            else
            {
                DebugHelper.Warn($"Dinheiro insuficiente para comprar {store.Name}!");
            }
        }

        public void SellStoreOnClick()
        {
            var store = _gameStoreController.GetStore();
            DebugHelper.Log($"Botão de vender {store.Name} clicado.");

            if (!_sessionController.IsSessionComplete())
            {
                DebugHelper.Warn($"Não é possível vender {store.Name} durante uma sessão de jogatina!");
                return;
            }

            GameEconomyManager.Instance.AddMoneyUI(store.CalculateTotalRevenue());
            bool sold = _gameStoreController.SellStore();

            if (sold)
            {
                DestroyStore();
            }

            UpdateUI();
        }

        public void StartGameplaySession()
        {
            if (_sessionController != null && !_sessionController.IsSessionActive())
            {
                _sessionController.StartNewSession(GetRandomSessionTime());
                sessionSlider.value = 0f;
                startSessionButton.interactable = false;
                sessionHandled = false;
                
                string storeName = _gameStoreController.GetStore().Name;
                DebugHelper.Log($"Chamando GetNextRequest para a loja: {storeName}");
                PlayerQueueManager.Instance?.GetNextRequest(storeName);
                
                DebugHelper.Log("Sessão de jogatina iniciada manualmente.");
            }
        }

        public void InitializeGameStore(string name, int count, float revenue, string image, float priceHour)
        {
            IGameStoreRepository gameStoreRepository = new GameStoreRepository(name, count, revenue, image, priceHour);
            _gameStoreController = new GameStoreController(gameStoreRepository);
            _storeInstance = gameObject;

            DebugHelper.Log($"GameStore inicializado: {name} com {count} loja(s), receita de R${revenue}, imagem '{image}'");

            GameStoreTracker.Instance?.RegisterStore(name);
            InitializeSession();
            UpdateUI();
        }

        private void InitializeSession()
        {
            float randomTime = GetRandomSessionTime();
            IGameplaySessionRepository sessionRepo = new GameplaySessionRepository(randomTime);
            _sessionController = new GameplaySessionController(sessionRepo);
            _sessionController.CancelSession();
            sessionSlider.value = 0f;
            startSessionButton.interactable = true;
            sessionHandled = true;
        }

        private void HandleSessionComplete()
        {
            float hours = _sessionController.GetPlayedHours();
            float revenue = _gameStoreController.GetStore().PriceHour;
            float total = revenue * hours * _gameStoreController.GetStore().StoreCount;

            GameEconomyManager.Instance.AddMoneyUI(total);
            DebugHelper.Log($"+R${total:0.00} ganhos após {hours}h de jogatina!");

            startSessionButton.interactable = true;
        }

        private void DestroyStore()
        {
            if (_storeInstance != null)
            {
                Destroy(_storeInstance);
                DebugHelper.Log($"Loja {_gameStoreController.GetStore().Name} removida da cena.");
            }
            else
            {
                DebugHelper.Warn("Tentativa de destruir a loja, mas a instância é nula.");
            }
        }

        private void UpdateUI()
        {
            GameStore store = _gameStoreController.GetStore();
            nameText.text = store.Name;
            storeCountText.text = store.StoreCount.ToString();
            revenueText.text = $"Compre mais games! R${store.RevenuePerStore}\nValor Total: R${store.CalculateTotalRevenue():0.00}";
            imageGameStore.sprite = Resources.Load<Sprite>(store.Image);

            DebugHelper.Log($"UI Atualizada: {store.Name} = {store.StoreCount}, Receita = R${store.CalculateTotalRevenue():0.00}");
        }

        private float GetRandomSessionTime()
        {
            int[] options = { 10, 30, 60 }; // tempo em segundos
            return options[Random.Range(0, options.Length)];
        }
        
    }
}
