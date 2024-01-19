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
    public bool AreCorrectlySpawned = true;

    [Serializable]
    public class RespawnPoint 
    {
        public GameObject CurrentlySpawnedEnemy;
        public GameObject EnemyPrefab;
        Vector3 SpawnVector;
        public void setSpawnVector()
        {
            SpawnVector = CurrentlySpawnedEnemy.transform.position;
        }
        public GameObject Spawn()
        {
           GameObject Spawned =  Instantiate(EnemyPrefab, SpawnVector,Quaternion.identity);
            return Spawned;
        }
        public void DestroyCurrentEnemy()
        {
            Destroy(CurrentlySpawnedEnemy);
        }
    }
    [SerializeField] List<RespawnPoint> respawnPoints;
    private void OnEnable()
    {
        LoadTrigger.ActivatorTags.Add(TagsCollection.instance.Player_SinglePointCollider);
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
            AreCorrectlySpawned = false;
        }
    }
     void OpenDoor()
    {
        exitDoorController.OpenDoor();
    }
    public void RespawnEnemies(object sender, EventArgsTriggererInfo triggereInfo)
    {
        if(!AreCorrectlySpawned)
        {
            EnemiesGO.Clear();
            foreach (RespawnPoint point in respawnPoints)
            {
                point.DestroyCurrentEnemy();
                GameObject spawnedEnemy = (point.Spawn());
                point.CurrentlySpawnedEnemy = spawnedEnemy;
                EnemiesGO.Add(spawnedEnemy);
                Generic_HealthSystem thisHealth = spawnedEnemy.GetComponent<Generic_HealthSystem>();
                thisHealth.OnDeath += EnemyDied;
            }
            EnemiesAlive = EnemiesGO.Count;
            StartCoroutine(RespawnCooldown());
        }
    }
    IEnumerator RespawnCooldown()
    {
        AreCorrectlySpawned = true;
        yield return new WaitForSeconds(4);
        AreCorrectlySpawned = false;
    }
}
