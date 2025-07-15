using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnRoom_Controller : MonoBehaviour, IRoom
{
    [SerializeField] TiedEnemy_Controller respawner;
    [SerializeField] DoorAnimationController exitDoorController;
    [SerializeField] DoorAnimationController enterDoorController;

    void respawnerActivated()
    {
        exitDoorController.EnableAutoDoorOpener(); 
    }
    public void OnRoomLoaded()
    {
        //check if its respawning or not and remove the respaners Head or not
        //if its respawning, remove head and enable door, also, if respawning, intaClose Door
        //else subscibe to respawner activated, close enter door with animation

        enterDoorController.CloseDoor();
        exitDoorController.DisableAutoDoorOpener();
        respawner.OnRespawnerActivated += respawnerActivated;
        GlobalPlayerReferences.Instance.references.stateMachine.ForceChangeState(GlobalPlayerReferences.Instance.references.IdleState);

    }

    public void OnRoomUnloaded()
    {
        respawner.OnRespawnerActivated -= respawnerActivated;
    }

    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;
    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;
}
