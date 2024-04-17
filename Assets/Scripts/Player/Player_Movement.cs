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

    [Header("RUN")]
    [SerializeField] float RunningSpeed = 40;
    [SerializeField] float RunInputDelayTime = 0.5f;
    float TimerDelay;
    bool isDelaying;
    bool IsWaitingInputDelay;
    bool isRunning = false;


    [Header("ROLL")]
    public bool canDash = true;
    public bool isDashing;
    [SerializeField] float RollTime;
    [SerializeField] float RollMaxForce;
    [SerializeField] float RollCooldown;
    public AnimationCurve RollCurve;


    private void OnEnable()
    {
        playerRefs.events.OnPerformRoll += CallDashMovement;
        playerRefs.events.OnDeath += StopRunningOnDeath;
        playerRefs.events.OnPerformAttack += StopRunning;
    }
    private void OnDisable()
    {
        playerRefs.events.OnPerformRoll -= CallDashMovement;
        playerRefs.events.OnDeath -= StopRunningOnDeath;
        playerRefs.events.OnPerformAttack -= StopRunning;
    }
    void Start()
    {
        CurrentSpeed = BaseSpeed;
    }

    
    void Update()
    {
        Vector2 input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        Move(input);

        if (isRunning)
        {
            CurrentSpeed = RunningSpeed;
            playerRefs.animator.SetBool("Running", true);
        }
       
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space))
        {
            IsWaitingInputDelay = true;
            TimerDelay = 0;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
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
        
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.Space))
        {
            //if ((Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) == (0, 0)) { return; }
            if(playerRefs.currentStamina.Value <= 0) { return; }

            StopRunning();

            if (IsWaitingInputDelay)
            {
                IsWaitingInputDelay = false;
                playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Roll"));
            } 
        }
    }
    void StopRunningOnDeath(object sender, Generic_EventSystem.DeadCharacterInfo args) { StopRunning(); }
    void StopRunning()
    {
        isRunning = false;
        CurrentSpeed = BaseSpeed;
        playerRefs.animator.SetBool("Running", false);
    }
    void Move(Vector2 vector2)
    {
        playerRefs._rigidbody.AddForce(vector2.normalized * CurrentSpeed * Time.deltaTime * 100);
        WalkingAnimation();
    }
  
    void WalkingAnimation()
    {
        if (!isDashing)
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
    }
    void CallDashMovement()
    {
        //Call the event to remove Stamina
        playerRefs.events.OnStaminaAction?.Invoke(1f);

        //Find the direction. If there is no direction, return???? maybe nose
        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;
        if (Axis.magnitude == 0) {  return; }
        if(Input.GetAxisRaw("Horizontal") >= 0) { }
        playerRefs.spriteFliper.canFlip = false; //sprite can not flip during roll
        StartCoroutine(DashMovement(Axis));
    }
    IEnumerator DashMovement(Vector2 direction)
    {
        float time = 0;
        float weight = 0;
        while (time < RollTime)
        {
            time = time + Time.deltaTime;
            weight = RollCurve.Evaluate(time/RollTime);
            playerRefs._rigidbody.AddForce(direction * RollMaxForce * weight* Time.deltaTime);
            yield return null;
        }
        playerRefs.spriteFliper.canFlip = true; //sprite can flip after roll
    }
    
    public void EV_SlowDownSpeed() { CurrentSpeed /= 2; }
    public void EV_ReturnSpeed() { CurrentSpeed = BaseSpeed; }
    public void EV_CantDash() { canDash = false; }
    public void EV_CanDash() { canDash = true; }
    public void EV_HidePlayerCollider() { playerRefs.damageDetectorCollider.enabled = false; }
    public void EV_ShowPlayerCollider() { playerRefs.damageDetectorCollider.enabled = true; }
}
