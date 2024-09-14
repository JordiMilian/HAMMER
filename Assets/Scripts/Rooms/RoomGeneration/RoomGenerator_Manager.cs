using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RoomGenerator_Manager : MonoBehaviour
{
    public Action<int> Call_GenerateRooms_StartFromIndex;
    public Action<int> Call_GenerateSingleGroupOfRooms;
    public Action<Vector2> Call_GenerateAllRoomsFromPosition;
    public int AreaIndex;

    public List<RoomsGroup_script> GroupsOfRoomsList = new List<RoomsGroup_script>();

    [Serializable]
    public class GroupList
    {
        public List<Room_script> list;
        public GroupList()
        {
            list = new List<Room_script>();
        }
    }
    public List<GroupList> CompleteList_spawnedRooms = new List<GroupList>();

    [SerializeField] GameState gameState;

    Vector2 LastInitialPosition;

    private void OnEnable()
    {
        Call_GenerateRooms_StartFromIndex += GenerateRooms_startFromIndex;
        Call_GenerateSingleGroupOfRooms += GenerateSingleGroupOfRooms;
        Call_GenerateAllRoomsFromPosition += GenerateAllRoomsFromPos;
    }
    private void OnDisable()
    {
        Call_GenerateRooms_StartFromIndex -= GenerateRooms_startFromIndex;
        Call_GenerateSingleGroupOfRooms -= GenerateSingleGroupOfRooms;
        Call_GenerateAllRoomsFromPosition -= GenerateAllRoomsFromPos;

        gameState.currentPlayersRooms.Clear();
        gameState.currentPlayerRooms_index.Clear();
    }
    
    void GenerateRooms_startFromIndex(int index)
    {
        //Create empty Lists if they dont exist yet
        CreateNecesaryEmptyLists();

        for (int i = index; i < GroupsOfRoomsList.Count; i++)
        {
            GenerateGroup(i, LastInitialPosition);
        }
    }
    void GenerateAllRoomsFromPos(Vector2 startingPos)
    {
        CreateNecesaryEmptyLists();

        for (int i = 0; i < GroupsOfRoomsList.Count; i++)
        {
            GenerateGroup(i, startingPos);
        }

        LastInitialPosition = startingPos;
    }
    void CreateNecesaryEmptyLists()
    {
        int necesaryIndexes = GroupsOfRoomsList.Count;
        if (CompleteList_spawnedRooms.Count < necesaryIndexes)
        {
            for (int i = 0; i < necesaryIndexes; i++)
            {
                CompleteList_spawnedRooms.Add(new GroupList());
            }
        }
    }
    void GenerateSingleGroupOfRooms(int index)
    {
        GenerateGroup(index, transform.position);
        UpdateInitialPositions(index + 1);
    }
    void UpdateInitialPositions(int index)
    {
        for (int i = index; i < GroupsOfRoomsList.Count; i++)
        {
            RoomsGroup_script thisGroup = GroupsOfRoomsList[i];
            thisGroup.gameObject.transform.position = GroupsOfRoomsList[i - 1].LastExitPosition;
            thisGroup.LastExitPosition = thisGroup.currentlySpawnedRooms[thisGroup.currentlySpawnedRooms.Count - 1].ExitPosition.position;
        }
    }
    void GenerateGroup(int index, Vector2 maybeStartingPos)
    {
        Vector2 tempInitialPosition = maybeStartingPos;
        if (index == 0) { }

        else if(GroupsOfRoomsList[index - 1].transform != null)
        {
            tempInitialPosition = GroupsOfRoomsList[index - 1].LastExitPosition;
        }

        GroupsOfRoomsList[index].RespawnRooms(tempInitialPosition); //Instantiate the chosen indexed group of rooms


        //Refill the subLists
        CompleteList_spawnedRooms[index].list.Clear();

        //Go throw all the rooms in that group to do stuff
        for (int i = 0; i < GroupsOfRoomsList[index].currentlySpawnedRooms.Count; i++)
        {
            Room_script roomScript = GroupsOfRoomsList[index].currentlySpawnedRooms[i]; //get the script

            CompleteList_spawnedRooms[index].list.Add(roomScript); //add to the  complete list

            subscribeToRoom(roomScript, new Vector3Int(index, i, AreaIndex)); //Subscribe to this room
        }
        
    }
    void subscribeToRoom(Room_script room, Vector3Int completeIndex)
    {
        room.indexInCompleteList = completeIndex;
        room.onRoomEntered += playerEnteredSomeRoom;
        room.onRoomExited += playerExitedSomeRoom;
    }
    void playerEnteredSomeRoom(GameObject player, Room_script room)
    {
        gameState.currentPlayersRooms.Add(room);
        gameState.currentPlayerRooms_index.Add(room.indexInCompleteList);
    }
    void playerExitedSomeRoom(GameObject player, Room_script room)
    {
        gameState.currentPlayersRooms.Remove(room);
        gameState.currentPlayerRooms_index.Remove(room.indexInCompleteList);
    }
    private void OnDrawGizmosSelected()
    {
        
        if (gameState.currentPlayersRooms.Count > 0)
        {
            List<Room_script> PlayerRooms = gameState.currentPlayersRooms;

            BoxCollider2D groupCollider = GroupsOfRoomsList[PlayerRooms[0].indexInCompleteList.x].GetComponent<BoxCollider2D>();
            UsefullMethods.DrawCollider(groupCollider, Color.blue);

            for (int i = 0; i < PlayerRooms.Count; i++)
            {
                Room_script thisRoom = CompleteList_spawnedRooms[PlayerRooms[i].indexInCompleteList.x].list[PlayerRooms[i].indexInCompleteList.y].GetComponent<Room_script>();
                BoxCollider2D roomCollider = thisRoom.enterRoomCollider.GetComponent<BoxCollider2D>();
                UsefullMethods.DrawCollider(roomCollider, Color.cyan);
            }
        }
    }
}
