using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbody;
    [SerializeField] Animator player_Animator;
    //Player_ComboSystem comboSystem;
    [SerializeField] Player_ComboSystem_chargeless comboSystem;

    [Header("BASE MOVEMENT")]
    public float CurrentSpeed;
    public float BaseSpeed;

    [Header("RUN")]
    [SerializeField] float RunningSpeed = 40;
    [SerializeField] float RunInputDelayTime = 0.5f;
    float TimerDelay;
    bool IsWaitingInputDelay;
    bool isRunning = false;


    [Header("ROLL")]
    public bool canDash = true;
    bool isWaitingDash;
    public bool isDashing;
    [SerializeField] float RollTime;
    [SerializeField] float RollMaxForce;
    [SerializeField] float RollCooldown;
    public AnimationCurve RollCurve;
    [SerializeField] Collider2D damageCollider;

    [SerializeField] Player_EventSystem eventSystem;
    [SerializeField] FloatVariable playerStamina;
    [SerializeField] Player_ActionPerformer actionPerformer;

    private void OnEnable()
    {
        eventSystem.OnPerformRoll += CallDashMovement;
    }
    private void OnDisable()
    {
        eventSystem.OnPerformRoll -= CallDashMovement;
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        player_Animator = GetComponent<Animator>();
        
        CurrentSpeed = BaseSpeed;
    }

    
    void Update()
    {
        Vector2 input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        Move(input);

        if (isRunning)
        {
            CurrentSpeed = RunningSpeed;
            player_Animator.SetBool("Running", true);
        }
       
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space))
        {
            IsWaitingInputDelay = true;
            TimerDelay = 0;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Space))
        {
            DelayInput();
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.Space))
        {
            //if ((Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) == (0, 0)) { return; }
            if(playerStamina.Value <= 0) { return; }

            StopRunning();

            if (IsWaitingInputDelay)
            {
                IsWaitingInputDelay = false;
                actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Roll"));
            } 
        }
    }
    void StopRunning()
    {
        isRunning = false;
        CurrentSpeed = BaseSpeed;
        player_Animator.SetBool("Running", false);
    }
    void Move(Vector2 vector2)
    {
        rigidbody.AddForce(vector2.normalized * CurrentSpeed * Time.deltaTime * 100);
        WalkingAnimation();
    }
  
    void DelayInput()
    { 
        TimerDelay += Time.deltaTime;
       
        if(TimerDelay > RunInputDelayTime)
        {
            IsWaitingInputDelay = false;
            isRunning = true;
        }

    }
    void WalkingAnimation()
    {
        if (!isDashing)
        {
            if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
            {
                player_Animator.SetBool("Walking", true);
            }
            else
            {
                player_Animator.SetBool("Walking", false);
            }
        }
    }
    void CallDashMovement()
    {
        //Call the event to remove Stamina
        eventSystem.OnStaminaAction?.Invoke(this, new Player_EventSystem.EventArgs_StaminaConsumption(1f));

        //Find the direction. If there is no direction, return???? maybe nose
        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;
        if (Axis.magnitude == 0) {  return; }
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
            rigidbody.AddForce(direction * RollMaxForce * weight* Time.deltaTime);
            yield return null;
        }
    }
    
    public void EV_SlowDownSpeed() { CurrentSpeed /= 2; }
    public void EV_ReturnSpeed() { CurrentSpeed = BaseSpeed; }
    public void EV_CantDash() { canDash = false; }
    public void EV_CanDash() { canDash = true; }
    public void EV_HidePlayerCollider() { damageCollider.enabled = false; }
    public void EV_ShowPlayerCollider() { damageCollider.enabled = true; }
}
