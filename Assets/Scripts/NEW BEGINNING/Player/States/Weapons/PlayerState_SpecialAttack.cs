using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_SpecialAttack : PlayerState, IAddForceStats
{
    [SerializeField] float Damage, Knockback;
    Coroutine currentAttackCoroutine;
    [SerializeField] Gradient weaponTrailGradient;
    [SerializeField] AudioClip SFX_SpecialAttack;


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
        subscribeToRequests();

        playerRefs.currentStats.CurrentBloodFlow = 0;

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Slow);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.VerySlow);

        currentAttackCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_short));

        foreach (Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.Damage = Damage * playerRefs.currentStats.DamageMultiplicator;
            dealer.Stagger = Damage;
            dealer.Knockback = Knockback;
            dealer.player_isChargingSpecialAttack = false;
        }
        playerRefs.weaponTrail.colorGradient = weaponTrailGradient;
        playerRefs.animationEvents.OnShowAttackCollider += OnShowAttackCollider;
    }
    public override void OnDisable()
    {
        unsubscribeToRequests();
        if (currentAttackCoroutine != null) { StopCoroutine(currentAttackCoroutine); }
        foreach (Generic_DamageDealer dealer in playerRefs.DamageDealersList)
        {
            dealer.player_isChargingSpecialAttack = true;
        }
        playerRefs.animationEvents.OnShowAttackCollider -= OnShowAttackCollider;
        playerRefs.weaponTrail.colorGradient = playerRefs.baseWeaponGradient;
    }
    void OnShowAttackCollider() { SFX_PlayerSingleton.Instance.playSFX(SFX_SpecialAttack); }
}
