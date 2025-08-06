using System.Collections.Generic;
public enum UpgradesType
{
    MaxHp, DamageAfterParry
}
[System.Serializable]
public class UpgradesDatabase
{
    public List<Upgrade> Upgrades;

    public Upgrade GetUpgradeByType(UpgradesType type)
    {
        foreach (var upgrade in Upgrades)
        {
            if (upgrade.upgradeType == type)
            {
                return upgrade;
            }
        }
        return null;
    }
}
