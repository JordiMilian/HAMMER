using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_BasicComboAttack : PlayerState
{
    [SerializeField] PlayerState nextComboAttack;
    [SerializeField] float Damage, Knockback, HitStop;
    Coroutine currentAttackCoroutine;
    public override void OnEnable()
    {
        playerRefs.animator.speed = playerRefs.currentStats.AttackSpeed;
        subscribeToRequests();

        currentAttackCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_short));
       
        foreach(Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.Damage = Damage;
            dealer.Knockback = Knockback;
        }
    }
    public override void OnDisable()
    {
        if(currentAttackCoroutine != null) { StopCoroutine(currentAttackCoroutine); }
        unsubscribeToRequests();
        playerRefs.animator.speed = 1;
    }
    protected override void RequestAttack()
    {
        stateMachine.RequestChangeState(nextComboAttack);
    }
}
