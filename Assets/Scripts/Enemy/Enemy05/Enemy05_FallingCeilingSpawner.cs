using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy05_FallingCeilingSpawner : MonoBehaviour
{
    [SerializeField] GameObject fallingCeilingPrefab;
    [SerializeField] PolygonCollider2D spawnArea;
    [SerializeField] float minTimeBetweenSpawns = 1, maxTimeBetweenSpawns = 3;

    IRoomWithEnemies roomWithEnemies;
    [SerializeField] GameObject roomWithEnemiesObject;
    private void OnValidate()
    {
        UsefullMethods.CheckIfGameobjectImplementsInterface<IRoomWithEnemies>(ref roomWithEnemiesObject, ref roomWithEnemies);
    }
    private void OnEnable()
    {
        OnValidate();
        roomWithEnemies.OnEnemiesSpawned += StartSpawning;
        roomWithEnemies.OnAllEnemiesKilled += EndSpawning;
        GameEvents.OnPlayerDeath += EndSpawning;
    }
    private void OnDisable()
    {
        roomWithEnemies.OnEnemiesSpawned -= StartSpawning;
        roomWithEnemies.OnAllEnemiesKilled -= EndSpawning;
        GameEvents.OnPlayerDeath -= EndSpawning;
    }
    Coroutine spawnCoroutine;
    void StartSpawning() { if (spawnCoroutine != null) { return; } spawnCoroutine = StartCoroutine(spawnWithRandomDelay()); }
    void EndSpawning() { StopCoroutine(spawnCoroutine); }
    IEnumerator spawnWithRandomDelay()
    {
        float delayTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

        yield return new WaitForSeconds(delayTime);
        SpawnPrefab();

        spawnCoroutine = StartCoroutine(spawnWithRandomDelay());

        //
        void SpawnPrefab()
        {
            Vector2 randomPoint = UsefullMethods.RandomPointInCollider(spawnArea);
            GameObject fallingCeiling = Instantiate(fallingCeilingPrefab, randomPoint, Quaternion.identity, transform);
        }
    }
   
}
