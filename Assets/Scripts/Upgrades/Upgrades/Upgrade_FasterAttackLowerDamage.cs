using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FasterWeakerAttacks", fileName = "FasterWeakerAttacks")]
public class Upgrade_FasterAttackLowerDamage : Upgrade
{
    [SerializeField] float FasterPercent;
    [SerializeField] float WeakerPercent;
    Player_AnimationSpeedControler animationSpeedController;
    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {
        animationSpeedController = entity.GetComponent<Player_AnimationSpeedControler>();
        playerRefs = entity.GetComponent<Player_References>();

        float addedSpeed = animationSpeedController.BaseSpeed * UsefullMethods.normalizePercentage(FasterPercent, false, true) ;
        animationSpeedController.attackingSpeed += addedSpeed;

        float removedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(WeakerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator -= removedDamage;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedSpeed = animationSpeedController.BaseSpeed * UsefullMethods.normalizePercentage(FasterPercent, false, true);
        animationSpeedController.attackingSpeed -= removedSpeed;

        float addedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(WeakerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator += addedDamage;
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(FasterPercent.ToString() + "%")
            + " faster Swings";
    }
}
