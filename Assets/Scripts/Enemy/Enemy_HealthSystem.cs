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

    public override void Death(GameObject killer)
    {
        eventSystem.OnDeath?.Invoke(this, new Generic_EventSystem.Args_DeadCharacter(gameObject, killer));
        //if (deadBody != null) { var DeadBody = Instantiate(deadBody, transform.position, Quaternion.identity); }
        //if (deadHead != null) { var DeadHead = Instantiate(deadHead, transform.position, Quaternion.identity); }
        if(BloodCristals != null) 
        { 
            for(int i = 0; i< AmountOfCristals;i++)
            {
                Instantiate(BloodCristals, transform.position, Quaternion.identity);
            }
        }
        TimeScaleEditor.Instance.SlowMotion(80, 1.2f);
        StartCoroutine(DelayedDestruction());
    }

    //Cutrada maxima amb 2 delays wtf arreglaho
    IEnumerator DelayedDestruction()
    {
        yield return new WaitForSecondsRealtime(0.07f);
        Destroy(gameObject);
    }

}
