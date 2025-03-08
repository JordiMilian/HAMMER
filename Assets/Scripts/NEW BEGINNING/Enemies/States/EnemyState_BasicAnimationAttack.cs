using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_BasicAnimationAttack : EnemyState_Attack
{
    [SerializeField] AnimationClip attackAnimationClip;
    Coroutine animationLenghtWaiterCoroutine;
    public override void OnEnable()
    {
        animator.CrossFade(attackAnimationClip.name, 0.1f);
        animationLenghtWaiterCoroutine = StartCoroutine(animationLenghtWaiter());
    }
    IEnumerator animationLenghtWaiter()
    {
        float timer = 0;
        while (timer < attackAnimationClip.length)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        OnAttackFinished();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if (animationLenghtWaiterCoroutine != null)
        {
            StopCoroutine(animationLenghtWaiterCoroutine);
        }
    }
    
}
