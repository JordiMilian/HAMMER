using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeThrower : Enemy_BaseProjectileCreator
{
    [SerializeField] GameObject FrisbeePrefab;

    [SerializeField] Transform SpawnPos;
    [SerializeField] bool throwTriggerTest;
    [SerializeField] Animator enemyAnimator;
    FrisbeeController lastFrisbee;
    private void OnDisable()
    {
        if (lastFrisbee != null) { lastFrisbee.onReturnedFrisbee -= onFrisbeeReturned; }
    }
    public void EV_ThrowFrisbee()
    {
        UpdateVectorData();
        Vector2 spawnPos = SpawnPos.position;
        Vector2 directionToPlayerFromSpawn = (playerPosition - spawnPos).normalized;
        GameObject newFrisbee = Instantiate(FrisbeePrefab, SpawnPos.position, Quaternion.identity);
        lastFrisbee = newFrisbee.GetComponent<FrisbeeController>();
        lastFrisbee.throwFrisbee(directionToPlayerFromSpawn, SpawnPos);
        lastFrisbee.onReturnedFrisbee += onFrisbeeReturned;
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
