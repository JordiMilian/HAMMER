using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/SlowerStrongerAttacks", fileName = "SlowerStrongerAttacks")]
public class Upgrade_SlowerAttackHigherDamage : Upgrade
{
    [SerializeField] float SlowerPercent;
    [SerializeField] float StrongerPercent;
    Player_AnimationSpeedControler animationSpeedController;
    Generic_Stats stats;
    public override void onAdded(GameObject entity)
    {
        animationSpeedController = entity.GetComponent<Player_AnimationSpeedControler>();
        stats = entity.GetComponent<Generic_Stats>();

        animationSpeedController.attackingSpeed *= UsefullMethods.normalizePercentage(SlowerPercent, true);
        stats.DamageMultiplier *= UsefullMethods.normalizePercentage(StrongerPercent);
    }
    public override void onRemoved(GameObject entity)
    {
        animationSpeedController.attackingSpeed /= UsefullMethods.normalizePercentage(SlowerPercent, true);
        stats.DamageMultiplier /= UsefullMethods.normalizePercentage(StrongerPercent);
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(SlowerPercent.ToString() + "%")
            + " slower swings - "
            + UsefullMethods.highlightString(StrongerPercent.ToString() + "%")
            + " stronger Attacks";
    }
}
