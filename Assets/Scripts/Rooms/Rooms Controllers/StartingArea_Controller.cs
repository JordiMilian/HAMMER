using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingArea_Controller : MonoBehaviour, IMultipleRoom, IRoom
{
    [SerializeField] PlayerState startingPlayerState;


    #region MULTIPLE ROOM
    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;

    #endregion

    public void OnRoomLoaded()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        playerRefs.stateMachine.ForceChangeState(playerRefs.RespawningState);
        //cutsceneable zooming in and letting the player etc
        //maybe the player need a state for this?
    }

    public void OnRoomUnloaded()
    {

    }
}
