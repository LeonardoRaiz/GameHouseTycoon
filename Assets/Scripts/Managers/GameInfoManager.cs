using Controllers;
using Interfaces;
using Models;
using Repository;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class GameInfoManager : MonoBehaviour
    {
        public static GameInfoManager Instance { get; private set; }

        [SerializeField] private TextMeshProUGUI infoText;

        private GameInfoController _controller;
        private bool _isShowingMessage = false;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            IGameInfoRepository repo = new GameInfoRepository();
            _controller = new GameInfoController(repo);
            infoText.text = "";
        }

        private void Update()
        {
            if (!_isShowingMessage && _controller.HasMessage())
            {
                GameInfoMessage message = _controller.GetNextMessage();
                StartCoroutine(DisplayMessage(message));
            }
        }

        public void ShowMessage(string content, float duration = 3f)
        {
            _controller.AddMessage(content, duration);
        }

        private System.Collections.IEnumerator DisplayMessage(GameInfoMessage message)
        {
            _isShowingMessage = true;
            infoText.text = message.Content.ToUpper(); // opcional
            yield return new WaitForSeconds(message.Duration);
            infoText.text = "";
            _isShowingMessage = false;
        }
    }
}