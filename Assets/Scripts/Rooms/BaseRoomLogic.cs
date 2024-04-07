using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_OnTriggerEnterEvents;

public class BaseRoomLogic : MonoBehaviour
{
    [Header ("Door opening stuff")]
    [SerializeField] DoorAnimationController doorController;
    [SerializeField] Generic_OnTriggerEnterEvents reopenDoorTrigger;
    [SerializeField] AnimationClip openDoorAnimation;
    public bool isRoomPermanentlyCompleted;
    public virtual void OnEnable()
    {
        reopenDoorTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        reopenDoorTrigger.OnTriggerEntered += ReopenDoor;

        //If the room is completed, complete, else dont let the door open
        if (isRoomPermanentlyCompleted) { RoomCompleted(false,true); }
        else { reopenDoorTrigger.GetComponent<Collider2D>().enabled = false; }
    }
    public void RoomCompleted(bool withAnimation, bool isRoomPermanent)
    {
        if (isRoomPermanentlyCompleted) {return; }
        if (withAnimation) { StartCoroutine(OpenDoorFocusCamera()); }
        if (isRoomPermanent) { isRoomPermanentlyCompleted = true; }

        //Activate the trigger to Reopen Door
        reopenDoorTrigger.GetComponent<BoxCollider2D>().enabled = true;

    }
    public void ReopenDoor(object sender, EventArgsCollisionInfo args)
    {
        doorController.OpenDoor();
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
