using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomWithEnemiesLogic : BaseRoomWithDoorLogic
{
    [Serializable]
    public class EnemySpawn
    {
        public GameObject PrefabEnemy;
        [Range(1,2)]
        public int Tier = 1;
        public int Weight = 1;
        public float minInstances = 1;
        public float maxInstances = 1;
        [HideInInspector] public float currentInstances;
    }
    [SerializeField] int MaxWeight_T1;
    [SerializeField] int MaxWeight_T2;
    public List<EnemySpawn> SpawneableEnemies = new List<EnemySpawn>();
    [HideInInspector] public List<GameObject> CurrentlySpawnedEnemies = new List<GameObject>();

    [SerializeField] Generic_OnTriggerEnterEvents SpawnTrigger;
    [SerializeField] Collider2D SpawnArea;
    [SerializeField] GameObject EnemiesContainer;
    int EnemiesAlive;
    bool areCorrectlySpawned;
    Coroutine correctlySpawnedCoroutine;
    [SerializeField] EnterTriggerCutscene enterRoomCutscene;
    public Action onEnemiesSpawned;

    public GameState gameState;

    public override void OnEnable()
    {
        base.OnEnable();

        GameEvents.OnPlayerRespawned += delayedDestroy;
        SpawnTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        SpawnTrigger.OnTriggerEntered += SpawnEnemies;
    }
    public override void OnDisable()
    {
        base.OnDisable();

        GameEvents.OnPlayerRespawned -= delayedDestroy;
        SpawnTrigger.OnTriggerEntered -= SpawnEnemies;
    }
    void delayedDestroy()
    {
        DestroyCurrentEnemies(.25f);
    }
    private void DestroyCurrentEnemies(float delay = 0)
    {
        for (int i = CurrentlySpawnedEnemies.Count - 1; i >= 0; i--)
        {
            StartCoroutine(UsefullMethods.destroyWithDelay(delay, CurrentlySpawnedEnemies[i]));

            CurrentlySpawnedEnemies.Remove(CurrentlySpawnedEnemies[i]);
        }
    }
    void SpawnEnemies(Collider2D collision)
    {
        if (isRoomPermanentlyCompleted) { RoomCompleted(false, true); return; }
        if (areCorrectlySpawned) { return; }
        if (SpawneableEnemies.Count == 0)
        {
            RoomCompleted(false,false);
            Debug.LogWarning("Nothing to spawn");
            return;
        }

        enterRoomCutscene.hasCutscenePlayed = false;

        DestroyCurrentEnemies();
        
        int currentWeight_T1 = 0;
        int currentWeight_T2 = 0;

        foreach (EnemySpawn spawn in SpawneableEnemies)
        {
            //Restart counters of spawners
            spawn.currentInstances = 0;

            //Spawn the minimums
            for (int i = 0; i < spawn.minInstances; i++)
            {
                ActuallySpawn(spawn);
                currentWeight_T1 += spawn.Weight;
            }
        }
        SpawnWeights(1, currentWeight_T1, MaxWeight_T1);
        SpawnWeights(2, currentWeight_T2, MaxWeight_T2);

        EnemiesAlive = CurrentlySpawnedEnemies.Count;
        areCorrectlySpawned = true;

        //not correctly spawned after a delay
        if (correctlySpawnedCoroutine != null) { StopCoroutine(correctlySpawnedCoroutine); }
        correctlySpawnedCoroutine = StartCoroutine(NotCorrectlySpawnedTimer());

        onEnemiesSpawned?.Invoke();
        Debug.Log("Spawned enemies wtf");

        //
        void SpawnWeights(int Tier, int currentWeight, int maxWeight)
        {
            int attemptsToSpawn = 0;
            int weightReference = currentWeight;
            while (weightReference < maxWeight)
            {
                attemptsToSpawn++;
                if (attemptsToSpawn == 30)
                {
                    Debug.LogError("Something wrong with Spawners, check min-max stuff");
                    break;
                }
                //Pick a random index
                int randomIndex = UnityEngine.Random.Range(0, SpawneableEnemies.Count);
                if (SpawneableEnemies[randomIndex].Tier != Tier) { continue; } //If not in the proper Tier pick a diferent enemy
                EnemySpawn thisSpawn = SpawneableEnemies[randomIndex];

                //If already maxed, repeat
                if (thisSpawn.currentInstances >= thisSpawn.maxInstances) { continue; }

                //Spawn and add Weight
                ActuallySpawn(thisSpawn);
                weightReference += thisSpawn.Weight;
            }
        }
        void ActuallySpawn(EnemySpawn spawn)
        {
            // Find random point and Instantiate the Enemy
            Vector2 SpawnPosition = UsefullMethods.RandomPointInCollider(SpawnArea);

            GameObject SpawnedEnemy = Instantiate(
              spawn.PrefabEnemy,
              SpawnPosition,
              Quaternion.identity,
              EnemiesContainer.transform);

            CurrentlySpawnedEnemies.Add(SpawnedEnemy);

            //Subscribe to everything
            IDamageReceiver enemy_damageReceiver = SpawnedEnemy.GetComponent<IDamageReceiver>();
            IKilleable enemy_killeable = SpawnedEnemy.GetComponent<IKilleable>();
            IStats enemyStats = SpawnedEnemy.GetComponent<IStats>();
            
            enemy_killeable.OnKilled_event += EnemyDied;
            enemy_damageReceiver.OnDamageReceived_event += EnemyDamaged;

            spawn.currentInstances++;

            //Set values per Death
            float hpPerDeath = UsefullMethods.normalizePercentage(gameState.enemiesPercentHealthPerDeath, false, true) * enemyStats.GetBaseStats().MaxHp;
            enemyStats.GetCurrentStats().MaxHp += gameState.playerDeaths * hpPerDeath;
            enemyStats.GetCurrentStats().CurrentHp += gameState.playerDeaths * hpPerDeath;

            float damagePerDeath = UsefullMethods.normalizePercentage(gameState.enemiesPercentDamageMultiplyPerDeath, false, true) * enemyStats.GetBaseStats().DamageMultiplicator;
            enemyStats.GetCurrentStats().DamageMultiplicator += gameState.playerDeaths * damagePerDeath;
        }

    }
    IEnumerator NotCorrectlySpawnedTimer()
    {
        yield return new WaitForSeconds(5);
        areCorrectlySpawned = false;
    }
    void EnemyDied(DeadCharacterInfo info)
    {
        EnemiesAlive--;
        areCorrectlySpawned = false;
        CurrentlySpawnedEnemies.Remove(info.DeadGameObject);
        if (EnemiesAlive <= 0) { RoomCompleted(true,false); }
    }
    void EnemyDamaged(ReceivedAttackInfo info)
    {
        areCorrectlySpawned = false;
    }
}


