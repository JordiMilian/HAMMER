using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_IdleAction : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("canTransition", true);
        animator.SetBool("isInputing", true);
    }
}
