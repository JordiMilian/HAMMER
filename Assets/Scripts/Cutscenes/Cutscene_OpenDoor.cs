using Pathfinding.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_OpenDoor : BaseCutsceneLogic, ICutsceneable
{
    [SerializeField] AnimationClip openDoorAnimation;
    [SerializeField] DoorAnimationController doorController;
    public IEnumerator ThisCutscene()
    {
        Transform doorTransform = doorController.transform;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);

        yield return new WaitForSeconds(0.5f); //Wait after killing the last dude

        TargetGroupSingleton.Instance.AddTarget(doorTransform, 10, 5); //Look at door
        yield return new WaitForSeconds(0.2f); //Wait 
        doorController.OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        TargetGroupSingleton.Instance.RemoveTarget(doorTransform); // Stop looking at camera

        playerStateMachine.ForceChangeState(playerRefs.IdleState);
    }
    public void ForceEndCutscene()
    {
        Transform doorTransform = doorController.transform;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        doorController.OpenDoor();
        TargetGroupSingleton.Instance.RemoveTarget(doorTransform);
        playerStateMachine.ForceChangeState(playerRefs.IdleState);
    }

    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(OpenDoorFocusCamera());
    }

    

    IEnumerator OpenDoorFocusCamera()
    {
       
        Transform doorTransform = doorController.transform;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);

        yield return new WaitForSeconds(0.5f); //Wait after killing the last dude

        TargetGroupSingleton.Instance.AddTarget(doorTransform, 10, 5); //Look at door
        yield return new WaitForSeconds(0.2f); //Wait 
        doorController.OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        TargetGroupSingleton.Instance.RemoveTarget(doorTransform); // Stop looking at camera

        playerStateMachine.ForceChangeState(playerRefs.IdleState);

        onCutsceneOver?.Invoke();
    }
}
