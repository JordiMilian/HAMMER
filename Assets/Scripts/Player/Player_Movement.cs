using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;

    [Header("BASE MOVEMENT")]
    public float CurrentSpeed;
    public float BaseSpeed;
    public float velocityMultiplier = 1;

    [Header("RUN")]
    [SerializeField] float RunningSpeed = 40;
    [SerializeField] float RunInputDelayTime = 0.5f;
    float TimerDelay;
    bool IsWaitingInputDelay;
    bool isRunning = false;

    [Header("ROLL")]
    [SerializeField] float RollTime;
    [SerializeField] float RollDistance;
    [SerializeField] bool updateAverageSize_trigger;
    float rollCurve_averageValue = 0;
    public AnimationCurve RollCurve;


    private void OnEnable()
    {
        Physics.IgnoreLayerCollision(15, 16);
        playerRefs.events.OnPerformRoll += CallDashMovement;
        playerRefs.events.OnDeath += StopRunningOnDeath;
        playerRefs.events.OnAttackStarted += StopRunning;
        playerRefs.events.OnEnterIdle += EV_ReturnSpeed;

        InputDetector.Instance.OnRollPressed += OnRollPressed;
        InputDetector.Instance.OnRollPressing += OnRollPressing;
        InputDetector.Instance.OnRollUnpressed += OnRollUnpressed;
        rollCurve_averageValue = UsefullMethods.GetAverageValueOfCurve(RollCurve, 10);
    }
    private void OnDisable()
    {
        playerRefs.events.OnPerformRoll -= CallDashMovement;
        playerRefs.events.OnDeath -= StopRunningOnDeath;
        playerRefs.events.OnAttackStarted -= StopRunning;
        playerRefs.events.OnEnterIdle -= EV_ReturnSpeed;

        InputDetector.Instance.OnRollPressed -= OnRollPressed;
        InputDetector.Instance.OnRollPressing -= OnRollPressing;
        InputDetector.Instance.OnRollUnpressed -= OnRollUnpressed;
    }
    void Start()
    {
        CurrentSpeed = BaseSpeed;
    }

    
    void Update()
    {
        Move(InputDetector.Instance.MovementDirectionInput);

        if (isRunning)
        {
            CurrentSpeed = RunningSpeed;
            playerRefs.animator.SetBool("Running", true);
        }
        if(updateAverageSize_trigger)
        {
            rollCurve_averageValue = UsefullMethods.GetAverageValueOfCurve(RollCurve, 10);
        }
    }
    void OnRollPressed()
    {
        IsWaitingInputDelay = true;
        TimerDelay = 0;
    }
    void OnRollPressing()
    {
        if (IsWaitingInputDelay)
        {
            TimerDelay += Time.deltaTime;

            if (TimerDelay > RunInputDelayTime)
            {
                IsWaitingInputDelay = false;
                isRunning = true;
            }
        }
    }
    void OnRollUnpressed()
    {
        if (playerRefs.currentStats.CurrentStamina <= 0) { return; }

        StopRunning();

        if (IsWaitingInputDelay)
        {
            IsWaitingInputDelay = false;

            //Multiply the looking direction with the Input direction:
            //If they coincide, the direction will be 1 and player is looking forward, else its -1 and its looking backwards
            //
            //Esto es una cutrada ficarho aqui pero weno funcione
            int direction = UsefullMethods.normalizeFloat(Input.GetAxisRaw("Horizontal")) * playerRefs.spriteFliper.lookingDirection;
            playerRefs.animator.SetInteger("LookingDirection", direction);

            playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Roll"));
        }
    }
    void StopRunningOnDeath(object sender, Generic_EventSystem.DeadCharacterInfo args) { StopRunning(); }
    public void StopRunning()
    {
        isRunning = false;
        CurrentSpeed = BaseSpeed;
        playerRefs.animator.SetBool("Running", false);
    }
    void Move(Vector2 vector2)
    {
        //playerRefs._rigidbody.AddForce(vector2.normalized * CurrentSpeed * Time.deltaTime * 100 * velocityMultiplier);
        playerRefs.characterMover.MovementVectorsPerSecond.Add(vector2.normalized * CurrentSpeed * velocityMultiplier);
        WalkingAnimation();
    }
  
    void WalkingAnimation()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
        {
            playerRefs.animator.SetBool("Walking", true);
        }
        else
        {
            playerRefs.animator.SetBool("Walking", false);
        }
    }
    void CallDashMovement()
    {
        //Call the event to remove Stamina
        playerRefs.events.CallStaminaAction?.Invoke(1.25f);

        //Find the direction. If there is no direction, return???? maybe nose
        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;

        playerRefs.spriteFliper.canFlip = false; //sprite can not flip during roll

        //If the player is not imputing a direction, rotate to the oposite of the sword
        if (Axis.magnitude == 0) 
        {
            Vector2 opositeDirectionToSword = -playerRefs.followMouse.SwordDirection;
            StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
             playerRefs.characterMover,
             RollDistance,
             RollTime,
             opositeDirectionToSword,
             RollCurve,
             rollCurve_averageValue
             ));
            return;
        }

        //Else roll towards imput direction
        //StartCoroutine(DashMovement(Axis));
        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
            playerRefs.characterMover,
            RollDistance,
            RollTime,
            Axis,
            RollCurve,
            rollCurve_averageValue
            ));
    }
    IEnumerator DashMovement(Vector2 direction)
    {
        float time = 0;
        float weight = 0;
        while (time < RollTime)
        {
            time = time + Time.deltaTime;
            weight = RollCurve.Evaluate(time/RollTime);
            //playerRefs._rigidbody.AddForce(direction * RollMaxForce * weight* Time.deltaTime * velocityMultiplier);
            playerRefs.characterMover.MovementVectorsPerSecond.Add(direction * RollDistance * weight * velocityMultiplier);
            yield return null;
        }
        playerRefs.spriteFliper.canFlip = true; //sprite can flip after roll
    }
    
    public void EV_SlowDownSpeed() { CurrentSpeed /= 5; }
    public void EV_ReturnSpeed() { CurrentSpeed = BaseSpeed; }
    public void EV_HidePlayerCollider() { gameObject.layer = 15; playerRefs.damageDetectorCollider.enabled = false; }
    public void EV_ShowPlayerCollider() { gameObject.layer = 20; playerRefs.damageDetectorCollider.enabled = true; }
}
