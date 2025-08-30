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
        yield return WaitUntilAnimatorStateFinishes(thisAnimatorStateName);

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

    //normalized duration is in case we want to end waiting time before the animation is over in order to create a better transition
    protected IEnumerator WaitUntilAnimatorStateFinishes(string stateName, float normalizedDuration = 1) 
    {
        yield return null; //wait one frame so the transition can start
        float trainsitionTime = 0;
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (!currentStateInfo.IsName(stateName))
        {
            trainsitionTime += Time.deltaTime;
            currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        float animDuration = currentStateInfo.length / animator.speed - trainsitionTime;
        yield return new WaitForSeconds(animDuration * normalizedDuration);
    }
}
