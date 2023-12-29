using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player_Roll : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator animator;
    public bool canDash = true;
    bool isWaitingDash;
    public bool isDashing;
    [SerializeField] float RollTime;
    [SerializeField] float RollMaxForce;
    [SerializeField] float RollCooldown;
    public VisualEffect GroundImpact;

    public AnimationCurve RollCurve;

    Player_ComboSystem_Simple comboSystem;
    Player_Controller playerController;

    [SerializeField] float InputDelayTime = 0.5f;
    float TimerDelay;
    bool IsWaitingInputDelay;
    bool isRunning = false;
    [SerializeField] float RunningSpeed = 40;
    [SerializeField] TrailRenderer rollTrail;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        comboSystem = GetComponent<Player_ComboSystem_Simple>();
        playerController = GetComponent<Player_Controller>();
    }

    
    void Update()
    {
        
        if (isRunning)
        {
            playerController.CurrentSpeed = RunningSpeed;
        }
        else { playerController.CurrentSpeed = playerController.BaseSpeed; }
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
    void DelayInput()
    {
        
        TimerDelay += Time.deltaTime;
       
        if(TimerDelay > InputDelayTime)
        {
            IsWaitingInputDelay = false;
            isRunning = true;
        }
    }
    
    IEnumerator Dash()
    {
        canDash = false;
        comboSystem.canAttack = false;
        isDashing = true;
        comboSystem.IsAttackCanceled = true;
        GroundImpact.Play();
        animator.SetTrigger("Roll");


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
    public void EV_ShowRollTrail() { rollTrail.enabled = true; }
    public void EV_HideRollTrail() { rollTrail.enabled = false; }
}
