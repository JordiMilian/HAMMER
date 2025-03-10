using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System.Security.Cryptography.X509Certificates;

public class PlayerState_Rolling : PlayerState
{

    [SerializeField] float RollTime; //This should depend on the animation I think?
    [SerializeField] float RollDistance;
    [SerializeField] bool updateAverageSize_trigger;
    float rollCurve_averageValue = 0;

    [SerializeField] AnimationCurve RollCurve;
    Coroutine rollCoroutine, rollAnimationCoroutine;
    public override void OnEnable()
    {
        playerRefs.spriteFliper.canFlip = false; //sprite can not flip during roll

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.VerySlow);

        //Handle which direction it's facing and play proper animation

        PerformRollMovement();

        rollAnimationCoroutine = StartCoroutine(RollAutoTransition());
        subscribeToRequests();
    }
    
    void PerformRollMovement()
    {
        //Maybe InputDetector should be involced in this??
        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;

        Vector2 directionToRoll = Axis;

        //If the player is not imputing a direction, rotate to the oposite of the sword
        if (Axis.magnitude < 0.1f)
        {
            Vector2 opositeDirectionToSword = -playerRefs.followMouse.SwordDirection;
            directionToRoll = opositeDirectionToSword;
        }

        rollCoroutine = StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            RollDistance,
            RollTime,
            directionToRoll,
            RollCurve,
            rollCurve_averageValue
            ));
    }
    IEnumerator RollAutoTransition()
    {
        bool isPressingRun = false;

        InputDetector.Instance.OnRollPressing += pressing;
        InputDetector.Instance.OnRollUnpressed += unpressed;

        animator.CrossFade(AnimatorStateName, 0.1f);
        AnimationClip thisClip = UsefullMethods.GetAnimationClipByStateName(AnimatorStateName, animator);
        yield return StartCoroutine(UsefullMethods.WaitForAnimationTime(thisClip));

        if (isPressingRun) { stateMachine.ForceChangeState(playerRefs.RunningState); }
        else { stateMachine.ForceChangeState(playerRefs.IdleState); }

        InputDetector.Instance.OnRollPressing -= pressing;
        InputDetector.Instance.OnRollUnpressed -= unpressed;

        //
        void pressing() { isPressingRun = true; }
        void unpressed() { isPressingRun = false; }
    }
    

    public override void OnDisable()
    {
        if(rollCoroutine != null) { StopCoroutine(rollCoroutine); }
        if(rollAnimationCoroutine != null) { StopCoroutine(rollAnimationCoroutine); }

        playerRefs.movement2.SetMovementSpeed(MovementSpeeds.Regular);

        unsubscribeToRequests();
    }
    #region ACTION REQUESTS

    protected override void RequestAttack() { stateMachine.RequestChangeState(playerRefs.RollingAttackState); }

    #endregion

}
