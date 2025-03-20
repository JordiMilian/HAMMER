using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoomsOnRespawn : MonoBehaviour
{
    [SerializeField] RoomGenerator_Manager roomGenerator;
    [SerializeField] GameState gameState;

    private void OnEnable()
    {
        GameEvents.OnPlayerRespawned += GenerateRooms;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerRespawned -= GenerateRooms;
    }
    void GenerateRooms()
    {
        int currentRoomIndex = gameState.currentPlayersRooms[gameState.currentPlayersRooms.Count - 1].indexInCompleteList.x;
        roomGenerator.Call_GenerateRooms_StartFromIndex(currentRoomIndex + 1);
    }
}
