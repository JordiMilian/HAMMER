using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Generic_OnTriggerEnterEvents;

public class EnemyGenerator : MonoBehaviour
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
    [SerializeField] Generic_OnTriggerEnterEvents ReopenDoorTrigger;
    [SerializeField] Collider2D SpawnArea;
    [SerializeField] GameObject EnemiesContainer;
    [SerializeField] bool isRoomCompleted;
    [Header("Door animation stuff")]
    [SerializeField] DoorAnimationController doorController;
    [SerializeField] AnimationClip openDoorAnimation;
    [SerializeField] Transform DoorPosition;
    int EnemiesAlive;
    bool areCorrectlySpawned;

    private void OnEnable()
    {
        SpawnTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        SpawnTrigger.OnTriggerEntered += SpawnEnemies;
        ReopenDoorTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        ReopenDoorTrigger.OnTriggerEntered += ReopenDoor;
    }
    private void Start()
    {
        if(!isRoomCompleted) { ReopenDoorTrigger.GetComponent<BoxCollider2D>().enabled = false; }
    }
    void SpawnEnemies(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        if (areCorrectlySpawned) { return; }
        if (isRoomCompleted) { return; }
        if (SpawneableEnemies.Count == 0 || MaxWeight == 0)
        {
            RoomCompleted(false);
            Debug.LogWarning("Nothing to spawn");
            return; 
        }

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
            for (int i = 0; i<spawn.minInstances; i++)
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
            if (thisSpawn.currentInstances >= thisSpawn.maxInstances) { continue;}
           
            //Spawn and add Weight
            ActuallySpawn(thisSpawn);
            currentWeight += thisSpawn.Weight;

            
        }
        EnemiesAlive = CurrentlySpawnedEnemies.Count;
        areCorrectlySpawned = true;
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
    void EnemyDied(object sender, Generic_EventSystem.Args_DeadCharacter args)
    {
        EnemiesAlive--;
        areCorrectlySpawned = false;
        if(EnemiesAlive <= 0) { RoomCompleted(true); }
    }
    void EnemyDamaged(object sender, Generic_EventSystem.EventArgs_ReceivedAttackInfo args)
    {
        areCorrectlySpawned = false;
    }
    void RoomCompleted(bool withAnimation)
    {
        isRoomCompleted = true;
        if (withAnimation) { StartCoroutine(OpenDoorFocusCamera()); }

        //Activate the trigger to Reopen Door
        ReopenDoorTrigger.GetComponent<BoxCollider2D>().enabled = true;

    }
    void ReopenDoor(object sender, EventArgsCollisionInfo args)
    {
        doorController.OpenDoor();
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
        Debug.Log("Attempts: " + attempts);
        return randomPoint;
    }
    IEnumerator OpenDoorFocusCamera()
    {
        //Wait after killing the last dude
        yield return new WaitForSeconds(0.5f);

        //Find which TargetGroup slot is empty
        CinemachineTargetGroup targetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
        int emptyTarget = FindEmptyTargetgroupSlot(targetGroup);

        //Wait a second and open door 
        yield return new WaitForSeconds(0.2f);
        doorController.OpenDoor();

        //Create a target, wait, and empty it
        AddTargetToTargetGroup(targetGroup, emptyTarget, DoorPosition, 10, 5);
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f);
        AddTargetToTargetGroup(targetGroup, emptyTarget, null, 0, 0);

    }
    public static int FindEmptyTargetgroupSlot(CinemachineTargetGroup group)
    {
        for (int i = 0; i < group.m_Targets.Length; i++)
        {
            if (group.m_Targets[i].target != null) { continue; }
            else { return i; }
        }
        Debug.LogWarning("No empty Slot found");
        return -1;
    }
    public static void AddTargetToTargetGroup(CinemachineTargetGroup group, int index, Transform transform, float weight, float radius)
    {
        group.m_Targets[index].target = transform;
        group.m_Targets[index].weight = weight;
        group.m_Targets[index].radius = radius;
    }
}

