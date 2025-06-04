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
        yield return WaitForNextAnimationToFinish();

        stateMachine.ChangeState(stateToChange);
    }
    protected IEnumerator WaitForNextAnimationToFinish()
    {
        yield return null; //wait one frame so the transition can start
        AnimatorClipInfo[] nextClips = animator.GetNextAnimatorClipInfo(0);
        if (nextClips.Length > 0)
        {
            AnimationClip nextClip = nextClips[0].clip;
            yield return new WaitForSeconds(nextClip.length);
        }
    }
}
