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
        private GameStoreController _gameStoreController;
        [SerializeField] private TextMeshProUGUI storeCountText;
        [SerializeField] private TextMeshProUGUI revenueText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image imageGameStore ;
        private GameObject _storeInstance;

        public void BuyStoreOnClick()
        {
            DebugHelper.Log($"Botão de comprar {_gameStoreController.GetStore().Name} clicado.");
            _gameStoreController.BuyStore();
            UpdateUI();
        }

        public void SellStoreOnClick()
        {
            DebugHelper.Log($"Botão de vender {_gameStoreController.GetStore().Name} clicado.");

            bool sold = _gameStoreController.SellStore();

            if (sold)
            {
                DestroyStore(); 
            }

            UpdateUI();
        }
        
        public void InitializeGameStore(string name, int count, float revenue, string image)
        {
            IGameStoreRepository gameStoreRepository = new GameStoreRepository(name, count, revenue, image);
            _gameStoreController = new GameStoreController(gameStoreRepository);
            _storeInstance = gameObject;
            
            DebugHelper.Log($"GameStore inicializado para {name} com {count} lojas e receita de ${revenue} e imagem {image}");
            UpdateUI();
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
            nameText.text = $"{store.Name}";
            storeCountText.text = $"{store.StoreCount}";
            revenueText.text = $"Valor Total: R${store.CalculateTotalRevenue()}";
            imageGameStore.sprite = Resources.Load<Sprite>($"{store.Image}");
            DebugHelper.Log($"UI Atualizada: {store.Name} = {store.StoreCount}, Receita = ${store.CalculateTotalRevenue()}");
        }
    
    }
}
