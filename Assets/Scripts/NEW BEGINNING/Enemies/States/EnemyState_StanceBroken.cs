using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_StanceBroken : EnemyState
{
    Coroutine currentAnimCoroutine;

    public override void OnEnable()
    {
        base.OnEnable();

        EnemyRefs.moveToTarget.SetMovementSpeed(SpeedsEnum.VerySlow);
        animator.CrossFade(AnimatorStateName, 0.1f);
        currentAnimCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName,EnemyRefs.AgrooState,0.1f));
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.StanceBroken, transform.position);
    }
    public override void OnDisable()
    {
        if(currentAnimCoroutine != null)
        {
            StopCoroutine(currentAnimCoroutine);
        }
    }
}
