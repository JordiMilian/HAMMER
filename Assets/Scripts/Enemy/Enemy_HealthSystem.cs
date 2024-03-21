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
    [SerializeField] List<GameObject> DeadParts = new List<GameObject>();

    public override void Death(GameObject killer)
    {
        //if (deadBody != null) { var DeadBody = Instantiate(deadBody, transform.position, Quaternion.identity); }
       // if (deadHead != null) { var DeadHead = Instantiate(deadHead, transform.position, Quaternion.identity); }
        if(BloodCristals != null) 
        { 
            for(int i = 0; i< AmountOfCristals;i++)
            {
                Instantiate(BloodCristals, transform.position, Quaternion.identity);
            }
            
        }
        
        foreach(GameObject part in DeadParts)
        {
            Vector2 direction = (transform.position - killer.transform.root.position).normalized;
            GameObject InstantiatedDeadPart = Instantiate(part, transform.root.position, Quaternion.identity);
            InstantiatedDeadPart.GetComponent<DeadPartV3>().CallVertical(direction, 1, 1);
        }
        
        if (eventSystem.OnDeath != null) eventSystem.OnDeath(this, new Generic_EventSystem.Args_DeadCharacter(gameObject, killer));
        TimeScaleEditor.Instance.SlowMotion(80, 1.2f);
        Destroy(gameObject);
    }

}
