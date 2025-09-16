using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/ConsecutiveAttacks")]
public class Upgrade_ConsecutiveAttacks : Upgrade
{
    int consecutiveAttacks = 0;
    [SerializeField] int maxConsecutiveAttacks = 5;
    [SerializeField] float consecutiveAttackMaxTime = 1f;
    [SerializeField] float addedDamagePerAttack = .15f;
    const string CoroutineID = "ConsecutiveAttackBuff";
    Player_References playerRefs;
    IDamageDealer damageDealer;
    IEnumerator ConsecutiveAttackBuff()
    {
        if(consecutiveAttacks < maxConsecutiveAttacks)
        {
            playerRefs.currentStats.DamageMultiplicator += addedDamagePerAttack;
            consecutiveAttacks++;
        }
       
        float timer = 0;
        while (timer < consecutiveAttackMaxTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        playerRefs.currentStats.DamageMultiplicator -= addedDamagePerAttack * consecutiveAttacks;
        consecutiveAttacks = 0;

    }
    void OnDealtDamage(DealtDamageInfo info)
    {
        CoroutinesRunner.instance.EndCoroutine(CoroutineID);
        CoroutinesRunner.instance.RunCoroutine(ConsecutiveAttackBuff(), CoroutineID);
    }
    public override void onAdded(GameObject entity)
    {
        playerRefs = entity.GetComponent<Player_References>();
        damageDealer = playerRefs.GetComponent<IDamageDealer>();
        damageDealer.OnDamageDealt_event += OnDealtDamage;
    }

    public override void onRemoved(GameObject entity)
    {
        damageDealer.OnDamageDealt_event -= OnDealtDamage;
    }

    public override string shortDescription()
    {
        return "Consecutive attacks deal extra damage";
    }
}
