using System.Collections;
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
        animator.CrossFade(AnimatorStateName, 0f);
        animationLenghtWaiterCoroutine = StartCoroutine(animationLenghtWaiter());

        if(attackedWhileRecovery == null) { attackedWhileRecovery = rootGameObject.GetComponent<IAttackedWhileRecovery>(); }
        attackedWhileRecovery.isInRecovery = false;
    }
    IEnumerator animationLenghtWaiter()
    {
        yield return WaitUntilAnimatorStateFinishes(AnimatorStateName,.9f);

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
