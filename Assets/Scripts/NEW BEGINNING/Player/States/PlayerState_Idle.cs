using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState
{
    [SerializeField] string AnimatorStateName_Still;
    [SerializeField] string AnimatorStateName_Walking;
    bool isAnimationWalking = false;
    public override void OnEnable()
    {
        stateMachine.EV_ReturnInput();
        stateMachine.EV_CanTransition();

        animator.CrossFade(AnimatorStateName_Still, transitionTime_long);
        isAnimationWalking = false;

        subscribeToRequests();

        //Start recovering stamina
        playerRefs.playerStamina.StartRecovering();

        playerRefs.movement2.SetMovementSpeed(SpeedsEnum.Regular);
    }
    
    public override void Update()
    {
        base.Update();
        // Check walking animations
        float InputMagnitude = InputDetector.Instance.MovementDirectionInput.sqrMagnitude;

        if (InputMagnitude < 0.1f && isAnimationWalking)
        {
            animator.CrossFade(AnimatorStateName_Still, transitionTime_long);
            isAnimationWalking = false;
        }
        else if (InputMagnitude > 0.1f && !isAnimationWalking)
        {
            animator.CrossFade(AnimatorStateName_Walking, transitionTime_long);
            isAnimationWalking = true;
        }
    }
    public override void OnDisable()
    {
       unsubscribeToRequests();

        playerRefs.playerStamina.StopRecovering();
    }

}
