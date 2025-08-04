using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Max HP")]
public class Upgrade_MaxHP : Upgrade
{
    [SerializeField] float Percent;
    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {
        playerRefs = entity.GetComponent<Player_References>();

        float addedValue = (playerRefs.baseStats.MaxHp * UsefullMethods.normalizePercentage(Percent,false,true));
        playerRefs.currentStats.MaxHp +=  addedValue;
        playerRefs.currentStats.CurrentHp = playerRefs.currentStats.CurrentHp + addedValue;

    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = (playerRefs.baseStats.MaxHp * UsefullMethods.normalizePercentage(Percent,false, true));
        //float newValue = playerHealth.MaxHP.GetValue() / (1 + (Percent / 100));
        playerRefs.currentStats.MaxHp -= removedValue;
        playerRefs.currentStats.CurrentHp = playerRefs.currentStats.CurrentHp - removedValue;
    }
    public override string shortDescription()
    {
        //return "<color=red>" + Percent + "%<color=black> more HP";
        return $"More {UsefullMethods.highlightString("Max HP")}";
    }
}
