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

        EnemyRefs.moveToTarget.SetMovementSpeed(MovementSpeeds.Slow);
        animator.CrossFade(AnimatorStateName, 0.1f);
        animationLenghtWaiterCoroutine = StartCoroutine(animationLenghtWaiter());

        if(attackedWhileRecovery == null) { attackedWhileRecovery = rootGameObject.GetComponent<IAttackedWhileRecovery>(); }
        attackedWhileRecovery.isInRecovery = false;
    }
    IEnumerator animationLenghtWaiter()
    {
        yield return new WaitForSeconds(UsefullMethods.GetAnimationClipByStateName(AnimatorStateName, animator).length);
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
