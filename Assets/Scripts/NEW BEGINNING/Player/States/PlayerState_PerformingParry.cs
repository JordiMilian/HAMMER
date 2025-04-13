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

        playerRefs.movement2.SetMovementSpeed(SpeedsEnum.Slow);
        SFX_PlayerSingleton.Instance.playSFX(SFX_PerformParry, 0.1f, -0.5f, 0.5f);
        currentCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_short));
    }
    public override void OnDisable()
    {
        base.OnDisable();

        if(currentCoroutine != null) { StopCoroutine(currentCoroutine); }
    }
}
