using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState
{
    public override void OnEnable()
    {
        stateMachine.EV_ReturnInput();
        stateMachine.EV_CanTransition();

        animator.CrossFade("Idle", 0.1f);

        subscribeToRequests();

        //Start recovering stamina
        playerRefs.playerStamina.StartRecovering();

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Regular);
    }
    public override void OnDisable()
    {
       unsubscribeToRequests();

        playerRefs.playerStamina.StopRecovering();
    }

}
