using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManequin_IdleState : State
{
    [SerializeField] Generic_OnTriggerEnterEvents playerDetectionTrigger;
    [SerializeField] string AttackingBoolString = "Attacking";
    [SerializeField] State AttackingState;
    public override void OnEnable()
    {
        base.OnEnable();
        animator.SetBool(AttackingBoolString, false);
        playerDetectionTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        playerDetectionTrigger.OnTriggerEntered += OnPlayerEntered;
    }
    void OnPlayerEntered(Collider2D otherCollider)
    {
        stateMachine.ChangeState(AttackingState);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        playerDetectionTrigger.OnTriggerEntered -= OnPlayerEntered;
    }
}
