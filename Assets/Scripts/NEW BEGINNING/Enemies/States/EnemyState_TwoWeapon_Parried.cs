using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_TwoWeapon_Parried : EnemyState
{
    Coroutine currentStateCoroutine;
    public override void OnEnable()
    {
        base.OnEnable();

        EnemyRefs.moveToTarget.SetMovementSpeed(SpeedsEnum.VerySlow);
        EnemyRefs.moveToTarget.SetRotatinSpeed(SpeedsEnum.VerySlow);
        currentStateCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName, EnemyRefs.AgrooState, 0.1f));

        TwoWeaponEnemy_AnimationEvents W2_animationEvents = (TwoWeaponEnemy_AnimationEvents)EnemyRefs.basicAnimationEvents;

        W2_animationEvents.EV_Enemy_HideAttackCollider();
        W2_animationEvents.EV_Enemy_HideAttackCollider_W2();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if (currentStateCoroutine != null)
        {
            StopCoroutine(currentStateCoroutine);
        }
    }
}
