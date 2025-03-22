using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Interfaces;
using Log;
using Models;
using Repository;
using TMPro;

namespace Managers
{
    public class PlayerQueueManager : MonoBehaviour
    {
        public static PlayerQueueManager Instance { get; private set; }

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform queuePoint;
        [SerializeField] private Transform queueContainer;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private float spacing = 1.5f;
        [SerializeField] private float spawnInterval = 5f;
        [SerializeField] private int maxQueueSize = 5;

        private PlayerQueueController _controller;
        private List<GameObject> _visualQueue = new();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            IPlayerQueueRepository repo = new PlayerQueueRepository();
            _controller = new PlayerQueueController(repo);

            StartCoroutine(WaitForStoresThenStartQueue());
        }

        private IEnumerator WaitForStoresThenStartQueue()
        {
            while (GameStoreTracker.Instance == null || GameStoreTracker.Instance.GetActiveStoreNames().Count == 0)
                yield return null;

            DebugHelper.Log("GameStore registrada, fila visual iniciada...");
            StartCoroutine(AutoSpawnPlayers());
        }

        private IEnumerator AutoSpawnPlayers()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                if (_visualQueue.Count < maxQueueSize)
                {
                    string storeName = GetRandomAvailableStore();
                    if (!string.IsNullOrEmpty(storeName))
                    {
                        SpawnNewPlayer(storeName);
                    }
                }
            }
        }

        private string GetRandomAvailableStore()
        {
            List<string> stores = GameStoreTracker.Instance?.GetActiveStoreNames();
            if (stores == null || stores.Count == 0) return null;

            return stores[Random.Range(0, stores.Count)];
        }

        public void SpawnNewPlayer(string desiredStoreName)
        {
            PlayerRequest playerData = new(desiredStoreName);
            _controller.AddPlayer(playerData);

            GameObject visual = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity, queueContainer);
            visual.GetComponentInChildren<TextMeshProUGUI>().text = desiredStoreName;
            visual.GetComponent<PlayerTag>().desiredStoreName = desiredStoreName;

            Vector3 targetPos = queuePoint.position + Vector3.down * spacing * _visualQueue.Count;
            StartCoroutine(MoveToPosition(visual.transform, targetPos));

            _visualQueue.Add(visual);
        }

        private IEnumerator MoveToPosition(Transform obj, Vector3 target)
        {
            while (Vector3.Distance(obj.position, target) > 0.05f)
            {
                obj.position = Vector3.MoveTowards(obj.position, target, 5f * Time.deltaTime);
                yield return null;
            }
        }

        private void ReorganizeQueue()
        {
            for (int i = 0; i < _visualQueue.Count; i++)
            {
                Vector3 targetPos = queuePoint.position + Vector3.down * spacing * i;
                StartCoroutine(MoveToPosition(_visualQueue[i].transform, targetPos));
            }
        }

        public PlayerRequest GetNextRequest(string storeName)
        {
            DebugHelper.Log($"Tentando atender próximo jogador da fila para: {storeName}");

            if (_controller.GetQueueSizeForStore(storeName) == 0)
            {
                DebugHelper.Warn("Nenhum jogador na fila lógica!");
                return null;
            }

            for (int i = 0; i < _visualQueue.Count; i++)
            {
                GameObject obj = _visualQueue[i];
                PlayerTag tag = obj.GetComponent<PlayerTag>();

                if (tag == null)
                {
                    DebugHelper.Error("PlayerTag não encontrado no objeto visual!");
                    continue;
                }

                DebugHelper.Log($"Verificando jogador {i} na fila com tag: {tag.desiredStoreName}");

                if (tag.desiredStoreName == storeName)
                {
                    DebugHelper.Log($"Removendo jogador visual da fila para: {storeName}");
                    Destroy(obj);
                    _visualQueue.RemoveAt(i);
                    ReorganizeQueue();
                    break;
                }
            }

            return _controller.GetNextPlayerForStore(storeName);
        }

        public int GetQueueCountForStore(string storeName)
        {
            return _controller.GetQueueSizeForStore(storeName);
        }

        public PlayerRequest PeekNextRequest(string storeName)
        {
            return _controller.PeekNextRequest(storeName);
        }
    }
}