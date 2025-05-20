using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_PhaseTransition_GreenBoss : EnemyState
{
    Coroutine currentCoroutine;
    public override void OnEnable()
    {
        base.OnEnable();
        GameObject puddleController = GameObject.Find("HalfHealth_PuddleCreatureController");
        StartCoroutine(puddleController.GetComponent<HalfHealth_PuddleCreature>().appearPuddle());

        //move to position?
        EnemyRefs.stanceMeter.MakeStanceUnbreakeable();
        currentCoroutine = StartCoroutine( AutoTransitionToStateOnAnimationOver(AnimatorStateName, EnemyRefs.AgrooState, 0.1f));
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if(currentCoroutine != null) { StopCoroutine(currentCoroutine); }
        EnemyRefs.stanceMeter.ReturnToRegularStance();
    }
}
