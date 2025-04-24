using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleRooms_Controler : MonoBehaviour, IRoom
{
    //Rooms that are part of MultipleRooms should follow some rules
    //- The controller script of each of these rooms must implement the interface IMultipleRoom in addition to the IRoom interface 
    //- On the OnRoomLoaded we should not place cutscenes or anything that interrupts the player. Meaning NO BASIC ENEMY ROOMS should be placed as anything but the first room
    //- All rooms except the first one should not have an entering door to aboid two doors overlapping
    //- Make sure to disable the ExitColliders on the intermidiate rooms. Only the LAST room should have it on the exit position.

    //This is a simplified version of the script RoomsGroup_script. THis script only spawns rooms in the given order.
    private void OnValidate()
    {
        //there should be a check if the roomsToLoads implements IMultipleRoom
    }

    public GameObject[] RoomsToLoad; 

    public List<GameObject> currentlySpawnedRooms = new List<GameObject>();
    Vector2 LastExitPosition = Vector2.zero;
    public void OnRoomLoaded()
    {
        SpawnRooms(transform.position);
        //Fade out all the rooms except the first?
    }

    public void OnRoomUnloaded()
    {
       foreach(GameObject room in currentlySpawnedRooms)
       {
            IRoom room_IRoom = room.GetComponent<IRoom>();
            room_IRoom.OnRoomUnloaded();
       }
    }
    
    void SpawnRooms(Vector2 InitialPosition)
    {
        LastExitPosition = InitialPosition;
        GameObject[] chosenRooms = RoomsToLoad;

        transform.position = InitialPosition; //Put parent in Initial position

        for (int i = 0; i < chosenRooms.Length; i++)
        {
            GameObject newRoom = Instantiate(chosenRooms[i], LastExitPosition, Quaternion.identity, transform);

            IRoom newRoom_IRoom = newRoom.GetComponent<IRoom>();
            IMultipleRoom newRoom_IMultipleRoom = newRoom.GetComponent<IMultipleRoom>();
            newRoom_IRoom.OnRoomLoaded();
            if(newRoom_IMultipleRoom == null) { Debug.LogError($"Error: {chosenRooms[i].name} is not implementing IMultipleRoom"); }
            LastExitPosition = newRoom_IMultipleRoom.ExitPos;

            currentlySpawnedRooms.Add(newRoom);
        }
        CreateCombinedCollider();
    }
    void CreateCombinedCollider()
    {
        Bounds combinedBounds = new Bounds(transform.position, Vector2.zero);
        foreach (GameObject room in currentlySpawnedRooms)
        {
            IMultipleRoom room_IMultipleRoom = room.GetComponent<IMultipleRoom>();
            combinedBounds.Encapsulate(room_IMultipleRoom.combinedCollider.GetComponent<BoxCollider2D>().bounds);
        }
        UsefullMethods.BoundsToBoxCollider(combinedBounds, transform.position, transform.gameObject);
    }
}
