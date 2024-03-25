using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : Generic_HealthSystem
{

    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject deadHead;
    [SerializeField] GameObject BloodCristals;
    [SerializeField] int AmountOfCristals;
    [SerializeField] Enemy_StateMachine stateMachine;
    public override void Death(GameObject killer)
    {
        if(stateMachine.CurrentState == Enemy_StateMachine.States.Dead) { Debug.Log("already dead"); return; }

        eventSystem.OnDeath?.Invoke(this, new Generic_EventSystem.DeadCharacterInfo(gameObject, killer));
        //if (deadBody != null) { var DeadBody = Instantiate(deadBody, transform.position, Quaternion.identity); }
        //if (deadHead != null) { var DeadHead = Instantiate(deadHead, transform.position, Quaternion.identity); }
        if(BloodCristals != null) 
        { 
            for(int i = 0; i< AmountOfCristals;i++)
            {
                Instantiate(BloodCristals, transform.position, Quaternion.identity);
            }
        }
        
        //Destroyed in the StateMachine
    }
}
