using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FasterStamina", fileName = "FasterStamina")]
public class Upgrade_FasterStamina : Upgrade
{
    [SerializeField] float Percent;
    Player_Stamina staminaScript;
    public override void onAdded(GameObject entity)
    {
        staminaScript = entity.GetComponent<Player_Stamina>();

        float addedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        staminaScript.RecoverSpeedMultiplier += addedValue;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        staminaScript.RecoverSpeedMultiplier -= removedValue;
    }
    public override string shortDescription()
    {
        return "Recover Stamina <color=red>" + Percent + "%<color=black> faster";
    }
}
