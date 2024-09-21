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
    private void OnDisable()
    {
        respawner.OnRespawnerActivated -= respawnerActivated;
    }
    void respawnerActivated()
    {
        RoomCompleted(false, true);
        ExitDoorGate.OpenDoor();
    }

}
