using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_IdleAction : StateMachineBehaviour
{
    Player_EventSystem playerEvents;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("canTransition", true);
        animator.SetBool("isInputing", true);
        if(playerEvents == null)
        {
            playerEvents = animator.gameObject.GetComponent<Player_EventSystem>();
        }
        playerEvents.OnEnterIdle?.Invoke();
        
    }
}
