using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/SlowerStrongerAttacks", fileName = "SlowerStrongerAttacks")]
public class Upgrade_SlowerAttackHigherDamage : Upgrade
{
    [SerializeField] float SlowerPercent;
    [SerializeField] float StrongerPercent;

    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {

        playerRefs = entity.GetComponent<Player_References>();


        float removedSpeed = playerRefs.baseStats.AttackSpeed * UsefullMethods.normalizePercentage(SlowerPercent, false, true);
        playerRefs.currentStats.AttackSpeed -= removedSpeed;

        float addedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator += addedDamage;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedSpeed = playerRefs.baseStats.AttackSpeed * UsefullMethods.normalizePercentage(SlowerPercent, false, true);
        playerRefs.currentStats.AttackSpeed += removedSpeed;

        float addedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(StrongerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator -= addedDamage;
    }
    public override string shortDescription()
    {
       // return UsefullMethods.highlightString(StrongerPercent.ToString() + "%") + " stronger Attacks";
        return  $"More {UsefullMethods.highlightString("Damage")}";
    }
}
