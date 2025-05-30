using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cutscene_OpenDoor_Boss : BaseCutsceneLogic 
{
    [SerializeField] Animator BossDefeatedUI_animator;
    [SerializeField] AnimationClip BossDefeatedUiClip;
    [SerializeField] AnimationClip openDoorAnimation;
    [SerializeField] DoorAnimationController doorController;
    [SerializeField] GameState gameState;
    [SerializeField] float delayBeforeUI;
    [SerializeField] float delayBeforeDoor;
    [SerializeField] UpgradesGroup upgradeGroup;
    [SerializeField] bool skipUpgrades;
    public override void playThisCutscene()
    {
        if (skipUpgrades) { StartCoroutine(BossRoomFinishCutscene_NoUpgrades()); }
        else
        {
            currentCutscene = StartCoroutine(BossRoomFinishedCutscene());
        }
        
    }
    IEnumerator BossRoomFinishedCutscene()
    {
        Transform doorTransform = doorController.transform;
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;

        //Fade out music when boss defeated
        int indexOfGroup = gameState.currentPlayerRooms_index[gameState.currentPlayerRooms_index.Count - 1].x;
        RoomsGroup_script thisGroup = RoomGenerator_Manager.Instance.GroupsOfRoomsList[indexOfGroup];
        Audio_Area audioArea = thisGroup.GetComponentInChildren<Audio_Area>();
        audioArea.onFadeOutAudio(new Collider2D());

        yield return new WaitForSeconds(delayBeforeUI);

        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);
        BossDefeatedUI_animator.SetTrigger("BossDefeated");//Placeholder name?
        yield return new WaitForSeconds(BossDefeatedUiClip.length);


        yield return new WaitForSeconds(delayBeforeDoor); //Wait before door

       

        targetGroup.SetOnlyOneTarget(doorTransform, 10, 5); //Look at door
        yield return new WaitForSeconds(0.2f); //Wait 
        doorController.OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        targetGroup.ReturnPlayersTarget();
        targetGroup.RemoveTarget(doorTransform); // Stop looking at camera


        playerStateMachine.ForceChangeState(playerRefs.IdleState);
        upgradeGroup.StartSpawnCutscene(); //Cutrada maxima aixo ho hauries de cridar desde algun altre lloc. Arreglaho quan refactoritses les cutscenes pls

        onCutsceneOver?.Invoke();

    }
    IEnumerator BossRoomFinishCutscene_NoUpgrades()
    {
        Transform doorTransform = doorController.transform;
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

       
        TargetGroupSingleton targetGroup = TargetGroupSingleton.Instance;

        //Fade out music when boss defeated
        int indexOfGroup = gameState.currentPlayerRooms_index[gameState.currentPlayerRooms_index.Count - 1].x;
        RoomsGroup_script thisGroup = RoomGenerator_Manager.Instance.GroupsOfRoomsList[indexOfGroup];
        Audio_Area audioArea = thisGroup.GetComponentInChildren<Audio_Area>();
        audioArea.onFadeOutAudio(new Collider2D());

        yield return new WaitForSeconds(delayBeforeUI);

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);
        BossDefeatedUI_animator.SetTrigger("BossDefeated");//Placeholder name?
        yield return new WaitForSeconds(BossDefeatedUiClip.length);


        yield return new WaitForSeconds(delayBeforeDoor); //Wait before door



        targetGroup.SetOnlyOneTarget(doorTransform, 10, 5); //Look at door
        yield return new WaitForSeconds(0.2f); //Wait 
        doorController.OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        targetGroup.ReturnPlayersTarget();
        targetGroup.RemoveTarget(doorTransform); // Stop looking at camera


        playerStateMachine.ForceChangeState(playerRefs.IdleState);
        //upgradeGroup.StartSpawnCutscene(); //Cutrada maxima aixo ho hauries de cridar desde algun altre lloc. Arreglaho quan refactoritses les cutscenes pls
        onCutsceneOver?.Invoke();

    }
}
