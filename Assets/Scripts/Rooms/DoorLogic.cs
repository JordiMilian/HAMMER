using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class DoorLogic : MonoBehaviour
{
    [SerializeField] DoorAnimationController doorAnimations;
    [SerializeField] Generic_OnTriggerEnterEvents LoadTrigger;
    [SerializeField] Generic_OnTriggerEnterEvents ReopenDoorTrigger;
    [SerializeField] AnimationClip openingDoorClip;

    List<GameObject> EnemiesGO = new List<GameObject>();
    public int EnemiesAlive;
    bool AreCorrectlySpawned = true;
    bool isRoomCompleted = false;

    [Serializable]
    public class RespawnPoint 
    {
        public GameObject CurrentlySpawnedEnemy;
        public GameObject EnemyPrefab;
        Vector3 SpawnVector;
        public void setSpawnVector()
        {
            if (CurrentlySpawnedEnemy == null) 
            { 
                SpawnVector = Vector2.zero; 
                Debug.Log("Missing original enemy");
            }
            else SpawnVector = CurrentlySpawnedEnemy.transform.position; 
        }
        public GameObject Spawn()
        {
            GameObject Spawned = Instantiate(EnemyPrefab, SpawnVector, Quaternion.identity);
            return Spawned;
        }
        public void DestroyCurrentEnemy()
        {
            if(CurrentlySpawnedEnemy != null) { Destroy(CurrentlySpawnedEnemy); }
        }
    }
    [SerializeField] List<RespawnPoint> respawnPoints;
    private void OnEnable()
    {
         
        LoadTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        LoadTrigger.OnTriggerEntered += RespawnEnemies;
        ReopenDoorTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        ReopenDoorTrigger.OnTriggerEntered += ReopenDoor;
    }
    private void OnDisable()
    {
        LoadTrigger.OnTriggerEntered -= RespawnEnemies;
        ReopenDoorTrigger.OnTriggerEntered -= ReopenDoor;
    }
    private void Start()
    {
        isRoomCompleted = false;
        AreCorrectlySpawned = false;
        ReopenDoorTrigger.GetComponent<BoxCollider2D>().enabled = false;
        if (respawnPoints.Count == 0) 
        {
            Debug.Log("Room completed");
            RoomCompleted();
            return;
        }
        else { doorAnimations.CloseDoor(); }
        foreach (RespawnPoint point in respawnPoints)
        {
            point.setSpawnVector();
            AssignEnemyInfo(point, point.CurrentlySpawnedEnemy);
        }
        //RespawnEnemies(this, new EventArgsCollisionInfo(new Collider2D()));
    }
    void EnemyDied(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        EnemiesAlive--;
        AreCorrectlySpawned = false;
        if (EnemiesAlive <= 0)
        {
            RoomCompleted();
        }
    }
    void RoomCompleted()
    {
        
        StartCoroutine(OpenDoorFocusCamera());
        isRoomCompleted = true;
        ReopenDoorTrigger.GetComponent<BoxCollider2D>().enabled = true;
    }
    //Called if a player respawns behind a closed door
    void ReopenDoor(object sender, EventArgsCollisionInfo args)
    {
        doorAnimations.OpenDoor();
    }
    void RespawnEnemies(object sender, EventArgsCollisionInfo triggereInfo)
    {
        Debug.Log("Atempt respawn");
        if (AreCorrectlySpawned) return;
        if (isRoomCompleted) return;
        
        EnemiesGO.Clear();
        EnemiesAlive = 0;
        foreach (RespawnPoint point in respawnPoints)
        {
            if (point.EnemyPrefab == null) { Debug.Log("Missing Prefab"); continue; }
                
            point.DestroyCurrentEnemy();
            GameObject spawnedEnemy = point.Spawn();
            AssignEnemyInfo(point, spawnedEnemy);
        }
        
        StartCoroutine(RespawnCooldown());
        
    }
    //Asign the enemy info to each respawn point when they are respawned and subscribe to their death
    void AssignEnemyInfo(RespawnPoint point, GameObject spawnedEnemy)
    {
        point.CurrentlySpawnedEnemy = spawnedEnemy;
        EnemiesGO.Add(spawnedEnemy);
        Generic_EventSystem thisEventSystem = spawnedEnemy.GetComponent<Generic_EventSystem>();
        thisEventSystem.OnDeath += EnemyDied;
        EnemiesAlive++;
    }
    IEnumerator RespawnCooldown()
    {
        AreCorrectlySpawned = true;
        yield return new WaitForSeconds(4);
        AreCorrectlySpawned = false;
    }
    IEnumerator OpenDoorFocusCamera()
    {
        //Wait after killing the last dude
        yield return new WaitForSeconds(0.5f);


        //Find which TargetGroup slot is empty
        CinemachineTargetGroup targetGroup = GameObject.Find("TargetGroup").GetComponent<CinemachineTargetGroup>();
        int emptyTarget = 0;
        for(int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            if (targetGroup.m_Targets[i].target != null) { continue; }
            else { emptyTarget = i; break; }
        }

        //Wait a second and open door 
        yield return new WaitForSeconds(0.2f);
        doorAnimations.OpenDoor();
        //Create a target, wait, and empty it
        targetGroup.m_Targets[emptyTarget].target = transform;
        targetGroup.m_Targets[emptyTarget].weight = 10;
        targetGroup.m_Targets[emptyTarget].radius = 5;
        yield return new WaitForSeconds(openingDoorClip.length + 0.3f);
        targetGroup.m_Targets[emptyTarget].target = null;
        targetGroup.m_Targets[emptyTarget].weight = 0;
        targetGroup.m_Targets[emptyTarget].radius = 0;
    }
}
