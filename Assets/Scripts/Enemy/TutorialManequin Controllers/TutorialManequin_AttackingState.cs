using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManequin_AttackingState : State
{
    [SerializeField] Generic_OnTriggerEnterEvents playerDetectionTrigger;
    [SerializeField] State IdleState;
    [SerializeField] string attackingBoolName;
    public override void OnEnable()
    {
        base.OnEnable();
        animator.SetBool(attackingBoolName,true);
        playerDetectionTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        playerDetectionTrigger.OnTriggerExited += OnPlayerExited;

    }
    void OnPlayerExited(Collider2D collider)
    {
        stateMachine.ChangeState(IdleState);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        playerDetectionTrigger.OnTriggerExited -= OnPlayerExited;
        //untrigger the bool
    }

}
