using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_OpenDoor : BaseCutsceneLogic
{
    [SerializeField] AnimationClip openDoorAnimation;
    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(OpenDoorFocusCamera());
    }

    IEnumerator OpenDoorFocusCamera()
    {
        DoorAnimationController doorController = GetComponent<DoorAnimationController>();
        
        Transform doorTransform = doorController.transform;

        yield return new WaitForSeconds(0.5f); //Wait after killing the last dude

        TargetGroupSingleton.Instance.AddTarget(doorTransform, 10, 5); //Look at door
        yield return new WaitForSeconds(0.2f); //Wait 
        doorController.OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        TargetGroupSingleton.Instance.RemoveTarget(doorTransform); // Stop looking at camera

        onCutsceneOver?.Invoke();
    }
}
