using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Enemy_Attacking : StateMachineBehaviour
{
    Generic_EventSystem events;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attacking", false);
        if(events == null) { events = animator.gameObject.GetComponent<Generic_EventSystem>(); }

        events.OnAttackFinished?.Invoke();
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (events == null) { events = animator.gameObject.GetComponent<Generic_EventSystem>(); }

        events.OnStartAttack?.Invoke();
    }
}
