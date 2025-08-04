using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/ParryWithoutStamina", fileName = "CanParryWithoutStamina")]
public class Upgrade_CanParryWithoutStamina : Upgrade
{
    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {
        playerRefs = entity.GetComponent<Player_References>();
        playerRefs.ParryingState.doesRequireStamina = false;
    }

    public override void onRemoved(GameObject entity)
    {
        playerRefs.ParryingState.doesRequireStamina = true;
    }

    public override string shortDescription()
    {
        return $"Can {UsefullMethods.highlightString("Parry")} without Stamina";
    }


}
