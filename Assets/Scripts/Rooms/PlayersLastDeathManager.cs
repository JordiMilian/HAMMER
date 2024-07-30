using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersLastDeathManager : MonoBehaviour
{
    //Each room has a Room_LastUpgradeHolder.
    //When OnPlayerdeath is called we activate the subscription on the Room_lastUpgradeHolder on the current players room
    //All the unsubcribinge  is managed from their own script 
    //So this script is only responsible for finding the current room and activating the bell to spawn the upgrade 

    [SerializeField] GameState gameState;
    
    private void OnEnable()
    {
        GameEvents.OnPlayerDeath += SetRoomsUpgrade;
    }
    void SetRoomsUpgrade()
    {
        if(gameState.playerUpgrades.Count == 0) { return; }
        
        Room_LastUpgradeHolder lastUpgradeRoom = gameState.currentPlayersRooms[gameState.currentPlayersRooms.Count - 1].gameObject.GetComponent<Room_LastUpgradeHolder>();
        lastUpgradeRoom.subscribeToRoomCompleted();
    }
}
