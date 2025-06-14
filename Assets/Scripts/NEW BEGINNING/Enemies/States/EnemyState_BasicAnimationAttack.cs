using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_BasicAnimationAttack : EnemyState_Attack
{
    Coroutine animationLenghtWaiterCoroutine;
    [Header("Attacked while recovering")]
    IAttackedWhileRecovery attackedWhileRecovery;
    public override void OnEnable()
    {
        base.OnEnable();

        EnemyRefs.moveToTarget.SetMovementSpeed(SpeedsEnum.VerySlow);
        animator.CrossFade(AnimatorStateName, 0.1f);
        animationLenghtWaiterCoroutine = StartCoroutine(animationLenghtWaiter());

        if(attackedWhileRecovery == null) { attackedWhileRecovery = rootGameObject.GetComponent<IAttackedWhileRecovery>(); }
        attackedWhileRecovery.isInRecovery = false;
    }
    IEnumerator animationLenghtWaiter()
    {
        yield return null; //wait one frame so the transition can start
        AnimatorClipInfo[] nextClips = animator.GetNextAnimatorClipInfo(0);
        if (nextClips.Length > 0)
        {
            AnimationClip nextClip = nextClips[0].clip;
            yield return new WaitForSeconds(nextClip.length);
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
        EnemyRefs.basicAnimationEvents.EV_Enemy_HideAttackCollider();

        attackedWhileRecovery.isInRecovery = false;
    }
    
}
