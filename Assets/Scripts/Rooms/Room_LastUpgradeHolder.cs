using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_LastUpgradeHolder : MonoBehaviour
{
    //SERIELIZEFIELD
    [SerializeField] BaseRoomWithDoorLogic roomLogic;
    [SerializeField] GameState gameState;
    [SerializeField] GameObject BaseUpgradeContainerPrefab;
    [SerializeField] Transform SpawnPosition;

    public void subscribeToRoomCompleted()
    {
        roomLogic.onRoomCompleted += spawnUpgrade;
        GameEvents.OnPlayerDeath += unsubscribeToRoomCompleted;
    }
    void unsubscribeToRoomCompleted()
    {
        roomLogic.onRoomCompleted -= spawnUpgrade;
        GameEvents.OnPlayerDeath -= unsubscribeToRoomCompleted;
    }
    void spawnUpgrade(BaseRoomWithDoorLogic roomLogic)
    {
        unsubscribeToRoomCompleted();

        //Spawning logic...
        GameObject newContainer = Instantiate(BaseUpgradeContainerPrefab, SpawnPosition.position, Quaternion.identity);
        UpgradeContainer upgradeContainer = newContainer.GetComponent<UpgradeContainer>();

        upgradeContainer.upgradeEffect = gameState.lastLostUpgrade;

        upgradeContainer.OnSpawnContainer();
    }
}
