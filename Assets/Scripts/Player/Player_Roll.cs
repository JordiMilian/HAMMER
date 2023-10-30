using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Roll : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator animator;
    public bool canDash = true;
    public bool isDashing;
    [SerializeField] float RollTime;
    [SerializeField] float RollMaxForce;
    [SerializeField] float RollCooldown;

    public AnimationCurve RollCurve;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space))
        {
            if (canDash == true)
            {
                StartCoroutine(Dash());
              
            }
        }
    }
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float time = 0;
        float weight = 0;
        Vector2 Axis = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical")).normalized;
        animator.SetTrigger("Roll");
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
}
