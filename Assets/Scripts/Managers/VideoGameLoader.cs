using Log;


namespace Managers
{
    using Controllers;
    using Interfaces;
    using Repository;
    using Models;
    using UnityEngine;

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
            _videoGameController = new VideoGameController(repository, buttonContainer, buttonPrefab, OnVideoGameSelected);
            _videoGameController.CreateButtons();
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
                DebugHelper.Error("Limite máximo de GamaStore atingido!");
                return;
            }
            
            GameObject newStore = Instantiate(gameStorePrefab, storeContainer);
            GameStoreManager storeManager = newStore.GetComponent<GameStoreManager>();
            
            if (storeManager != null)
            {
                storeManager.InitializeGameStore(gameData.name, 1, gameData.basePrice, gameData.image);
            }
            else
            {
                DebugHelper.Error("GameStoreManager não encontrado no prefab!");
            }
        }
    }
}