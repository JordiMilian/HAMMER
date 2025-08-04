using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max Stamina", fileName = "Max Stamina")]
public class Upgrade_MaxStamina : Upgrade
{
    PlayerStats currentStats;
    PlayerStats baseStats;
    [SerializeField] float Percent;
    public override void onAdded(GameObject entity)
    {

        currentStats = entity.GetComponent<Player_References>().currentStats;
        baseStats = entity.GetComponent<Player_References>().baseStats;

        float addedStamina = baseStats.MaxStamina * UsefullMethods.normalizePercentage(Percent, false, true);
        currentStats.MaxStamina += addedStamina;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedStamina = baseStats.MaxStamina * UsefullMethods.normalizePercentage(Percent, false, true);
        currentStats.MaxStamina -= removedStamina;
    }
    public override string shortDescription()
    {
        //return UsefullMethods.highlightString(Percent.ToString() + "%") + " more Stamina";
        return $"More {UsefullMethods.highlightString("Max Stamina")}";
    }
}
