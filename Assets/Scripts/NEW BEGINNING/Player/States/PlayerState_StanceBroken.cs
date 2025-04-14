using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_StanceBroken : PlayerState
{
    Coroutine currentCoroutine;
    public override void OnEnable()
    {
        playerRefs.movement.SetMovementSpeed(SpeedsEnum.VerySlow);

        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.StanceBroken, transform.position);

        currentCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_short));

        subscribeToRequests();
    }

    public override void OnDisable()
    {
        if(currentCoroutine != null) StopCoroutine(currentCoroutine);
        unsubscribeToRequests();
    }
}
