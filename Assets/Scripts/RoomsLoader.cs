using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsLoader : MonoBehaviour
{
    public GameObject CurrentLoadedRoom;
    public void  LoadNewRoom(GameObject newRoom)
    {
        //unload current if there is one
        if(CurrentLoadedRoom != null)
        {
            Destroy(CurrentLoadedRoom);
        }
        //load new room
        CurrentLoadedRoom = GameObject.Instantiate(newRoom, transform.position, Quaternion.identity);
    }
}
