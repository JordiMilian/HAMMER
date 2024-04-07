using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Generic_OnTriggerEnterEvents;

public class EnemyGenerator : BaseRoomLogic
{
    [Serializable]
    public class EnemySpawn
    {
        public GameObject PrefabEnemy;
        public int Weight;
        public float minInstances;
        public float maxInstances;
        [HideInInspector] public float currentInstances;
    }
    [SerializeField] int MaxWeight;
    public List<EnemySpawn> SpawneableEnemies = new List<EnemySpawn>();
    [HideInInspector] public List<GameObject> CurrentlySpawnedEnemies = new List<GameObject>();

    [SerializeField] Generic_OnTriggerEnterEvents SpawnTrigger;
    [SerializeField] Collider2D SpawnArea;
    [SerializeField] GameObject EnemiesContainer;
    int EnemiesAlive;
    bool areCorrectlySpawned;
    Coroutine correctlySpawnedCoroutine;
    [HideInInspector] public bool reenteredRoom;

    public override void OnEnable()
    {
        base.OnEnable();

        SpawnTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        SpawnTrigger.OnTriggerEntered += SpawnEnemies;
        
    }
    private void Start()
    {
        
    }
    void SpawnEnemies(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {

        if (isRoomPermanentlyCompleted) { return; }
        if (areCorrectlySpawned) { return; }
        if (SpawneableEnemies.Count == 0 || MaxWeight == 0)
        {
            RoomCompleted(false,false);
            Debug.LogWarning("Nothing to spawn");
            return;
        }

        reenteredRoom = true;
        //Destroy the remaining enemies
        for (int i = CurrentlySpawnedEnemies.Count - 1; i >= 0; i--)
        {
            Destroy(CurrentlySpawnedEnemies[i]);
            CurrentlySpawnedEnemies.Remove(CurrentlySpawnedEnemies[i]);
        }
        int currentWeight = 0;

        foreach (EnemySpawn spawn in SpawneableEnemies)
        {
            //Restart counters of spawners
            spawn.currentInstances = 0;

            //Spawn the minimums
            for (int i = 0; i < spawn.minInstances; i++)
            {
                ActuallySpawn(spawn);
                currentWeight += spawn.Weight;
            }
        }
        int attemptsToSpawn = 0;
        while (currentWeight < MaxWeight)
        {
            attemptsToSpawn++;
            if (attemptsToSpawn == 30)
            {
                Debug.LogError("Something wrong with Spawners, check min-max stuff");
                break;
            }
            //Pick a random index
            int randomIndex = UnityEngine.Random.Range(0, SpawneableEnemies.Count);
            EnemySpawn thisSpawn = SpawneableEnemies[randomIndex];

            //If already maxed, repeat
            if (thisSpawn.currentInstances >= thisSpawn.maxInstances) { continue; }

            //Spawn and add Weight
            ActuallySpawn(thisSpawn);
            currentWeight += thisSpawn.Weight;
        }
        EnemiesAlive = CurrentlySpawnedEnemies.Count;
        areCorrectlySpawned = true;

        //not correctly spawned after a delay
        if (correctlySpawnedCoroutine != null) { StopCoroutine(correctlySpawnedCoroutine); }
        correctlySpawnedCoroutine = StartCoroutine(NotCorrectlySpawnedTimer());

    }
    void ActuallySpawn(EnemySpawn spawn)
    {
        // Find random point and Instantiate the Enemy
        Vector2 SpawnPosition = RandomPointInCollider(SpawnArea);
        GameObject SpawnedEnemy = Instantiate(
          spawn.PrefabEnemy,
          SpawnPosition,
          Quaternion.identity,
          EnemiesContainer.transform);

        CurrentlySpawnedEnemies.Add(SpawnedEnemy);

        //Subscribe to everything
        Generic_EventSystem enemyEvent = SpawnedEnemy.GetComponent<Generic_EventSystem>();
        enemyEvent.OnDeath += EnemyDied;
        enemyEvent.OnReceiveDamage += EnemyDamaged;

        spawn.currentInstances++;

    }
    IEnumerator NotCorrectlySpawnedTimer()
    {
        yield return new WaitForSeconds(5);
        areCorrectlySpawned = false;
    }
    void EnemyDied(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        EnemiesAlive--;
        areCorrectlySpawned = false;
        if (EnemiesAlive <= 0) { RoomCompleted(true,false); }
    }
    void EnemyDamaged(object sender, Generic_EventSystem.ReceivedAttackInfo args)
    {
        areCorrectlySpawned = false;
    }

    Vector2 RandomPointInCollider(Collider2D collider)
    {
        Vector2 randomPoint = Vector2.zero;
        int attempts = 0;
        do
        {
            float x = UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x);
            float y = UnityEngine.Random.Range(collider.bounds.min.y, collider.bounds.max.y);
            randomPoint = new Vector2(x, y);
            attempts++;
        }
        while (!collider.OverlapPoint(randomPoint));
        return randomPoint;
    }
}


