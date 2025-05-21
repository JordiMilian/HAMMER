using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_BasicGestureAttack : PlayerState, IAddForceStats, IGestureAttack
{
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
    public Vector2 gestureDirection { get; set; }
    [SerializeField] PlayerState_BasicGestureAttack nextComboState;

    public override void OnEnable()
    {
        playerRefs.animator.speed = playerRefs.currentStats.AttackSpeed;
        playerRefs.swordRotation.ForceDirection((Vector2)playerRefs.transform.position+ gestureDirection, 1);

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Slow);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Fast);
        subscribeToRequests();

        currentAttackCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_short));

        foreach (Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.Damage = Damage * playerRefs.currentStats.DamageMultiplicator;
            dealer.Knockback = Knockback;
        }
    }
    public override void OnDisable()
    {
        if (currentAttackCoroutine != null) { StopCoroutine(currentAttackCoroutine); }
        unsubscribeToRequests();
        playerRefs.animator.speed = 1;
    }
    protected override void OnTapDetected(TapData data)
    {
        if (data.tapLenght <= 1.1f)
        {
            nextComboState.GetComponent<IGestureAttack>().gestureDirection = data.endDirection;
            stateMachine.RequestChangeState(nextComboState);
        }
        else//strong attack
        {
            playerRefs.GestureAttack_Strong.GetComponent<IGestureAttack>().gestureDirection = data.endDirection;
            stateMachine.RequestChangeState(playerRefs.GestureAttack_Strong); 
        }
    }
}
