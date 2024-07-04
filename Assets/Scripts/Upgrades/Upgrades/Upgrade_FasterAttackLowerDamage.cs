using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FasterWeakerAttacks", fileName = "FasterWeakerAttacks")]
public class Upgrade_FasterAttackLowerDamage : Upgrade
{
    [SerializeField] float FasterPercent;
    [SerializeField] float WeakerPercent;
    Player_AnimationSpeedControler animationSpeedController;
    Generic_Stats stats;
    public override void onAdded(GameObject entity)
    {
        animationSpeedController = entity.GetComponent<Player_AnimationSpeedControler>();
        stats = entity.GetComponent<Generic_Stats>();

        animationSpeedController.attackingSpeed *= UsefullMethods.normalizePercentage(FasterPercent);
        stats.DamageMultiplier *= UsefullMethods.normalizePercentage(WeakerPercent,true);
    }
    public override void onRemoved(GameObject entity)
    {
        animationSpeedController.attackingSpeed /= UsefullMethods.normalizePercentage(FasterPercent);
        stats.DamageMultiplier /= UsefullMethods.normalizePercentage(WeakerPercent, true);
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(FasterPercent.ToString() + "%")
            + " faster swings - "
            + UsefullMethods.highlightString(WeakerPercent.ToString() + "%")
            + " weaker Attacks";
    }
}
