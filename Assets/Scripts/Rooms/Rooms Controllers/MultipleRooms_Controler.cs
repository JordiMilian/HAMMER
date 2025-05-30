using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleRooms_Controler : MonoBehaviour, IRoom
{
    /// <summary>
    /// This script must be placed into an empty Prefab to be placed into the Game Controller
    /// Rooms that are part of MultipleRooms should follow some rules
    ///- The controller script of each of these rooms must implement the interface IMultipleRoom in addition to the IRoom interface 
    ///- On the OnRoomLoaded we should not place cutscenes or anything that interrupts the player. Meaning NO BASIC ENEMY ROOMS should be placed as anything but the first room
    ///- All rooms except the first one should not have an entering door to aboid two doors overlapping
    ///- Make sure to disable the ExitColliders on the intermidiate rooms. Only the LAST room should have it on the exit position.

    ///This is a simplified version of the script RoomsGroup_script. THis script only spawns rooms in the given order.
    /// </summary>
    

    public GameObject[] RoomsToLoad;

    public List<GameObject> currentlySpawnedRooms = new List<GameObject>();
    Vector2 LastExitPosition = Vector2.zero;

    [SerializeField] bool useCombinedArea;
    [SerializeField] Generic_OnTriggerEnterEvents combinedCollider;

    public void OnRoomLoaded()
    {
        SpawnRooms(transform.position);
        
        for (int f = 0; f < currentlySpawnedRooms.Count; f++)//Fade out all the rooms except the first
        {
            Rooms_FadeInOut thisFader = currentlySpawnedRooms[f].GetComponentInChildren<Rooms_FadeInOut>();
            if(f == 0) { thisFader.playerEnteredRoom(new Collider2D()); }
            else { thisFader.playerExitedRoom(new Collider2D()); }
        }
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
        Bounds combinedBounds = new Bounds();

        for (int i = 0; i < chosenRooms.Length; i++)
        {
            GameObject newRoom = Instantiate(chosenRooms[i], LastExitPosition, Quaternion.identity, transform);

            IRoom newRoom_IRoom = newRoom.GetComponent<IRoom>();
            IMultipleRoom newRoom_IMultipleRoom = newRoom.GetComponent<IMultipleRoom>();
            newRoom_IRoom.OnRoomLoaded();
            if(newRoom_IMultipleRoom == null) { Debug.LogError($"Error: {chosenRooms[i].name} is not implementing IMultipleRoom"); }
            LastExitPosition = newRoom_IMultipleRoom.ExitPos;
            combinedBounds.Encapsulate(newRoom_IMultipleRoom.combinedCollider.GetComponent<Collider2D>().bounds);

            currentlySpawnedRooms.Add(newRoom);
        }
        if (useCombinedArea) { CreateCombinedCollider(combinedBounds); }
        
    }
    void CreateCombinedCollider(Bounds combinedBounds)
    {
        foreach (GameObject room in currentlySpawnedRooms)
        {
            IMultipleRoom room_IMultipleRoom = room.GetComponent<IMultipleRoom>();
            combinedBounds.Encapsulate(room_IMultipleRoom.combinedCollider.GetComponent<BoxCollider2D>().bounds);
        }

        UsefullMethods.BoundsToBoxCollider(combinedBounds, transform.position, combinedCollider.gameObject);
    }
}
