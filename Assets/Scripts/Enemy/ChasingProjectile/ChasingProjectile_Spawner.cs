using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingProjectile_Spawner : MonoBehaviour
{
    [SerializeField] Transform SpawningOriginPos;
    [SerializeField] GameObject ChaserController_Prefab;
    List<ChasingProjectile_Controller> chasersList = new List<ChasingProjectile_Controller>();

    [Header("Testign")]
    [SerializeField] bool SpawnTrigger;
    [SerializeField] int amountToSpawn;
    [SerializeField] float startingBoostVelocity;
    [SerializeField] float startingBoostDuration;
    [SerializeField] float DistanceFromCenterOnSpawn;


    private void Update()
    {
        if(SpawnTrigger)
        {
            StartCoroutine(MultiSpawnCoroutine());

            SpawnTrigger = false;
        }
    }
    IEnumerator MultiSpawnCoroutine()
    {
        foreach(ChasingProjectile_Controller chaser in chasersList) { Destroy(chaser.gameObject); } 

        Vector2[] CIrclePositions = UsefullMethods.GetPolygonPositions(SpawningOriginPos.position, amountToSpawn, DistanceFromCenterOnSpawn);
        chasersList = new List<ChasingProjectile_Controller>();
        for (int p = 0; p < amountToSpawn; p++)
        {
            GameObject newChaserGO = Instantiate(ChaserController_Prefab, CIrclePositions[p], Quaternion.identity);
            chasersList.Add(newChaserGO.GetComponent<ChasingProjectile_Controller>());
        }
        yield return new WaitForSeconds(1);
        for (int c = 0; c < amountToSpawn; c++)
        {
            Vector2 thisDirection = (CIrclePositions[c] - (Vector2)SpawningOriginPos.position).normalized;
            chasersList[c].StartingBoost(startingBoostVelocity, startingBoostDuration, thisDirection);
        }
    }
}
