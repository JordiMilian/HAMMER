using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : Generic_HealthSystem
{

    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject deadHead;
    [SerializeField] GameObject BloodCristals;
    [SerializeField] int AmountOfCristals;
    
    public override void Death()
    {
        if (deadBody != null) { var DeadBody = Instantiate(deadBody, transform.position, Quaternion.identity); }
        if (deadHead != null) { var DeadHead = Instantiate(deadHead, transform.position, Quaternion.identity); }
        if(BloodCristals != null) 
        { 
            for(int i = 0; i< AmountOfCristals;i++)
            {
                Instantiate(BloodCristals, transform.position, Quaternion.identity);
            }
            
        }

        Destroy(gameObject);
    }

}
