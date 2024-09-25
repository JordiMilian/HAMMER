using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FasterMovement", fileName = "FasterMovement")]
public class Upgrade_FasterMovement : Upgrade
{
    [SerializeField] float Percent;
    Player_Movement movement;
    public override void onAdded(GameObject entity)
    {
       movement = entity.GetComponent<Player_Movement>();

       float addedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
       movement.velocityMultiplier += addedValue;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        movement.velocityMultiplier -= removedValue;
    }
    public override string shortDescription()
    {
        return "<color=red>" + Percent + "%<color=black> faster movement";
    }
}
