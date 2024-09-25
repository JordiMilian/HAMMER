using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/GainFlow", fileName = "GainFlow")]
public class Upgrade_GainCharge : Upgrade
{
    [SerializeField] float Percent;
    Player_SpecialAttack specialScript;
    public override void onAdded(GameObject entity)
    {
        specialScript = entity.GetComponent<Player_SpecialAttack>();

        float addedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        specialScript.ChargeGainMultiplier += addedValue;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedValue = 1 * UsefullMethods.normalizePercentage(Percent, false, true);
        specialScript.ChargeGainMultiplier -= removedValue;
    }
    public override string shortDescription()
    {
        return "Gain BloodFlow <color=red>" + Percent + "%<color=black> faster";
    }
}
