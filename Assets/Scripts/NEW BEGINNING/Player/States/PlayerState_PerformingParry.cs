using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_PerformingParry : PlayerState
{
    Coroutine currentCoroutine;
    [SerializeField] AudioClip SFX_PerformParry;
    public override void OnEnable()
    {
        base.OnEnable();

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.VerySlow);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Regular);
        SFX_PlayerSingleton.Instance.playSFX(SFX_PerformParry, 0.1f, -0.5f, 0.5f);
        currentCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_instant));
    }
    public override void OnDisable()
    {
        base.OnDisable();

        if(currentCoroutine != null) { StopCoroutine(currentCoroutine); }
    }
}
