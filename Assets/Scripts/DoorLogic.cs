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

    List<GameObject> EnemiesGO = new List<GameObject>();
    int EnemiesAlive;
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
        LoadTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        LoadTrigger.OnTriggerEntered += RespawnEnemies;
        ReopenDoorTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
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
        ReopenDoorTrigger.enabled = false;
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
        RespawnEnemies(this, new EventArgsCollisionInfo(new Collider2D()));
    }
    void EnemyDied()
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
        doorAnimations.OpenDoor();
        isRoomCompleted = true;
        ReopenDoorTrigger.enabled = true;
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
        foreach (RespawnPoint point in respawnPoints)
        {
            if (point.EnemyPrefab == null) { Debug.Log("Missing Prefab"); continue; }
                
            point.DestroyCurrentEnemy();
            GameObject spawnedEnemy = point.Spawn();
            AssignEnemyInfo(point, spawnedEnemy);
        }
        EnemiesAlive = EnemiesGO.Count;
        Debug.Log("EnemiesCount = " + EnemiesAlive);
        StartCoroutine(RespawnCooldown());
        
    }
    //Asign the enemy info to each respawn point when they are respawned and subscribe to their death
    void AssignEnemyInfo(RespawnPoint point, GameObject spawnedEnemy)
    {
        point.CurrentlySpawnedEnemy = spawnedEnemy;
        EnemiesGO.Add(spawnedEnemy);
        Generic_EventSystem thisEventSystem = spawnedEnemy.GetComponent<Generic_EventSystem>();
        thisEventSystem.OnDeath += EnemyDied;
    }
    IEnumerator RespawnCooldown()
    {
        AreCorrectlySpawned = true;
        yield return new WaitForSeconds(4);
        AreCorrectlySpawned = false;
    }
}
