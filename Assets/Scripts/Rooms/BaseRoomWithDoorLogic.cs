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
    [SerializeField] AnimationClip openDoorAnimation;
    public bool isRoomPermanentlyCompleted;
    public Action<BaseRoomWithDoorLogic> onRoomCompleted;
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
        if (isRoomPermanentlyCompleted) { return; }
        if (withAnimation) { StartCoroutine(OpenDoorFocusCamera()); }
        if (isRoomPermanent) { isRoomPermanentlyCompleted = true; }
        else { return; }

        //Activate the trigger to Reopen Door
        doorController.EnableAutoDoorOpener();

    }
    IEnumerator OpenDoorFocusCamera()
    {
        Transform doorTransform = doorController.transform;

        yield return new WaitForSeconds(0.5f); //Wait after killing the last dude

        TargetGroupSingleton.Instance.AddTarget(doorTransform, 10, 5); //Look at door
        yield return new WaitForSeconds(0.2f); //Wait 
        doorController.OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        TargetGroupSingleton.Instance.RemoveTarget(doorTransform); // Stop looking at camera
    }
}
