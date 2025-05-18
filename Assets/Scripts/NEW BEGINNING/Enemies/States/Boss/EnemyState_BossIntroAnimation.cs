using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_BossIntroAnimation : EnemyState
{
    Coroutine currentCoroutine;
    public override void OnEnable()
    {
        base.OnEnable();
        currentCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver( AnimatorStateName,EnemyRefs.AgrooState,0.02f));
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if(currentCoroutine != null) { StopCoroutine(currentCoroutine); }
    }
}
