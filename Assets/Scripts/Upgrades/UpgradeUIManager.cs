using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeUIManager : MonoBehaviour
{
    public GameObject upgradeSlotPrefab;
    public Transform upgradeContainer;
    public Text currencyText;

    private List<UpgradeSlotUI> upgradeSlotUIs = new List<UpgradeSlotUI>();
    [SerializeField] private UpgradeManager upgradeManager;

    void Start()
    {
        
        if (upgradeManager == null)
        {
            upgradeManager = FindObjectOfType<UpgradeManager>();
            if (upgradeManager == null)
            {
                Debug.LogError("UpgradeManager not found in the scene! Please assign it in the inspector or add it to the scene.");
            }
            return;
        }

        GenerateUpgradeSlots();
    }

    void GenerateUpgradeSlots()
    {
        foreach (var upgradeSlot in upgradeManager.upgradeSlots)
        {
            GameObject slotObject = Instantiate(upgradeSlotPrefab, upgradeContainer);
            UpgradeSlotUI slotUI = slotObject.GetComponent<UpgradeSlotUI>();

            if (slotUI != null)
            {
                slotUI.Initialize(upgradeSlot.upgradeData);
                upgradeSlotUIs.Add(slotUI);

                int index = upgradeManager.upgradeSlots.IndexOf(upgradeSlot);
                slotUI.upgradeButton.onClick.AddListener(() => upgradeManager.PurchaseUpgrade(index));
            }
        }
    }

    public void UpdateAllSlots(List<UpgradeManager.UpgradeSlot> slots, int currency)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            UpdateSlotUI(slots[i], upgradeSlotUIs[i], currency);
        }
        UpdateCurrencyDisplay(currency);
    }

    private void UpdateSlotUI(UpgradeManager.UpgradeSlot slot, UpgradeSlotUI slotUI, int currency)
    {
        slotUI.levelText.text = $"Level: {slot.currentLevel}/{slot.upgradeData.maxLevel}";
        int cost = CalculateUpgradeCost(slot);
        slotUI.costText.text = $"Cost: {cost}";
        slotUI.upgradeButton.interactable = (currency >= cost && slot.currentLevel < slot.upgradeData.maxLevel);
    }

    private int CalculateUpgradeCost(UpgradeManager.UpgradeSlot slot)
    {
        return Mathf.RoundToInt(slot.upgradeData.baseCost * Mathf.Pow(slot.upgradeData.costMultiplier, slot.currentLevel));
    }

    private void UpdateCurrencyDisplay(int currency)
    {
        currencyText.text = $"Currency: {currency}";
    }
}

