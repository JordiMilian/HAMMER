using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KILLENEMIESAROUND : MonoBehaviour
{
    [SerializeField] GameState gameState;
    List<Generic_CharacterHealthSystem> enemiesHealth = new List<Generic_CharacterHealthSystem>();
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
        InvokeRepeating("CheckEnemiesDistanceAndKill", 1, 5);
    }
    List<Generic_CharacterHealthSystem> GetEnemiesHealths()
    {
        if (gameState.currentPlayerRooms_index.Count == 0) { return new List<Generic_CharacterHealthSystem>(); }

        RoomGenerator_Manager roomsGenerator = RoomGenerator_Manager.Instance;
        Vector3Int currentRoomsIndex = gameState.currentPlayerRooms_index[gameState.currentPlayerRooms_index.Count - 1];
        currentRoom = roomsGenerator.CompleteList_spawnedRooms[currentRoomsIndex.x].list[currentRoomsIndex.y];

        RoomWithEnemiesLogic currentRoomWithEnemies = currentRoom.GetComponent<RoomWithEnemiesLogic>();
        if(currentRoomWithEnemies == null) { return new List<Generic_CharacterHealthSystem>(); }
        GameObject[] enemiesGO = currentRoomWithEnemies.CurrentlySpawnedEnemies.ToArray();

        List<Generic_CharacterHealthSystem> enemiesHealthListTemp = new List<Generic_CharacterHealthSystem>();
        foreach (GameObject enem in enemiesGO)
        {
            Generic_CharacterHealthSystem enemieHealth = enem.GetComponent<Generic_CharacterHealthSystem>();

            if (enemieHealth == null) { continue; }

            enemiesHealthListTemp.Add(enemieHealth);
        }
        return enemiesHealthListTemp;
    }
    void KILLEMALL()
    {
        GetEnemiesHealths();
        StartCoroutine(KillEmSlowly(enemiesHealth.ToArray()));
    }
    void CheckEnemiesDistanceAndKill()
    {
        enemiesHealth = GetEnemiesHealths();
        if(enemiesHealth.Count == 0) { return; };
        Vector2 roomPos = currentRoom.transform.position;
        foreach(Generic_CharacterHealthSystem health in  enemiesHealth)
        {
            Vector2 enemyPos = health.transform.position;
            if((enemyPos - roomPos).magnitude > InstaKillEnemiesOnDistance)
            {
                health.RemoveLife(50, gameObject);
                Debug.Log("Killed: " + health.gameObject.name + "because he got too far");
            }
        }

    }
    IEnumerator KillEmSlowly(Generic_CharacterHealthSystem[] healthsArray)
    {
        foreach(Generic_CharacterHealthSystem health in healthsArray)
        {
            health.RemoveLife(50, gameObject);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
