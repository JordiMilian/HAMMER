using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingProjectile_Spawner : MonoBehaviour
{
    [SerializeField] Transform SpawningOriginPos;
    [SerializeField] GameObject ChaserController_Prefab;
  
    [Header("Spread")]
    [SerializeField] bool SpreadSpawnTrigger;
    [SerializeField] int amountToSpread;
    [SerializeField] float angleDegToSpread;
    [SerializeField] float DistanceFromCenterOnSpawn;
    [SerializeField] float delayBetweenProjectiles;


    private void Update()
    {
        if(SpreadSpawnTrigger || Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(SpreadSpawnCoroutine(999));
            SpreadSpawnTrigger = false;
        }
    }
    IEnumerator SpreadSpawnCoroutine(int amount) //Decide amount from here millor
    {
        Transform playerTf = GlobalPlayerReferences.Instance.playerTf;
        Vector2 directionToPlayer = (playerTf.position - transform.position).normalized;
        Vector2[] SpreadDirections = UsefullMethods.GetSpreadDirectionsFromCenter(directionToPlayer, amountToSpread, Mathf.Deg2Rad * angleDegToSpread);

        List<ChasingProjectile_Controller> chasersList = new List<ChasingProjectile_Controller>();
        for (int p = 0; p < amountToSpread; p++)
        {
            Vector2 newPosition = (Vector2)transform.position + (SpreadDirections[p] * DistanceFromCenterOnSpawn);
            GameObject newChaserGO = Instantiate(ChaserController_Prefab, newPosition, Quaternion.identity);
            newChaserGO.transform.up = SpreadDirections[p];
            chasersList.Add(newChaserGO.GetComponent<ChasingProjectile_Controller>());
            yield return new WaitForSeconds(delayBetweenProjectiles);
        }
        //yield return new WaitForSeconds(1);
        for (int c = 0; c < amountToSpread; c++)
        {
            chasersList[c].startChasing(playerTf);
            yield return new WaitForSeconds(delayBetweenProjectiles);
        }
    }
}
