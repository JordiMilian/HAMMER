using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsGroup_script : MonoBehaviour
{
    public string Name;
    public enum TypesOfRoom { AvoidAnyRepetition, AvoidTwoSameConsecutiveRooms, TrueRandom }
    public TypesOfRoom TypeOfRoom;
    public int AmountOfRoomsToSpawn;
    public GameObject[] RoomPrefabs;

    [HideInInspector] public Vector2 LastExitPosition;

    public List<Room_script> currentlySpawnedRooms = new List<Room_script>();
    GameObject[] GetRandomRooms()
    {
        List<GameObject> roomsList = new List<GameObject>();
        switch (TypeOfRoom)
        {
            case TypesOfRoom.TrueRandom:

                for (int i = 0; i < AmountOfRoomsToSpawn; i++)
                {
                    roomsList.Add(RoomPrefabs[UnityEngine.Random.Range(0, RoomPrefabs.Length)]);
                }

                return roomsList.ToArray();

            case TypesOfRoom.AvoidTwoSameConsecutiveRooms:

                int previusIndex = -1;

                for (int i = 0; i < AmountOfRoomsToSpawn; i++)
                {
                    int randomIndex = UnityEngine.Random.Range(0, RoomPrefabs.Length);
                    if (randomIndex == previusIndex) { i--; continue; }
                    roomsList.Add(RoomPrefabs[randomIndex]);
                    previusIndex = randomIndex;
                }

                return roomsList.ToArray();

            case TypesOfRoom.AvoidAnyRepetition:

                int tempAmountOfRooms = AmountOfRoomsToSpawn; //use a temporal variable to not touch the public int

                //If there are more room requests that available room. Generate the max amount of rooms posible
                if (tempAmountOfRooms > RoomPrefabs.Length) { tempAmountOfRooms = RoomPrefabs.Length; }

                List<int> spawnedIndexes = new List<int>();

                for (int i = 0; i < tempAmountOfRooms; i++)
                {
                    //generate random index and if its contained in the list, repeat 
                    int randomIndex = UnityEngine.Random.Range(0, RoomPrefabs.Length);
                    if (spawnedIndexes.Contains(randomIndex)) { i--; continue; }

                    spawnedIndexes.Add(randomIndex);

                    roomsList.Add(RoomPrefabs[randomIndex]);
                }

                return roomsList.ToArray();
        }
        return roomsList.ToArray();
    }
    void SpawnNewGroupOfRooms(Vector2 InitialPosition)
    {
        LastExitPosition = InitialPosition;
        GameObject[] chosenRooms = GetRandomRooms();

        transform.position = InitialPosition; //Put parent in Initial position

        for (int i = 0; i < chosenRooms.Length; i++)
        {
            GameObject newRoom = Instantiate(chosenRooms[i], LastExitPosition, Quaternion.identity, transform);
            Room_script thisRoom = newRoom.GetComponent<Room_script>();
            LastExitPosition = thisRoom.ExitPosition.position;

            //The bounds can be calculated beforehand. If they are skip
            if(!thisRoom.isBoundPrecalculated)
            {
                thisRoom.calculateBounds();
            }

            currentlySpawnedRooms.Add(thisRoom);
        }
        CreateCombinedCollider();
    }
    void DestroyCurrentlySpawnedRooms()
    {
        List<Room_script> roomsToDestroy = new List<Room_script>();
        foreach (Room_script room in currentlySpawnedRooms)
        {
            roomsToDestroy.Add(room);
        }

        foreach (Room_script room in roomsToDestroy)
        {
            currentlySpawnedRooms.Remove(room);
            Destroy(room.gameObject);
        }
    }
    public void RespawnRooms(Vector2 initialPos)
    {
        DestroyCurrentlySpawnedRooms();
        SpawnNewGroupOfRooms(initialPos);
    }
    void CreateCombinedCollider()
    {
        Bounds combinedBounds = new Bounds(transform.position, Vector2.zero);
        foreach (Room_script room in currentlySpawnedRooms)
        {
            combinedBounds.Encapsulate(room.enterRoomCollider.GetComponent<BoxCollider2D>().bounds);
        }
        UsefullMethods.BoundsToBoxCollider(combinedBounds, transform.position, transform.gameObject);
    }
}
