using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class SMB_Enemy_Agroo : StateMachineBehaviour
{
    Enemy_References enemyRefs;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyRefs == null) { enemyRefs = animator.GetComponent<Enemy_References>(); }

        Transform PlayerTransform = GlobalPlayerReferences.Instance.playerTf;

        enemyRefs.moveToTarget.LookingTarget = PlayerTransform;
        enemyRefs.moveToTarget.DoLook = true;
        enemyRefs.moveToTarget.MovementTarget = PlayerTransform;
        enemyRefs.moveToTarget.DoMove = true;
        enemyRefs.attackProvider.isProviding = true;

        animator.SetBool("inAgroo", true);

        enemyRefs.enemyEvents.OnEnterAgroo?.Invoke();

        
       
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("inAgroo", false);

        enemyRefs.enemyEvents.OnExitAgroo?.Invoke();
    }
}
