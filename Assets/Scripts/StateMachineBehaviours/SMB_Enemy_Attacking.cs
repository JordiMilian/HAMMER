using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Enemy_Attacking : StateMachineBehaviour
{

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attacking", false);
        animator.gameObject.GetComponent<Enemy_AttacksProviderV2>().AttackExited();
    }
}
