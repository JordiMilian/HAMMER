using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KILLENEMIESAROUND : MonoBehaviour
{
    [SerializeField] GameState gameState;
    List<IHealth> enemiesHealth = new List<IHealth>();
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
    List<IHealth> GetEnemiesHealths()
    {
        if (gameState.currentPlayerRooms_index.Count == 0) { return new List<IHealth>(); }

        RoomGenerator_Manager roomsGenerator = RoomGenerator_Manager.Instance;
        Vector3Int currentRoomsIndex = gameState.currentPlayerRooms_index[gameState.currentPlayerRooms_index.Count - 1];
        currentRoom = roomsGenerator.CompleteList_spawnedRooms[currentRoomsIndex.x].list[currentRoomsIndex.y];

        RoomWithEnemiesLogic currentRoomWithEnemies = currentRoom.GetComponent<RoomWithEnemiesLogic>();
        if(currentRoomWithEnemies == null) { return new List<IHealth>(); }
        GameObject[] enemiesGO = currentRoomWithEnemies.CurrentlySpawnedEnemies.ToArray();

        List<IHealth> enemiesHealthListTemp = new List<IHealth>();
        foreach (GameObject enem in enemiesGO)
        {
            IHealth enemieHealth = enem.GetComponent<IHealth>();

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
        foreach(IHealth health in enemiesHealth)
        {
            Vector2 enemyPos = (health as MonoBehaviour).transform.position;
            if((enemyPos - roomPos).magnitude > InstaKillEnemiesOnDistance)
            {
                health.RemoveHealth(50);
                Debug.Log("Killed: " + (health as MonoBehaviour).gameObject.name + "because he got too far");
            }
        }
    }
    IEnumerator KillEmSlowly(IHealth[] healthsArray)
    {
        foreach(IHealth health in healthsArray)
        {
            health.RemoveHealth(50);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
