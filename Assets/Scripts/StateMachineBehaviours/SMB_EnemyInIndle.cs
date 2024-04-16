using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_EnemyInIndle : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("inIdle", true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("inIdle", false);
    }
    
}
