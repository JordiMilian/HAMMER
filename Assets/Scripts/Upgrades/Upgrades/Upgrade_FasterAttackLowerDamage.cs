using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FasterWeakerAttacks", fileName = "FasterWeakerAttacks")]
public class Upgrade_FasterAttackLowerDamage : Upgrade
{
    [SerializeField] float FasterPercent;
    [SerializeField] float WeakerPercent;

    Player_References playerRefs;
    public override void onAdded(GameObject entity)
    {

        playerRefs = entity.GetComponent<Player_References>();

        float addedSpeed = playerRefs.baseStats.AttackSpeed * UsefullMethods.normalizePercentage(FasterPercent, false, true) ;
        playerRefs.currentStats.AttackSpeed += addedSpeed;

        float removedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(WeakerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator -= removedDamage;
    }
    public override void onRemoved(GameObject entity)
    {
        float removedSpeed = playerRefs.baseStats.AttackSpeed * UsefullMethods.normalizePercentage(FasterPercent, false, true);
        playerRefs.currentStats.AttackSpeed -= removedSpeed;

        float addedDamage = playerRefs.baseStats.DamageMultiplicator * UsefullMethods.normalizePercentage(WeakerPercent, false, true);
        playerRefs.currentStats.DamageMultiplicator += addedDamage;
    }
    public override string shortDescription()
    {
        return "Faster sword swings";
    }
}
