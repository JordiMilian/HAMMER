using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_FasterMovement : Upgrade
{
    [SerializeField] float Percent;
    Player_Movement movement;
    public override void onAdded(GameObject entity)
    {
       movement = entity.GetComponent<Player_Movement>();

       float addedValue = movement.BaseSpeed * UsefullMethods.normalizePercentage(Percent, false, true);
       movement.CurrentSpeed += addedValue;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = movement.BaseSpeed * UsefullMethods.normalizePercentage(Percent, false, true);
        movement.CurrentSpeed -= removedValue;
    }
    public override string shortDescription()
    {
        return "<color=red>" + Percent + "%<color=black> faster movement";
    }
}
