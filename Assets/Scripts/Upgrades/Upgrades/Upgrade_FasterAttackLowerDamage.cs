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

        float addedSpeed = animationSpeedController.BaseSpeed * UsefullMethods.normalizePercentage(FasterPercent, false, true) ;
        animationSpeedController.attackingSpeed += addedSpeed;

        float removedDamage = stats.BaseDamageMultiplier * UsefullMethods.normalizePercentage(WeakerPercent, false, true);
        stats.DamageMultiplier -= removedDamage;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedSpeed = animationSpeedController.BaseSpeed * UsefullMethods.normalizePercentage(FasterPercent, false, true);
        animationSpeedController.attackingSpeed -= removedSpeed;

        float addedDamage = stats.BaseDamageMultiplier * UsefullMethods.normalizePercentage(WeakerPercent, false, true);
        stats.DamageMultiplier += addedDamage;
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(FasterPercent.ToString() + "%")
            + " faster swings - ";
    }
}
