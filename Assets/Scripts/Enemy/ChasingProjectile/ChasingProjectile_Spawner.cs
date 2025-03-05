using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingProjectile_Spawner : MonoBehaviour
{
    [SerializeField] Transform SpawningOriginTf;
    [SerializeField] GameObject ChaserController_Prefab;

    [Header("Spread")]
    [SerializeField] bool SpreadSpawnTrigger;
    [SerializeField] int amountToSpread;
    [SerializeField] float angleDegToSpread;
    [SerializeField] float DistanceFromCenterOnSpawn;
    [SerializeField] float delayBetweenProjectiles;
    [SerializeField] Transform weaponPivot;


    private void Update()
    {
        if (SpreadSpawnTrigger || Input.GetKeyDown(KeyCode.O))
        {
            EV_Chaser_SpreadSpawn(amountToSpread);
            SpreadSpawnTrigger = false;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            EV_Chaser_SingleSpawn();
        }
    }
    public void EV_Chaser_SingleSpawn()
    {
        Transform playerTf = GlobalPlayerReferences.Instance.playerTf;

        GameObject newChaserGO = Instantiate(ChaserController_Prefab, SpawningOriginTf.position, Quaternion.identity);
        ChasingProjectile_Controller controller = newChaserGO.GetComponent<ChasingProjectile_Controller>();
        Vector2 directionToPlayer = (playerTf.position - SpawningOriginTf.position).normalized;
        controller.SpawnDelayAndChase(directionToPlayer, playerTf, transform);

    }
    public void EV_Chaser_SpreadSpawn(int amount)
    {
        StartCoroutine(SpreadSpawnCoroutine(amount));
        
        //
        IEnumerator SpreadSpawnCoroutine(int amount) //Decide amount from here millor
        {
            Transform playerTf = GlobalPlayerReferences.Instance.playerTf;
            Vector2 weaponDirection = weaponPivot.up;
            Vector2[] SpreadDirections = UsefullMethods.GetSpreadDirectionsFromCenter(weaponDirection, amountToSpread, Mathf.Deg2Rad * angleDegToSpread);

            for (int p = 0; p < amountToSpread; p++)
            {
                Vector2 newPosition = (Vector2)transform.position + (SpreadDirections[p] * DistanceFromCenterOnSpawn);
                GameObject newChaserGO = Instantiate(ChaserController_Prefab, newPosition, Quaternion.identity);
                ChasingProjectile_Controller controller = newChaserGO.GetComponent<ChasingProjectile_Controller>();
                controller.SpawnDelayAndChase(SpreadDirections[p], GlobalPlayerReferences.Instance.playerTf, transform);
                yield return new WaitForSeconds(delayBetweenProjectiles);
            }

        }
    }
}
