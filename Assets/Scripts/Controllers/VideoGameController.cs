using System.Collections.Generic;
using Interfaces;
using Log;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class VideoGameController
    {
        private readonly IVideoGameRepository _repository;
        private readonly Transform _buttonContainer;
        private readonly GameObject _buttonPrefab;
        private readonly System.Action<VideoGameData> _onGameSelected;

        private int playersServed = 0;
        private int unlockThreshold = 1;
        private int currentUnlockedIndex = 0;

        private List<VideoGameData> _allVideoGames;
        private List<VideoGameData> _unlockedGames = new();

        public VideoGameController(IVideoGameRepository repository, Transform buttonContainer, GameObject buttonPrefab, System.Action<VideoGameData> onGameSelected)
        {
            _repository = repository;
            _buttonContainer = buttonContainer;
            _buttonPrefab = buttonPrefab;
            _onGameSelected = onGameSelected;
        }

        public void InitializeButtons(List<VideoGameData> games)
        {
            _allVideoGames = games;
            UnlockNextGame(); 
        }

        private void UnlockNextGame()
        {
            if (currentUnlockedIndex < _allVideoGames.Count)
            {
                VideoGameData game = _allVideoGames[currentUnlockedIndex];
                CreateButton(game);
                _unlockedGames.Add(game);

                DebugHelper.LogController($"Novo videogame desbloqueado: {game.name}", "VideoGameController");


                GameObject prefab = Resources.Load<GameObject>("Prefabs/UnlockNotificationPrefab");
                if (prefab != null)
                {
                    GameObject notification = Object.Instantiate(prefab, GameObject.Find("UI Canvas").transform); 
                    notification.SetActive(true);

                    var nameText = notification.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
                    var iconImage = notification.transform.Find("Icon")?.GetComponent<Image>();

                    if (nameText != null) nameText.text = game.name.ToUpper();
                    if (iconImage != null)
                    {
                        Sprite sprite = Resources.Load<Sprite>(game.image);
                        if (sprite != null) iconImage.sprite = sprite;
                    }

                    Object.Destroy(notification, 4f);
                }
                else
                {
                    DebugHelper.WarnController("Prefab de notificação não encontrado!", "VideoGameController");
                }

                currentUnlockedIndex++;
            }
        }

        private void CreateButton(VideoGameData game)
        {
            GameObject buttonObj = Object.Instantiate(_buttonPrefab, _buttonContainer);
            
            var nameText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (nameText != null)
                nameText.text = game.name.ToUpper(); 
            
            var imageComponents = buttonObj.GetComponentsInChildren<Image>(true);
            foreach (var img in imageComponents)
            {
                if (img.gameObject.name == "ImageVideoGame")
                {
                    Sprite sprite = Resources.Load<Sprite>(game.image);
                    if (sprite != null)
                    {
                        img.sprite = sprite;
                    }
                    else
                    {
                        DebugHelper.WarnController($"Sprite não encontrado para: {game.image}", "VideoGameController");
                    }
                }
            }

            // Botão e preço
            var button = buttonObj.GetComponentInChildren<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => _onGameSelected(game));
                var priceText = button.GetComponentInChildren<TextMeshProUGUI>();
                if (priceText != null)
                    priceText.text = $"Comprar\n{game.basePrice}";
            }
        }

        public void NotifyPlayerServed()
        {
            playersServed++;

            DebugHelper.LogController($"Jogadores atendidos: {playersServed}", "VideoGameController");

            if (playersServed >= unlockThreshold)
            {
                playersServed = 0;
                unlockThreshold += 5;
                UnlockNextGame();
            }
        }
    }
}