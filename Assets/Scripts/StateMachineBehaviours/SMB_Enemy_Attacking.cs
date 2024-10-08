using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Enemy_Attacking : StateMachineBehaviour
{
    Enemy_References references;
    Generic_EventSystem events;
    Enemy_AgrooMovement agrooMovement;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attacking", false);
        if(references == null) { references = animator.gameObject.GetComponent<Enemy_References>(); }

        references.enemyEvents.OnAttackFinished?.Invoke();
       
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (references == null) { references = animator.gameObject.GetComponent<Enemy_References>(); }

        references.enemyEvents.OnStartAttack?.Invoke();
        references.agrooMovement.EV_SlowMovingSpeed();
    }
}
