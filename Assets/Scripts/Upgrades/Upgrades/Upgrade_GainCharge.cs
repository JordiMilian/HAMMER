using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/GainFlow", fileName = "GainFlow")]
public class Upgrade_GainCharge : Upgrade
{
    [SerializeField] float Percent;
    PlayerStats currentStats;
    public override void onAdded(GameObject entity)
    {
        currentStats = entity.GetComponent<Player_References>().currentStats;

        float addedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        currentStats.BloodflowMultiplier += addedValue;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        currentStats.BloodflowMultiplier -= removedValue;
    }
    public override string shortDescription()
    {
        return "Gain BloodFlow <color=red>" + Percent + "%<color=black> faster";
    }
}
