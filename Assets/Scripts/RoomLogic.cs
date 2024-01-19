using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class RoomLogic : MonoBehaviour
{
    [SerializeField] DoorController exitDoorController;
    [SerializeField] Generic_OnTriggerEnterEvents LoadTrigger;

    List<GameObject> EnemiesGO = new List<GameObject>();
    int EnemiesAlive;
    public bool AreSpawned = true;

    [Serializable]
    public class RespawnPoint 
    {
        public GameObject SpawnedEnemy;
        public GameObject EnemyPrefab;
        Vector3 SpawnVector;
        public void setSpawnVector()
        {
            SpawnVector = SpawnedEnemy.transform.position;
        }
        public GameObject Spawn()
        {
           GameObject Spawned =  Instantiate(EnemyPrefab, SpawnVector,Quaternion.identity);
            return Spawned;
        }
    }
    [SerializeField] List<RespawnPoint> respawnPoints;
    private void OnEnable()
    {
        LoadTrigger.ActivatorTags.Add("Player_SinglePointCollider");
        LoadTrigger.OnTriggerEntered += RespawnEnemies;
    }
    private void Start()
    {
        foreach (RespawnPoint point in respawnPoints)
        {
            point.setSpawnVector();
        }
    }
    void EnemyDied(object sender, EventArgs args)
    {
        EnemiesAlive--;
        if (EnemiesAlive <= 0)
        {
            OpenDoor();
            AreSpawned = false;
        }
    }
     void OpenDoor()
    {
        exitDoorController.OpenDoor();
    }
    public void RespawnEnemies(object sender, EventArgsTriggererInfo triggereInfo)
    {
        if(!AreSpawned)
        {
            EnemiesGO.Clear();
            foreach (RespawnPoint point in respawnPoints)
            {
                GameObject spawnedEnemy = (point.Spawn());

                EnemiesGO.Add(spawnedEnemy);
                Generic_HealthSystem thisHealth = spawnedEnemy.GetComponent<Generic_HealthSystem>();
                thisHealth.OnDeath += EnemyDied;
            }
            EnemiesAlive = EnemiesGO.Count;
            AreSpawned = true;
        }
    }
}
