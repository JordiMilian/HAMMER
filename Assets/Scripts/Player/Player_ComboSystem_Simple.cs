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

    
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Player_Controller>();
        chargeAttack = GetComponent<Player_ChargeAttack>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            player.RestartDamage();

            switch(nextAttack)
            {
                case NextAttack.NextAttack01:
                    if (!animator.GetBool(Attack01_Charging))
                    {
                        animator.SetTrigger(Attack01_Charging); 
                    }
                    break;

                case NextAttack.NextAttack02:
                    if (!animator.GetBool(Attack02_Charging))
                    {
                        animator.SetTrigger(Attack02_Charging);
                    }
                    break;
            }
        }
        if(Input.GetKeyUp(KeyCode.M))
        {
            chargeAttack.isCharging = false;
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
    }
    public void ComboOver()
    {
        nextAttack = NextAttack.NextAttack01;
    }
   
}

