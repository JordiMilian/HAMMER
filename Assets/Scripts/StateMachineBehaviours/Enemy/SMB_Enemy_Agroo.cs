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

        animator.SetBool(Tags.InAgroo, true);

        enemyRefs.enemyEvents.OnEnterAgroo?.Invoke();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(Tags.InAgroo, false);

        enemyRefs.enemyEvents.OnExitAgroo?.Invoke();
    }
}
