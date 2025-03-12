using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Runnig : PlayerState
{
    [SerializeField] float StaminaCostPerSecond;
    public override void OnEnable()
    {
        stateMachine.EV_ReturnInput();
        stateMachine.EV_CanTransition();

        animator.CrossFade(AnimatorStateName, transitionTime_long);

        subscribeToRequests();

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Fast);

        InputDetector.Instance.OnRollUnpressed += OnUnpressedRun;
    }
    public override void OnDisable()
    {
        unsubscribeToRequests();

        InputDetector.Instance.OnRollUnpressed -= OnUnpressedRun;

    }
    public override void Update()
    {
        playerRefs.playerStamina.RemoveStamina(StaminaCostPerSecond * Time.deltaTime);
    }
    void OnUnpressedRun()
    {
        stateMachine.ForceChangeState(playerRefs.IdleState);
    }
}
