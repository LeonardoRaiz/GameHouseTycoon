using System.IO;
using Log;

namespace Controllers
{
    using System.Collections.Generic;
    using Interfaces;
    using Models;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class VideoGameController
    {
        private readonly IVideoGameRepository _repository;
        private readonly Transform _buttonContainer;
        private readonly GameObject _buttonPrefab;
        private readonly System.Action<VideoGameData> _onGameSelected;

        public VideoGameController(IVideoGameRepository repository, Transform buttonContainer, GameObject buttonPrefab, System.Action<VideoGameData> onGameSelected)
        {
            _repository = repository;
            _buttonContainer = buttonContainer;
            _buttonPrefab = buttonPrefab;
            _onGameSelected = onGameSelected;
        }

        public void CreateButtons()
        {
            List<VideoGameData> videoGames = _repository.LoadVideoGames();
            foreach (var game in videoGames)
            {
                GameObject buttonObj = Object.Instantiate(_buttonPrefab, _buttonContainer);
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = $"{game.name}\n\n<size=10>Ganho por hora:</size>\nR${game.priceHour}";
                
                Transform imageContainer = buttonObj.transform.Find("ImageVideoGame"); 
                if (imageContainer != null)
                {
                    Image imgComponent = imageContainer.GetComponent<Image>();
                    if (imgComponent != null)
                    {
                        Sprite sprite = Resources.Load<Sprite>(game.image);
                        if (sprite != null)
                        {
                            imgComponent.sprite = sprite;
                        }
                        else
                        {
                            DebugHelper.Warn($"Imagem não encontrada para {game.name}: caminho -> {game.image}");
                        }
                    }
                    else
                    {
                        DebugHelper.Warn($"Nenhum componente Image encontrado dentro de {imageContainer.name}.");
                    }
                }
                else
                {
                    DebugHelper.Warn($"Filho 'ImageContainer' não encontrado no botão de {game.name}");
                }

                buttonObj.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text =
                    $"Comprar\n{game.basePrice}";
                buttonObj.GetComponentInChildren<Button>().onClick.AddListener(() => _onGameSelected(game));
            }
        }
    }
}