using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator_Manager : MonoBehaviour
{
    [SerializeField] bool GenerateRooms;
    [SerializeField] int GeneratedGroupOfRoomsIndex;
    [Serializable]
    public class GroupOfRooms
    {
        public string Name;
        public enum TypesOfRoom { AvoidAnyRepetition, AvoidTwoSameConsecutiveRooms, TrueRandom}
        public TypesOfRoom TypeOfRoom;
        public int AmountOfRoomsToSpawn;
        public GameObject[] RoomPrefabs;
        public Transform ParentTf;
        public Vector2 LastExitTf;

        List<GameObject> currentlySpawnedRooms = new List<GameObject>();
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
        void SpawnGroupOfRooms( Vector2 InitialPosition)
        {
            LastExitTf = InitialPosition;
            GameObject[] chosenRooms = GetRandomRooms();

            //If there is no parent, create one
            if(ParentTf == null)
            {
                GameObject ParentGO = Instantiate(new GameObject("RoomsParent"));
                ParentTf = ParentGO.transform;
            }

            ParentTf.position = InitialPosition; //Put parent in Initial position

            for (int i = 0; i < chosenRooms.Length; i++)
            {
                GameObject newRoom = Instantiate(chosenRooms[i], LastExitTf, Quaternion.identity, ParentTf);
                RoomGenerator_Room thisRoom = newRoom.GetComponent<RoomGenerator_Room>();
                LastExitTf = thisRoom.ExitPosition.position;
                thisRoom.calculateBounds();
                currentlySpawnedRooms.Add(newRoom);
            }
            GetFullBounds();
        }
        void DestroyCurrentlySpawnedRooms()
        {
            foreach (GameObject room in currentlySpawnedRooms)
            {
                Destroy(room);
            }
        }
        public void RespawnRooms(Vector2 initialPos)
        {
            DestroyCurrentlySpawnedRooms();
            SpawnGroupOfRooms(initialPos);
        }
        void GetFullBounds()
        {
            Bounds combinedBounds = new Bounds(ParentTf.position, Vector2.zero);
            foreach (GameObject room in currentlySpawnedRooms)
            {
                combinedBounds.Encapsulate( room.GetComponent<RoomGenerator_Room>().combinedWorldBounds);
            }
            
            UsefullMethods.BoundsToBoxCollider(combinedBounds, ParentTf.position, ParentTf.gameObject);
        }
    }
    public List<GroupOfRooms> GroupsOfRoomsList = new List<GroupOfRooms>();
 
    private void Update()
    {
        if(GenerateRooms)
        {
            GenerateRoomsFromIndex(GeneratedGroupOfRoomsIndex);
            GenerateRooms = false;
        }
    }
    void GenerateRoomsFromIndex(int index)
    {
        for (int i = index; i < GroupsOfRoomsList.Count; i++)
        {
            GenerateIndexRooms(i);
        }
    }
    void GenerateIndexRooms(int index)
    {
        Vector2 tempInitialPosition = transform.position;
        if (index == 0) { }

        else if(GroupsOfRoomsList[index - 1].ParentTf != null)
        {
            tempInitialPosition = GroupsOfRoomsList[index - 1].LastExitTf;
        }

        GroupsOfRoomsList[index].RespawnRooms(tempInitialPosition);
    }

}
