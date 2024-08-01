using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_LastUpgradeHolder : MonoBehaviour
{
    //SERIELIZEFIELD
    [SerializeField] BaseRoomWithDoorLogic roomLogic;
    [SerializeField] Room_script thisRoom;
    [SerializeField] GameState gameState;
    [SerializeField] GameObject BaseUpgradeContainerPrefab;
    [SerializeField] Transform SpawnPosition;

    //An instance of this script is holded by every room. 
    //The subcription to itselt is done by the MANAGER
    //The unsubscribtion is done by itself by multiple reasons (player picks up the uprade or player dies again)
    private void OnEnable()
    {
        roomLogic.onRoomCompleted += checkIfSpawnUpgrade;
    }
    private void OnDisable()
    {
        roomLogic.onRoomCompleted -= checkIfSpawnUpgrade;
    }

    void checkIfSpawnUpgrade(BaseRoomWithDoorLogic roomLogic)
    {
        if(thisRoom.indexInCompleteList == gameState.IndexOfLostUpgradeRoom)
        {
            SpawnUpgrades();
            gameState.isLostUpgradeAvailable = false;
        }
    }
    void SpawnUpgrades()
    {
        GameObject newContainer = Instantiate(BaseUpgradeContainerPrefab, SpawnPosition.position, Quaternion.identity);
        UpgradeContainer upgradeContainer = newContainer.GetComponent<UpgradeContainer>();

        upgradeContainer.upgradeEffect = gameState.lastLostUpgrade;

        upgradeContainer.OnSpawnContainer();

        
    }
}
