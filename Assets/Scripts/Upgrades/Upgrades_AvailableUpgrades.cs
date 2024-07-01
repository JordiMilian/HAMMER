using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Available Upgrades Holder")]
public class Upgrades_AvailableUpgrades : ScriptableObject
{
   public List<Upgrade> AvailableUpgrades;

    public  Upgrade GetRandomUpgrade()
    {
        int randomIndex = Random.Range(0, AvailableUpgrades.Count);
        return AvailableUpgrades[randomIndex];
    }
}
