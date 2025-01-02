using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Enemy_Idle : StateMachineBehaviour
{
    Enemy_References enemyRefs;
    Generic_OnTriggerEnterEvents agrooTrigger;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyRefs == null) { enemyRefs = animator.GetComponent<Enemy_References>(); }

        agrooTrigger = enemyRefs.playerInAgrooCollider;

        enemyRefs.moveToTarget.DoMove = false;
        enemyRefs.moveToTarget.MovementTarget = null;
        enemyRefs.moveToTarget.DoLook = false;
        enemyRefs.moveToTarget.LookingTarget = null;
        enemyRefs.attackProvider.isProviding = false;

        agrooTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        agrooTrigger.OnTriggerEntered += AgrooTriggerEntered;

        enemyRefs.enemyEvents.OnEnterIdle?.Invoke();
    }

    void AgrooTriggerEntered(Collider2D collision)
    {
        agrooTrigger.OnTriggerEntered -= AgrooTriggerEntered;

        if(collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            enemyRefs.animator.SetTrigger(Tags.playerDetected);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyRefs.enemyEvents.OnExitIdle?.Invoke();
    }
}
