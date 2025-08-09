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

    public bool isRollAttackUnlocked = false;

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

        checkForRunning_Coroutine = StartCoroutine(AutoTransitionToStateOnAnimationOver(stateName, playerRefs.IdleState, transitionTime_short));

        InputDetector.Instance.OnRollPressing += pressing;
        InputDetector.Instance.OnRollUnpressed += unpressed;
    }
    public override void OnDisable()
    {
        if (rollMovementCoroutine != null) { StopCoroutine(rollMovementCoroutine); }
        if (rollAnimationCoroutine != null) { StopCoroutine(rollAnimationCoroutine); }
        if (checkForRunning_Coroutine != null) { StopCoroutine(checkForRunning_Coroutine); }

        playerRefs.movement.SetMovementSpeed(SpeedsEnum.Regular);
        playerRefs.spriteFliper.canFlip = true;
        playerRefs.animationEvents.EV_ShowPlayerCollider();


        InputDetector.Instance.OnRollPressing -= pressing;
        InputDetector.Instance.OnRollUnpressed -= unpressed;

        base.OnDisable();
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
    void pressing()
    {
        stateMachine.RequestChangeState(playerRefs.RunningState);
    }
    void unpressed()
    {
        stateMachine.CancelRequest(playerRefs.RunningState);
    }

    #region ACTION REQUESTS

    protected override void RequestAttack() 
    { 
        if(isRollAttackUnlocked)
        {
            stateMachine.RequestChangeState(playerRefs.RollingAttackState);
        }
        else
        {
            stateMachine.RequestChangeState(playerRefs.StartingComboAttackState);
        }
    }

    #endregion

}
