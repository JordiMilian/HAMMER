using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingCurrentRoom : MonoBehaviour
{
    CurrentPlayersRoomManager roomManager;
    BoxCollider2D roomCollider;
    BoxCollider2D groupCollider;
    private void Awake()
    {
        roomManager = GetComponent<CurrentPlayersRoomManager>();
        roomManager.OnPlayerChangedRoom += GetColliders;
    }
    void GetColliders()
    {
        roomCollider = roomManager.currentRoom.GetComponent<BoxCollider2D>();
        groupCollider = roomManager.currentGroupOfRooms.GetComponent<BoxCollider2D>();
    }
    private void OnDrawGizmos()
    {
        if(roomCollider != null && groupCollider != null)
        {
            UsefullMethods.DrawCollider(roomCollider, Color.cyan);
            UsefullMethods.DrawCollider(groupCollider, Color.blue);
        }
       
    }
}
