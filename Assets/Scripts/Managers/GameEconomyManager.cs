using Controllers;
using Interfaces;
using Repository;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class GameEconomyManager : MonoBehaviour
    {
        public static GameEconomyManager Instance { get; set; }
        
        
        [SerializeField] private TextMeshProUGUI moneyText;
        private GameEconomyController _economyController;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }
        private void Start()
        {
            IGameEconomyRepository repo = new GameEconomyRepository(1000f); // valor inicial
            _economyController = new GameEconomyController(repo);
            UpdateUI();
        }

        public void AddMoneyUI(float amount)
        {
            _economyController.AddMoney(amount);
            UpdateUI();
        }

        public void SpendMoneyUI(float amount)
        {
            if (_economyController.SpendMoney(amount))
            {
                UpdateUI();
            }
            else
            {
                Debug.LogWarning("Dinheiro insuficiente!");
            }
        }
        
        public bool CanAfford(float amount)
        {
            return _economyController.GetMoney() >= amount;
        }

        public float GetCurrentMoney()
        {
            return _economyController.GetMoney();
        }

        private void UpdateUI()
        {
            moneyText.text = $"R$:{_economyController.GetMoney():0.00}";
        }
    }
}