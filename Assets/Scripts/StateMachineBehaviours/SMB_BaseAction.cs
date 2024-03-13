using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_BaseAction : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isInputing", false);
        animator.SetBool("canTransition", false);
    }  
}

