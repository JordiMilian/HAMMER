using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FasterStamina", fileName = "FasterStamina")]
public class Upgrade_FasterStamina : Upgrade
{
    [SerializeField] float Percent;
    PlayerStats currentStats;
    public override void onAdded(GameObject entity)
    {
        currentStats = entity.GetComponent<Player_References>().currentStats;

        float addedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        currentStats.RecoveryStaminaSpeed += addedValue;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        currentStats.RecoveryStaminaSpeed -= removedValue;
    }
    public override string shortDescription()
    {
        return "Recover Stamina <color=red>" + Percent + "%<color=black> faster";
    }
}
