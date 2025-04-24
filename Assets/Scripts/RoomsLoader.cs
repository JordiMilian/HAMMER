using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsLoader : MonoBehaviour
{
    public GameObject CurrentLoadedRoom;
    IRoom currentRoomInterface;
    public void  LoadNewRoom(GameObject newRoom)
    {
        //unload current if there is one
        if(CurrentLoadedRoom != null)
        {
            currentRoomInterface.OnRoomUnloaded();
            Destroy(CurrentLoadedRoom);
        }

        //load new room
        CurrentLoadedRoom = GameObject.Instantiate(newRoom, transform.position, Quaternion.identity);
        currentRoomInterface = CurrentLoadedRoom.GetComponent<IRoom>();
        currentRoomInterface.OnRoomLoaded();
    }
}
