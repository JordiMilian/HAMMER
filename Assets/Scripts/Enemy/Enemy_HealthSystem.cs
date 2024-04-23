using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : Generic_HealthSystem
{
    [SerializeField] Enemy_References enemyRefs;
    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject deadHead;
    [SerializeField] GameObject BloodCristals;
    [SerializeField] int AmountOfCristals;
    public override void Death(GameObject killer)
    {
        if(enemyRefs.stateMachine.CurrentState == Enemy_StateMachine.States.Dead) { Debug.Log("already dead"); return; }

        enemyRefs.genericEvents.OnDeath?.Invoke(this, new Generic_EventSystem.DeadCharacterInfo(gameObject, killer));
        if(BloodCristals != null) 
        { 
            for(int i = 0; i< AmountOfCristals;i++)
            {
                //Instantiate(BloodCristals, transform.position, Quaternion.identity);
            }
        }
        
        //Destroyed in the StateMachine
    }
}
