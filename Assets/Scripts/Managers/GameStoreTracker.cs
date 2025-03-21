using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameStoreTracker : MonoBehaviour
    {
        public static GameStoreTracker Instance { get; private set; }

        private readonly List<string> _activeStores = new();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void RegisterStore(string name)
        {
            if (!_activeStores.Contains(name))
                _activeStores.Add(name);
        }

        public void UnregisterStore(string name)
        {
            if (_activeStores.Contains(name))
                _activeStores.Remove(name);
        }

        public List<string> GetActiveStoreNames()
        {
            return new List<string>(_activeStores);
        }
    }
}