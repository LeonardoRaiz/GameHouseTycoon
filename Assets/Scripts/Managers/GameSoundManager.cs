using Controllers;
using Interfaces;
using Log;
using Models;
using Repository;
using UnityEngine;

namespace Managers
{
    public class GameSoundManager : MonoBehaviour
    {
        public static GameSoundManager Instance { get; private set; }

        [SerializeField] private AudioSource audioSource;

        private GameSoundController _controller;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            IGameSoundRepository repo = new GameSoundRepository();
            _controller = new GameSoundController(repo);
        }

        private void Update()
        {
            if (!_audioPlaying && _controller.HasSound())
            {
                GameSoundData sound = _controller.GetNextSound();
                PlayClip(sound);
            }
        }

        private bool _audioPlaying => audioSource != null && audioSource.isPlaying;

        private void PlayClip(GameSoundData soundData)
        {
            AudioClip clip = Resources.Load<AudioClip>(soundData.SoundName);
            if (clip != null && audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.volume = soundData.Volume;
                audioSource.Play();
            }
            else
            {
                DebugHelper.Warn($"Som não encontrado: {soundData.SoundName}");
            }
        }

        public void PlaySound(string soundName, float volume = 1f)
        {
            _controller.PlaySound(soundName, volume);
        }
    }
}