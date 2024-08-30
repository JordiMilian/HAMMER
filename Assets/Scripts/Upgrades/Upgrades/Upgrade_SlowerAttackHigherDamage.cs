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


        float removedSpeed = animationSpeedController.BaseSpeed * UsefullMethods.normalizePercentage(SlowerPercent, false, true);
        animationSpeedController.attackingSpeed -= removedSpeed;

        float addedDamage = stats.BaseDamageMultiplier * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        stats.DamageMultiplier += addedDamage;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedSpeed = animationSpeedController.BaseSpeed * UsefullMethods.normalizePercentage(SlowerPercent, false, true);
        animationSpeedController.attackingSpeed += removedSpeed;

        float addedDamage = stats.BaseDamageMultiplier * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        stats.DamageMultiplier -= addedDamage;
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(StrongerPercent.ToString() + "%")
            + " stronger Attacks";
    }
}
