using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Parried : EnemyState
{
    Coroutine currentStateCoroutine;
    [SerializeField] BasicEnemy_AnimationEvents animationEvents;
    public override void OnEnable()
    {
        base.OnEnable();

        EnemyRefs.moveToTarget.SetMovementSpeed(MovementSpeeds.VerySlow);
        EnemyRefs.moveToTarget.SetRotatinSpeed(MovementSpeeds.VerySlow);
        currentStateCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(AnimatorStateName,EnemyRefs.AgrooState,0.1f));

        animationEvents.EV_Enemy_HideAttackCollider();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if(currentStateCoroutine != null)
        {
            StopCoroutine(currentStateCoroutine);
        }
    }
}
