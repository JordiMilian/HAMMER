using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Parried : PlayerState
{
    Coroutine currentCoroutine;
  public override void OnEnable()
    {
        base.OnEnable();
        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Slow);


        currentCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, playerRefs.IdleState, transitionTime_instant));
        playerRefs.animationEvents.EV_Enemy_HideAttackCollider();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if(currentCoroutine != null) { StopCoroutine(currentCoroutine); }
    }
}
