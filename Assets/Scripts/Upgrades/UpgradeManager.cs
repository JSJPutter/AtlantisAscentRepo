using UnityEngine;
using System.Collections.Generic;
using Core;

public class UpgradeManager : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeSlot
    {
        public UpgradeData upgradeData;
        public int currentLevel;
    }

    public List<UpgradeSlot> upgradeSlots;
    public int currency;

    [SerializeField] private UpgradeUIManager uiManager; // Serialized field for inspector assignment

    void Awake()
    {
        // If uiManager is not assigned in the inspector, try to find it in the scene
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UpgradeUIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UpgradeUIManager not found in the scene! Please assign it in the inspector or add it to the scene.");
            }
        }
    }

    void Start()
    {
        LoadUpgrades();
        UpdateUI();
    }

    void LoadUpgrades()
    {
        // Load saved upgrade levels and currency
        currency = PlayerPrefs.GetInt("Currency", 1000);
        foreach (var slot in upgradeSlots)
        {
            slot.currentLevel = PlayerPrefs.GetInt($"Upgrade_{slot.upgradeData.upgradeName}_Level", 0);
        }
    }

    public void PurchaseUpgrade(int index)
    {
        UpgradeSlot slot = upgradeSlots[index];
        int cost = CalculateUpgradeCost(slot);

        if (currency >= cost && slot.currentLevel < slot.upgradeData.maxLevel)
        {
            currency -= cost;
            slot.currentLevel++;
            ApplyUpgrade(slot);
            UpdateUI();
        }
    }

    int CalculateUpgradeCost(UpgradeSlot slot)
    {
        return Mathf.RoundToInt(slot.upgradeData.baseCost * Mathf.Pow(slot.upgradeData.costMultiplier, slot.currentLevel));
    }

    void ApplyUpgrade(UpgradeSlot slot)
    {
        // Apply the upgrade effect to the player
        slot.upgradeData.ApplyUpgrade(GameManager.Instance.Player, slot.currentLevel);
    }

    void UpdateUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateAllSlots(upgradeSlots, currency);
        }
        else
        {
            Debug.LogWarning("UpgradeUIManager is not set. UI will not be updated.");
        }
    }

    public void ReturnToGame()
    {
        // Save upgrades and return to the main game scene
        SaveUpgrades();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameplayScene");
    }

    void SaveUpgrades()
    {
        PlayerPrefs.SetInt("Currency", currency);
        foreach (var slot in upgradeSlots)
        {
            PlayerPrefs.SetInt($"Upgrade_{slot.upgradeData.upgradeName}_Level", slot.currentLevel);
        }
        PlayerPrefs.Save();
    }
}