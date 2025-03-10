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

        animator.CrossFade(AnimatorStateName, 0.1f);

        subscribeToRequests();

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Running);

        InputDetector.Instance.OnRollUnpressed += OnUnpressedRun;
    }
    public override void OnDisable()
    {
        unsubscribeToRequests();

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Regular);

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
