using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_OpenDoor : BaseCutsceneLogic
{
    [SerializeField] AnimationClip openDoorAnimation;
    [SerializeField] DoorAnimationController doorController;
    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(OpenDoorFocusCamera());
    }

    IEnumerator OpenDoorFocusCamera()
    {
       
        Transform doorTransform = doorController.transform;
        Player_EventSystem playerEvents = GlobalPlayerReferences.Instance.references.events;

        playerEvents.CallDisable();

        yield return new WaitForSeconds(0.5f); //Wait after killing the last dude

        TargetGroupSingleton.Instance.AddTarget(doorTransform, 10, 5); //Look at door
        yield return new WaitForSeconds(0.2f); //Wait 
        doorController.OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        TargetGroupSingleton.Instance.RemoveTarget(doorTransform); // Stop looking at camera

        playerEvents.CallEnable();

        onCutsceneOver?.Invoke();
    }
}
