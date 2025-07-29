using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState
{
    [SerializeField] string AnimatorStateName_Still;
    [SerializeField] string AnimatorStateName_Walking;
    [SerializeField] float delayBeforeRecoveringStamina = 0.5f;
    bool isAnimationWalking = false;
    Coroutine delayStartRecovery;
    public override void OnEnable()
    {
        stateMachine.EV_ReturnInput();
        stateMachine.EV_CanTransition();

        animator.CrossFade(AnimatorStateName_Still, transitionTime_long);
        isAnimationWalking = false;

        subscribeToRequests();

        //Start recovering stamina
        if(delayStartRecovery != null)
        {
            StopCoroutine(delayStartRecovery);
        }
        delayStartRecovery = StartCoroutine(delayAndRecoverStamina());

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Regular);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Regular);

        //
        IEnumerator delayAndRecoverStamina()
        {
            yield return new WaitForSeconds(delayBeforeRecoveringStamina);
            playerRefs.playerStamina.StartRecovering();
        }
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
        if(delayStartRecovery != null)
        {
            StopCoroutine(delayStartRecovery);
            delayStartRecovery = null;
        }

        playerRefs.playerStamina.StopRecovering();
    }

}
