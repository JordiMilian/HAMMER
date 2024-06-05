using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayersRoomManager : MonoBehaviour
{
    public GameObject currentRoom;
    public GameObject currentGroupOfRooms;
    public Action OnPlayerChangedRoom;

    public static CurrentPlayersRoomManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void SetCurrentRoom(GameObject room)
    {
        currentRoom = room;
        OnPlayerChangedRoom?.Invoke();
    }
    public void SetCurrentGroup(GameObject group)
    {
        currentGroupOfRooms = group;
        OnPlayerChangedRoom?.Invoke();
    }
}
