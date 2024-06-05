using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RoomGenerator_Manager : MonoBehaviour
{
    [SerializeField] bool triggerGenerateRooms;
    [SerializeField] bool triggerSingleGroupRandomizer;
    [SerializeField] int SingleGroupRandomizerIndex;

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
    public List<GroupList> completeList = new List<GroupList>();

    [Header("Read only")]
    public List<Vector2Int> PlayerRooms; 

    private void Update()
    {
        if(triggerGenerateRooms)
        {
            if(isPlayerInAnyRoom())
            {
                GenerateAllRooms_startFromIndex(PlayerRooms[0].x + 1);
            }
            else
            {
                GenerateAllRooms_startFromIndex(0);
            }
            triggerGenerateRooms = false;
        }
        if(triggerSingleGroupRandomizer)
        {
            GenerateGroup(SingleGroupRandomizerIndex);
            UpdateInitialPositions(SingleGroupRandomizerIndex + 1);
            triggerSingleGroupRandomizer = false;
        }
    }
    void GenerateAllRooms_startFromIndex(int index)
    {
        //Create empty Lists if they dont exist yet
        int necesaryIndexes = GroupsOfRoomsList.Count;
        if (completeList.Count < necesaryIndexes)
        {
            for (int i = 0; i < necesaryIndexes; i++)
            {
                completeList.Add(new GroupList());
            }
        }


        for (int i = index; i < GroupsOfRoomsList.Count; i++)
        {
            GenerateGroup(i);
        }
    }
    void UpdateInitialPositions(int index)
    {
        for (int i = index; i < GroupsOfRoomsList.Count; i++)
        {
            RoomsGroup_script thisGroup = GroupsOfRoomsList[i];
            thisGroup.gameObject.transform.position = GroupsOfRoomsList[i - 1].LastExitTf;
            thisGroup.LastExitTf = thisGroup.currentlySpawnedRooms[thisGroup.currentlySpawnedRooms.Count - 1].ExitPosition.position;
        }
    }
    void GenerateGroup(int index)
    {
        Vector2 tempInitialPosition = transform.position;
        if (index == 0) { }

        else if(GroupsOfRoomsList[index - 1].transform != null)
        {
            tempInitialPosition = GroupsOfRoomsList[index - 1].LastExitTf;
        }

        GroupsOfRoomsList[index].RespawnRooms(tempInitialPosition); //Instantiate the chosen indexed group of rooms


        //Refill the subLists
        completeList[index].list.Clear();

        //Go throw all the rooms in that group to do stuff
        for (int i = 0; i < GroupsOfRoomsList[index].currentlySpawnedRooms.Count; i++)
        {
            Room_script roomScript = GroupsOfRoomsList[index].currentlySpawnedRooms[i]; //get the script

            completeList[index].list.Add(roomScript); //add to the  complete list

            subscribeToRoom(roomScript, new Vector2Int(index, i)); //Subscribe to this room
        }
        
    }
    void subscribeToRoom(Room_script room, Vector2Int completeIndex)
    {
        room.indexInCompleteList = completeIndex;
        room.OnPlayerEnteredRoom += playerEnteredSomeRoom;
        room.OnPlayerExitedRoom += playerExitedSomeRoom;
    }
    void playerEnteredSomeRoom(Vector2Int index)
    {
        PlayerRooms.Add(index);
    }
    void playerExitedSomeRoom(Vector2Int index)
    {
        PlayerRooms.Remove(index);
    }
    private void OnDrawGizmos()
    {
        if(PlayerRooms.Count > 0)
        {
            BoxCollider2D groupCollider = GroupsOfRoomsList[PlayerRooms[0].x].GetComponent<BoxCollider2D>();
            UsefullMethods.DrawCollider(groupCollider, Color.blue);

            for (int i = 0; i < PlayerRooms.Count; i++)
            {
                BoxCollider2D roomCollider = completeList[PlayerRooms[i].x].list[PlayerRooms[i].y].GetComponent<BoxCollider2D>();
                UsefullMethods.DrawCollider(roomCollider, Color.cyan);
            }
        }
        
    }
    bool isPlayerInAnyRoom()
    {
       return PlayerRooms.Count > 0; 
    }

}
