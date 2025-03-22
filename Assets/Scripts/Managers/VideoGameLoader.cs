using Log;
using Controllers;
using Interfaces;
using Repository;
using Models;
using UnityEngine;

namespace Managers
{
    public class VideoGameLoader : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject gameStorePrefab;
        [SerializeField] private Transform storeContainer;

        private VideoGameController _videoGameController;

        void Start()
        {
            IVideoGameRepository repository = new VideoGameRepository();
            var games = repository.LoadVideoGames();
            _videoGameController = new VideoGameController(repository, buttonContainer, buttonPrefab, OnVideoGameSelected);
            _videoGameController.InitializeButtons(games);
        }

        void OnVideoGameSelected(VideoGameData selectedGame)
        {
            DebugHelper.Log($"Selecionado: {selectedGame.name}, Ano: {selectedGame.year}, Preço: ${selectedGame.basePrice}");
            SpawnGameStore(selectedGame);
        }

        void SpawnGameStore(VideoGameData gameData)
        {
            if (storeContainer.childCount >= 6)
            {
                DebugHelper.Error("Limite máximo de GameStore atingido!");
                return;
            }

            float storeCost = gameData.basePrice;

            if (GameEconomyManager.Instance.GetCurrentMoney() < storeCost)
            {
                DebugHelper.Warn($"Dinheiro insuficiente para comprar {gameData.name}!");
                return;
            }

            GameEconomyManager.Instance.SpendMoneyUI(storeCost);

            GameObject newStore = Instantiate(gameStorePrefab, storeContainer);
            GameStoreManager storeManager = newStore.GetComponent<GameStoreManager>();

            if (storeManager != null)
            {
                storeManager.InitializeGameStore(gameData.name, 1, gameData.basePrice, gameData.image, gameData.priceHour);
            }
            else
            {
                DebugHelper.Error("GameStoreManager não encontrado no prefab!");
            }
        }

        public void NotifyPlayerServed()
        {
            _videoGameController.NotifyPlayerServed();
        }
    }
}