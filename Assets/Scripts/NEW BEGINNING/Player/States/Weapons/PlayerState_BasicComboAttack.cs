using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_BasicComboAttack : PlayerState, IAddForceStats
{
    [SerializeField] PlayerState nextComboAttack;
    [SerializeField] float Damage, Knockback, HitStop;
    Coroutine currentAttackCoroutine;
    #region ADD FORCE STATS
    [Header("Add Force Stats")]
    [SerializeField] float _minDistance;
    [SerializeField] float _maxDistance, _offset, _defaultDistance;
    public float DefaultOtherDistance { get { return _defaultDistance; } set { _defaultDistance = value; } }
    public float MinOtherDistance { get { return _minDistance; } set { _minDistance = value; } }
    public float MaxOtherDistance { get { return _maxDistance; } set { _maxDistance = value; } }
    public float Offset { get { return _offset; } set { _offset = value; } }
    #endregion

    public override void OnEnable()
    {
        playerRefs.animator.speed = playerRefs.currentStats.AttackSpeed;

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Slow);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Slow);
        subscribeToRequests();

        currentAttackCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_short));
       
        foreach(Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.player_isChargingSpecialAttack = true;
            dealer.Damage = Damage * playerRefs.currentStats.DamageMultiplicator;
            dealer.Stagger = Damage;
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
