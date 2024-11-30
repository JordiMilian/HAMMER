using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/SlowerStrongerAttacks", fileName = "SlowerStrongerAttacks")]
public class Upgrade_SlowerAttackHigherDamage : Upgrade
{
    [SerializeField] float SlowerPercent;
    [SerializeField] float StrongerPercent;
    Player_AnimationSpeedControler animationSpeedController;
    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {
        animationSpeedController = entity.GetComponent<Player_AnimationSpeedControler>();
        playerRefs = entity.GetComponent<Player_References>();


        float removedSpeed = animationSpeedController.BaseSpeed * UsefullMethods.normalizePercentage(SlowerPercent, false, true);
        animationSpeedController.attackingSpeed -= removedSpeed;

        float addedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator += addedDamage;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedSpeed = animationSpeedController.BaseSpeed * UsefullMethods.normalizePercentage(SlowerPercent, false, true);
        animationSpeedController.attackingSpeed += removedSpeed;

        float addedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator -= addedDamage;
    }
    public override string shortDescription()
    {
        return UsefullMethods.highlightString(StrongerPercent.ToString() + "%")
            + " stronger Attacks";
    }
}
