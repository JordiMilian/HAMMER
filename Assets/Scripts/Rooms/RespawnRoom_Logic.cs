using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnRoom_Logic : BaseRoomWithDoorLogic
{
    [SerializeField] Player_Respawner respawner;
    [SerializeField] DoorAnimationController ExitDoorGate;
    public override void OnEnable()
    {
        base.OnEnable();
        respawner.OnRespawnerActivated += respawnerActivated;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        respawner.OnRespawnerActivated -= respawnerActivated;
    }
    void respawnerActivated()
    {
        RoomCompleted(false, true);

        if (ExitDoorGate != null) { ExitDoorGate.OpenDoor(); } //Aixo tecnicament hauria de ser una extensio amb herencia o yo que se pero bueno
    }

}
