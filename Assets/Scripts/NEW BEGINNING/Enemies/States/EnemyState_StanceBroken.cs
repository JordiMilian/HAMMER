using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_StanceBroken : State
{
    [SerializeField] AnimationClip stanceBrokenAnimationClip;
    Coroutine currentAnimCoroutine;

    public override void OnEnable()
    {
        animator.CrossFade(stanceBrokenAnimationClip.name, 0.1f);
        currentAnimCoroutine = StartCoroutine(stanceBrokenCoroutine());
    }
    IEnumerator stanceBrokenCoroutine()
    {
        yield return StartCoroutine( UsefullMethods.WaitForAnimationTime(stanceBrokenAnimationClip));
        
        rootGameObject.GetComponent<IChangeStateByType>().ChangeStateByType(StateTags.Agroo);
    }
    public override void OnDisable()
    {
        if(currentAnimCoroutine != null)
        {
            StopCoroutine(currentAnimCoroutine);
        }
    }
}
