using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerState_Rolling : PlayerState
{
    [SerializeField] string AnimatorStateName_frontRoll;
    [SerializeField] string AnimatorStateName_backRoll;
    [SerializeField] float RollTime; //This should depend on the animation I think?
    [SerializeField] float RollDistance;
    [SerializeField] bool updateAverageSize_trigger;
    [SerializeField] VisualEffect VFX_RollDust;
    [SerializeField] AudioClip SFX_RollSound;
    float rollCurve_averageValue = -1;

    [SerializeField] AnimationCurve RollCurve;
    Coroutine rollMovementCoroutine, rollAnimationCoroutine, checkForRunning_Coroutine;
    public override void OnEnable()
    {
        base.OnEnable();

        playerRefs.spriteFliper.canFlip = false; //sprite can not flip during roll

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.VerySlow);
        playerRefs.swordRotation.SetRotationSpeed(SpeedsEnum.Slow);

        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;

        //Handle which direction it's facing and play proper animation
        float DotProductWithFacingDirection = Vector2.Dot(Axis, playerRefs.spriteFliper.lookingVector);
        string stateName;
        if (DotProductWithFacingDirection >= 0) { stateName = AnimatorStateName_frontRoll; }
        else { stateName = AnimatorStateName_backRoll;}
        //rollAnimationCoroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(stateName,playerRefs.IdleState,transitionTime_short));

        PerformRollMovement(Axis);

        VFX_RollDust.Play();
        SFX_PlayerSingleton.Instance.playSFX(SFX_RollSound, 0.1f);

        //checkForRunning_Coroutine = StartCoroutine(checkForRunning(stateName,transitionTime_short));
        checkForRunning_Coroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(stateName, playerRefs.IdleState, transitionTime_short));
    }
    
    void PerformRollMovement(Vector2 direction)
    {
        //Maybe InputDetector should be involced in this??
        //If the player is not imputing a direction, rotate to the oposite of the sword
        if (direction.magnitude < 0.1f)
        {
            Vector2 opositeDirectionToSword = -playerRefs.swordRotation.SwordDirection;
            direction = opositeDirectionToSword;
        }

        if (rollCurve_averageValue < 0) { rollCurve_averageValue = UsefullMethods.GetAverageValueOfCurve(RollCurve, 10); }
        rollMovementCoroutine = StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            RollDistance,
            RollTime,
            direction,
            RollCurve,
            rollCurve_averageValue
            ));
    }
    IEnumerator checkForRunning(string thisAnimatorStateName, float normalizedTransitionDuration)
    {
        bool isPressingRun = false;

        InputDetector.Instance.OnRollPressing += pressing;
        InputDetector.Instance.OnRollUnpressed += unpressed;

        //the same as the AutoTransitionToState
        animator.CrossFadeInFixedTime(thisAnimatorStateName, normalizedTransitionDuration);

        yield return null; //wait one frame so the transition can start
        AnimatorClipInfo[] nextClips = animator.GetNextAnimatorClipInfo(0);
        if (nextClips.Length > 0)
        {
            AnimationClip nextClip = nextClips[0].clip;
            yield return StartCoroutine(UsefullMethods.WaitForAnimationTime(nextClip));
        }
        else { Debug.LogError("ERROR: No transition clip found"); }


        InputDetector.Instance.OnRollPressing -= pressing;
        InputDetector.Instance.OnRollUnpressed -= unpressed;
        if (isPressingRun) { stateMachine.ForceChangeState(playerRefs.RunningState); }
        else { stateMachine.ForceChangeState(playerRefs.IdleState); }


        //
        void pressing() { isPressingRun = true; }
        void unpressed() { isPressingRun = false; }
    }

    public override void OnDisable()
    {
        if(rollMovementCoroutine != null) { StopCoroutine(rollMovementCoroutine); }
        if(rollAnimationCoroutine != null) { StopCoroutine(rollAnimationCoroutine); }
        if(checkForRunning_Coroutine != null) { StopCoroutine(checkForRunning_Coroutine); }

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Regular);
        playerRefs.spriteFliper.canFlip = true;

        base.OnDisable();  
    }
    #region ACTION REQUESTS

    protected override void RequestAttack() { stateMachine.RequestChangeState(playerRefs.RollingAttackState); }

    #endregion

}
