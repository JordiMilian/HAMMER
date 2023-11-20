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
    public bool isDashing;
    [SerializeField] float RollTime;
    [SerializeField] float RollMaxForce;
    [SerializeField] float RollCooldown;
    public VisualEffect GroundImpact;

    public AnimationCurve RollCurve;

    Player_ComboSystem_Simple comboSystem;

    float holdTime;
    bool InputIsHold;
    bool InputIsTap;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        comboSystem = GetComponent<Player_ComboSystem_Simple>();
    }

    
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space))
        {
            switch (canDash)
            {
                case true:
                    if ((Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) != (0, 0))
                    {
                        StartCoroutine(Dash());
                    }
                    break;
                case false:
                    StartCoroutine(WaitForCanDash());
                    break;

            }
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
        while (!canDash) 
        {
            yield return null;
        }
        Debug.Log("once");
        StartCoroutine(Dash());

    }
    public void EV_CantDash() { canDash = false; }
    
    public void EV_CanDash() { canDash = true; }
}
