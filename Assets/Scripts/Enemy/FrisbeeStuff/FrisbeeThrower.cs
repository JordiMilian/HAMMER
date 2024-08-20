using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeThrower : Enemy_BaseProjectileCreator
{
    [SerializeField] GameObject FrisbeePrefab;

    [SerializeField] Transform SpawnPos;
    [SerializeField] bool throwTriggerTest;
    public void EV_ThrowFrisbee()
    {
        UpdateVectorData();

        GameObject newFrisbee = Instantiate(FrisbeePrefab, SpawnPos.position, Quaternion.identity);
        newFrisbee.GetComponent<FrisbeeController>().throwFrisbee(weaponDirection);
    }
    private void Update()
    {
        if(throwTriggerTest)
        {
            EV_ThrowFrisbee();
            throwTriggerTest = false;
        }
    }
}
