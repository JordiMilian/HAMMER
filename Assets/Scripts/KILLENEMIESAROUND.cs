using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KILLENEMIESAROUND : MonoBehaviour
{
    [SerializeField] GameState gameState;
    List< Generic_HealthSystem> enemiesHealth = new List<Generic_HealthSystem>();
    [SerializeField] float InstaKillEnemiesOnDistance;
     Room_script currentRoom;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            KILLEMALL();
        }
    }
    private void Start()
    {
        InvokeRepeating("CheckEnemiesDistanceAndKill", 0, 5);
    }
    void UpdateEnemiesHealthList()
    {
        enemiesHealth.Clear();
        RoomGenerator_Manager roomsGenerator = RoomGenerator_Manager.Instance;

        if(gameState.currentPlayerRooms_index.Count == 0) { return; }

        Vector3Int currentRoomsIndex = gameState.currentPlayerRooms_index[gameState.currentPlayerRooms_index.Count - 1];
        currentRoom = roomsGenerator.CompleteList_spawnedRooms[currentRoomsIndex.x].list[currentRoomsIndex.y];

        RoomWithEnemiesLogic currentRoomWithEnemies = currentRoom.GetComponent<RoomWithEnemiesLogic>();
        if(currentRoomWithEnemies == null) { return; }
        GameObject[] enemiesGO = currentRoomWithEnemies.CurrentlySpawnedEnemies.ToArray();
        enemiesHealth = new List<Generic_HealthSystem>();
        foreach (GameObject enem in enemiesGO)
        {
            Generic_HealthSystem enemieHealth = enem.GetComponent<Generic_HealthSystem>();

            if (enemieHealth == null) { continue; }

            enemiesHealth.Add(enemieHealth);
        }
    }
    void KILLEMALL()
    {
        UpdateEnemiesHealthList();
        StartCoroutine(KillEmSlowly(enemiesHealth.ToArray()));
    }
    void CheckEnemiesDistanceAndKill()
    {
        Debug.Log("Checking enemies distance");
        UpdateEnemiesHealthList();
        Vector2 roomPos = currentRoom.transform.position;
        foreach(Generic_HealthSystem health in  enemiesHealth)
        {
            Vector2 enemyPos = health.transform.position;
            if((enemyPos - roomPos).magnitude > InstaKillEnemiesOnDistance)
            {
                health.RemoveLife(50, gameObject);
                Debug.Log("Killed: " + health.gameObject.name + "because he got too far");
            }
        }

    }
    IEnumerator KillEmSlowly(Generic_HealthSystem[] healthsArray)
    {
        foreach(Generic_HealthSystem health in healthsArray)
        {
            health.RemoveLife(50, gameObject);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
