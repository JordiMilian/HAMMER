using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_TwoWeapon_StanceBroken : EnemyState
{
    Coroutine currentAnimCoroutine;

    public override void OnEnable()
    {
        base.OnEnable();

        EnemyRefs.moveToTarget.SetMovementSpeed(SpeedsEnum.VerySlow);
        animator.CrossFade(AnimatorStateName, 0.1f);
        currentAnimCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, EnemyRefs.AgrooState, 0.1f));
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.StanceBroken, transform.position);

        TwoWeaponEnemy_AnimationEvents W2_animationEvents = (TwoWeaponEnemy_AnimationEvents)EnemyRefs.basicAnimationEvents;

        W2_animationEvents.EV_Enemy_HideAttackCollider();
        W2_animationEvents.EV_Enemy_HideAttackCollider_W2();
    }
    public override void OnDisable()
    {
        if (currentAnimCoroutine != null)
        {
            StopCoroutine(currentAnimCoroutine);
        }
    }
}
