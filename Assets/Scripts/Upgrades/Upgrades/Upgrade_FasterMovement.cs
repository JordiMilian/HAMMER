using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FasterMovement", fileName = "FasterMovement")]
public class Upgrade_FasterMovement : Upgrade
{
    [SerializeField] float Percent;
    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {
        playerRefs = GlobalPlayerReferences.Instance.references;

       float addedValue = playerRefs.baseStats.BaseSpeed * UsefullMethods.normalizePercentage(Percent, false, true);
       playerRefs.currentStats.BaseSpeed += addedValue;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = playerRefs.baseStats.BaseSpeed * UsefullMethods.normalizePercentage(Percent, false, true);
        playerRefs.currentStats.BaseSpeed -= removedValue;
    }
    public override string shortDescription()
    {
        return "<color=red>" + Percent + "%<color=black> faster movement";
    }
}
