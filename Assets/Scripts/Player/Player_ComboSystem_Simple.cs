using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player_ComboSystem_Simple : MonoBehaviour
{
    string Attack01_Charging = "Attack01_Charging";
    string Attack02_Charging = "Attack02_Charging";
    string Attack01_Release = "Attack01_Release";
    string Attack02_Release = "Attack02_Release";
   
    private enum NextAttack { NextAttack01, NextAttack02,}
    [SerializeField] NextAttack nextAttack = NextAttack.NextAttack01;
    Player_ChargeAttack chargeAttack;

    Animator animator;
    Player_Controller player;
    Player_Roll playerRoll;

    public bool IsAttackCanceled;
    public bool canAttack;
   

    
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player_Controller>();
        chargeAttack = GetComponent<Player_ChargeAttack>();
        canAttack = true;
        playerRoll = GetComponent<Player_Roll>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Mouse0))
        {
                SetChargeTriggers();
        }  
        if(Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(!IsAttackCanceled)
            {
                if(canAttack)
                {
                    
                    SetReleaseTriggers();
                }

                if (!canAttack)
                {
                    StartCoroutine(WaitForCanAttackRelease());
                }
            }
           
        }
    }
    void SetChargeTriggers()
    {
        player.RestartDamage();
        IsAttackCanceled = false;

        switch (nextAttack)
        {
            case NextAttack.NextAttack01:
                animator.SetTrigger(Attack01_Charging);
                break;

            case NextAttack.NextAttack02:
                animator.SetTrigger(Attack02_Charging);
                break;
        }
    }
    void SetReleaseTriggers()
    {
        chargeAttack.isCharging = false;
        playerRoll.canDash = false;
        switch (nextAttack)
        {
            case NextAttack.NextAttack01:
                if (!animator.GetBool(Attack01_Release))
                {
                    animator.SetTrigger(Attack01_Release);
                    nextAttack = NextAttack.NextAttack02;
                }
                break;

            case NextAttack.NextAttack02:
                if (!animator.GetBool(Attack02_Release))
                {
                    animator.SetTrigger(Attack02_Release);
                    nextAttack = NextAttack.NextAttack01;
                }
                break;
        }
    }
    IEnumerator WaitForCanAttackCharge()
    {
        while (!canAttack)
        {
            yield return null;
        }
        SetChargeTriggers();
    }
    IEnumerator WaitForCanAttackRelease()
    {
        while (!canAttack)
        {
            yield return null;
        }
        SetReleaseTriggers();
    }
    public void ComboOver()
    {
        nextAttack = NextAttack.NextAttack01;
    }
    public void CanAttack()
    {
        canAttack = true;
    }
   
}

