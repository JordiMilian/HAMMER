using Pathfinding.Examples;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class BaseRoomWithDoorLogic : MonoBehaviour
{
    [Header ("Door opening stuff")]
    [SerializeField] DoorAnimationController doorController;
    public bool isRoomPermanentlyCompleted;
    public Action<BaseRoomWithDoorLogic> onRoomCompleted;

    [SerializeField] BaseCutsceneLogic openDoorCutscene;
    public virtual void OnEnable()
    {
        //If the room is completed, complete, else dont let the door open
        if (isRoomPermanentlyCompleted) { RoomCompleted(false,true); }

        else { doorController.DisableAutoDoorOpener(); }
    }
    public virtual void OnDisable()
    {
        
    }
    public void RoomCompleted(bool withAnimation = false, bool isRoomPermanent = false)
    {
        onRoomCompleted?.Invoke(this);
        //if (isRoomPermanentlyCompleted) { return; }
        if (withAnimation) { CutscenesManager.Instance.AddCutscene(openDoorCutscene); }
        if (isRoomPermanent) { isRoomPermanentlyCompleted = true; }
        else { return; }

        //Activate the trigger to Reopen Door
        doorController.EnableAutoDoorOpener();

    }
}
