using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    protected Enemy_References EnemyRefs;
    [SerializeField] protected string AnimatorStateName;
    public override void OnEnable()
    {
        if(EnemyRefs == null)
        {
            EnemyRefs = rootGameObject.GetComponent<Enemy_References>();
        }
    }
    protected IEnumerator AutoTransitionToStateOnAnimationOver(string thisAnimatorStateName, State stateToChange, float normalizedTransitionDuration)
    {
        animator.CrossFade(thisAnimatorStateName, normalizedTransitionDuration);
        AnimationClip thisClip = UsefullMethods.GetAnimationClipByStateName(thisAnimatorStateName, animator);
        yield return new WaitForSeconds(thisClip.length);
        stateMachine.ChangeState(stateToChange);
    }
}
