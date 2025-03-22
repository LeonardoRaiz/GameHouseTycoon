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
            if (_gameStoreController.TryBuyStore())
                UpdateUI();
        }

        public void SellStoreOnClick()
        {
            if (_sessionController.IsSessionActive())
            {
                DebugHelper.Warn($"Não é possível vender {_gameStoreController.GetStore().Name} durante uma sessão!");
                return;
            }
            if (_gameStoreController.TrySellStore())
            {
                DestroyStore();
                UpdateUI();
            }
        }

        public void StartGameplaySession()
        {
            if (_sessionController == null || _sessionController.IsSessionActive())
                return;

            string storeName = _gameStoreController.GetStore().Name;
            var nextPlayer = PlayerQueueManager.Instance?.PeekNextRequest(storeName);

            if (nextPlayer == null)
            {
                DebugHelper.Warn($"Nenhum jogador na fila para a loja {storeName}.");
                return;
            }

            if (nextPlayer.DesiredStore != storeName)
            {
                DebugHelper.Warn($"Jogador quer jogar em {nextPlayer.DesiredStore}, não em {storeName}.");
                return;
            }

            _sessionController.StartNewSession(GetRandomSessionTime());
            sessionSlider.value = 0f;
            startSessionButton.interactable = false;
            sessionHandled = false;

            PlayerQueueManager.Instance.GetNextRequest(storeName); 
            DebugHelper.Log($"Sessão iniciada para {storeName}. Jogador atendido: {nextPlayer.DesiredStore}");
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
            VideoGameLoader loader = FindObjectOfType<VideoGameLoader>();
            loader?.NotifyPlayerServed();
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
            storeCountText.text = $"Quantidade de jogos:\n{store.StoreCount.ToString()}";
            revenueText.text = $"Compre mais games! R${store.RevenuePerStore / 3}\nValor Total: R${store.CalculateTotalRevenue():0.00}";
            imageGameStore.sprite = Resources.Load<Sprite>(store.Image);

            DebugHelper.Log($"UI Atualizada: {store.Name} = {store.StoreCount}, Receita = R${store.CalculateTotalRevenue():0.00}");
        }

        private float GetRandomSessionTime()
        {
            int[] options = { 10, 30, 60 }; 
            return options[Random.Range(0, options.Length)];
        }
        
    }
}
