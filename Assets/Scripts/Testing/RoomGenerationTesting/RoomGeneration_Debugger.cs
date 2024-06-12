using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration_Debugger : MonoBehaviour
{
    [SerializeField] RoomGenerator_Manager manager;
    [SerializeField] bool Trigger_AllRooms, Trigger_SingleGroup, Trigger_StartingFromGroup;
    [SerializeField] int triggerIndex;

    private void Update()
    {
        if(Trigger_AllRooms)
        {
            manager.Call_GenerateAllRoomsFromPosition?.Invoke(transform.position);
            Trigger_AllRooms = false;
        }
        if(Trigger_SingleGroup)
        {
            manager.Call_GenerateSingleGroupOfRooms?.Invoke(triggerIndex);
            Trigger_SingleGroup = false;
        }
        if(Trigger_StartingFromGroup)
        {
            manager.Call_GenerateRooms_StartFromIndex?.Invoke(triggerIndex);
            Trigger_StartingFromGroup = false;
        }
    }
}
