using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public int maxLevel;
    public int baseCost;
    public float costMultiplier;
    public Sprite icon;

    public virtual void ApplyUpgrade(PlayerController player, int level)
    {
        // Override this method in derived classes to apply specific upgrades
    }
}

// Example of a specific upgrade type
[CreateAssetMenu(fileName = "New Max Health Upgrade", menuName = "Upgrades/Max Health Upgrade")]
public class MaxHealthUpgrade : UpgradeData
{
    public int healthIncreasePerLevel = 10;

    public override void ApplyUpgrade(PlayerController player, int level)
    {
        player.maxHealth += healthIncreasePerLevel * level;
    }
}

// Another example
[CreateAssetMenu(fileName = "New Move Speed Upgrade", menuName = "Upgrades/Move Speed Upgrade")]
public class MoveSpeedUpgrade : UpgradeData
{
    public float speedIncreasePerLevel = 5f;

    public override void ApplyUpgrade(PlayerController player, int level)
    {
        player.moveSpeed += speedIncreasePerLevel * level;
    }
}