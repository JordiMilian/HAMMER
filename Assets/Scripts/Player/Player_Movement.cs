using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player_Movement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator Player_Animator;
    Player_ComboSystem comboSystem;

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

    public EventHandler OnPerformRoll;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Player_Animator = GetComponent<Animator>();
        comboSystem = GetComponent<Player_ComboSystem>();
        CurrentSpeed = BaseSpeed;
    }

    
    void Update()
    {
        var input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        Move(input);

        if (isRunning)
        {
            CurrentSpeed = RunningSpeed;
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
            isRunning = false;
            CurrentSpeed = BaseSpeed;
            if(IsWaitingInputDelay)
            {
                IsWaitingInputDelay = false;
                switch (canDash)
                {
                    case true:
                        if ((Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) != (0, 0))
                        {
                            StartCoroutine(Dash());
                        }
                        break;
                    case false:
                        if (!isWaitingDash) { StartCoroutine(WaitForCanDash()); }

                        break;

                }
            } 
        }
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
                Player_Animator.SetBool("Walking", true);
            }
            else
            {
                Player_Animator.SetBool("Walking", false);
            }
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        comboSystem.canAttack = false;
        isDashing = true;
        comboSystem.IsAttackCanceled = true;
        if(OnPerformRoll != null) OnPerformRoll(this, EventArgs.Empty);
        Player_Animator.SetTrigger("Roll");


        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;

        float time = 0;
        float weight = 0;
        while (time < RollTime)
        {
            time = time + Time.deltaTime;
            weight = RollCurve.Evaluate(time/RollTime);
            rigidbody.AddForce(Axis * RollMaxForce * weight* Time.deltaTime);
            yield return null;
        }
        isDashing = false;
        yield return new WaitForSeconds(RollCooldown);
        canDash = true;

    }
    IEnumerator WaitForCanDash()
    {
        isWaitingDash = true;
        while (!canDash) 
        {
            yield return null;
        }
        isWaitingDash = true;
        StartCoroutine(Dash());
    }
    public void EV_CantDash() { canDash = false; }
    public void EV_CanDash() { canDash = true; }
    public void EV_HidePlayerCollider() { damageCollider.enabled = false; }
    public void EV_ShowPlayerCollider() { damageCollider.enabled = true; }
}
