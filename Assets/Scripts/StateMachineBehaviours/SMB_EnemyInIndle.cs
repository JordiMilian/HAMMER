using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_EnemyInIndle : StateMachineBehaviour
{
    Enemy_EventSystem events;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("inIdle", true);
        if(events == null) { events = animator.gameObject.GetComponent<Enemy_EventSystem>(); }
        events.OnEnterIdle?.Invoke();
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("inIdle", false);
        events.OnExitIdle?.Invoke();
    }
    
}
