using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlotUI : MonoBehaviour
{
    public Image icon;
    public Text upgradeName;
    public Text description;
    public Text levelText;
    public Text costText;
    public Button upgradeButton;

    public void Initialize(UpgradeData data)
    {
        icon.sprite = data.icon;
        upgradeName.text = data.upgradeName;
        description.text = data.description;
    }
}