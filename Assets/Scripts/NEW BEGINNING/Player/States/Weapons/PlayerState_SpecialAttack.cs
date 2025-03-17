using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_SpecialAttack : PlayerState
{
    [SerializeField] float Damage, Knockback, HitStop;
    Coroutine currentAttackCoroutine;
    public override void OnEnable()
    {
        subscribeToRequests();

        playerRefs.currentStats.CurrentBloodFlow = 0;

        currentAttackCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_short));

        foreach (Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.Damage = Damage;
            dealer.Knockback = Knockback;
            dealer.HitStop = HitStop;
        }
    }
    public override void OnDisable()
    {
        unsubscribeToRequests();
        if (currentAttackCoroutine != null) { StopCoroutine(currentAttackCoroutine); }
    }
}
