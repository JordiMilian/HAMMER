using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_AttackAction : SMB_BaseAction
{
    Player_EventSystem playerEvents;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isInputing", false);
        animator.SetBool("canTransition", false);

        animator.SetBool("Act_Attack", false);

        if(playerEvents == null) { playerEvents = animator.gameObject.GetComponent<Player_EventSystem>(); }

        playerEvents.OnAttackStarted?.Invoke();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerEvents.OnAttackFinished?.Invoke();
    }

}
