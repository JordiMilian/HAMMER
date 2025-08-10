
using System.Collections.Generic;
using UnityEngine;
public enum upgradeRarity
{
    Common, Uncommon, Rare, Legendary
}

[CreateAssetMenu(menuName = "Available Upgrades Holder")]
public class Upgrades_AvailableUpgrades : ScriptableObject
{
   public List<Upgrade> CommonUpgrades;
    public List<Upgrade> UncommonUpgrades;
    public List<Upgrade> RareUpgrades;
    public List<Upgrade> LegendaryUpgrades;
    [SerializeField] GameState gameState;

    public  Upgrade GetRandomUpgrade(upgradeRarity rarity)
    {
        switch (rarity)
        {
            case upgradeRarity.Common:
                List<Upgrade> CommonUpgradesTemp = new();
                CommonUpgradesTemp.AddRange(CommonUpgrades);
                CommonUpgradesTemp.Add(GetRandomUpgradeFromList(UncommonUpgrades));
                return GetRandomUpgradeFromList(CommonUpgradesTemp);
            case upgradeRarity.Uncommon:
                return GetRandomUpgradeFromList(UncommonUpgrades);
            case upgradeRarity.Rare:
                return GetRandomUpgradeFromList(RareUpgrades);
            case upgradeRarity.Legendary:
                return GetRandomUpgradeFromList(LegendaryUpgrades);
        }
        return CommonUpgrades[0];
    }
    Upgrade GetRandomUpgradeFromList(List<Upgrade> list)
    {
        int attempts = 0;
        Upgrade randomUpgrade;
        do 
        { 
            randomUpgrade = list[Random.Range(0, list.Count)];

            attempts++;
            if(attempts > 30) { Debug.LogError($"No available upgrades found"); return null; }
        }
        while (gameState.playerUpgrades.Contains(randomUpgrade));

        return randomUpgrade;
        

    }
}
