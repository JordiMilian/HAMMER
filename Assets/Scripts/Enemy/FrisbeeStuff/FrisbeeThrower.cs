using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeThrower : Enemy_BaseProjectileCreator
{
    [SerializeField] GameObject FrisbeePrefab;

    [SerializeField] Transform SpawnPos;
    [SerializeField] bool throwTriggerTest;
    [SerializeField] Animator enemyAnimator;
    public void EV_ThrowFrisbee()
    {
        UpdateVectorData();
        Vector2 spawnPos = SpawnPos.position;
        Vector2 directionToPlayerFromSpawn = (playerPosition - spawnPos).normalized;
        GameObject newFrisbee = Instantiate(FrisbeePrefab, SpawnPos.position, Quaternion.identity);
        FrisbeeController frisbeeController = newFrisbee.GetComponent<FrisbeeController>();
        frisbeeController.throwFrisbee(directionToPlayerFromSpawn, SpawnPos);
        frisbeeController.onReturnedFrisbee += onFrisbeeReturned;
    }
    void onFrisbeeReturned()
    {
        enemyAnimator.SetTrigger("PickUpFrisbee");
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
