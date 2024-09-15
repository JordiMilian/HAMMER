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
    public override void playThisCutscene()
    {
        currentCutscene = StartCoroutine(BossRoomFinishedCutscene());
        onCutsceneOver?.Invoke();
    }
    IEnumerator BossRoomFinishedCutscene()
    {
        Transform doorTransform = doorController.transform;
        Player_EventSystem playerEvents = GlobalPlayerReferences.Instance.references.events;

        //Fade out music when boss defeated
        int indexOfGroup = gameState.currentPlayerRooms_index[gameState.currentPlayerRooms_index.Count - 1].x;
        RoomsGroup_script thisGroup = RoomGenerator_Manager.Instance.GroupsOfRoomsList[indexOfGroup];
        Audio_Area audioArea = thisGroup.GetComponentInChildren<Audio_Area>();
        audioArea.onFadeOutAudio(new Collider2D());

        yield return new WaitForSeconds(delayBeforeUI);

        playerEvents.CallDisable();
        BossDefeatedUI_animator.SetTrigger("BossDefeated");//Placeholder name?
        yield return new WaitForSeconds(BossDefeatedUiClip.length);


        yield return new WaitForSeconds(delayBeforeDoor); //Wait before door

        TargetGroupSingleton.Instance.AddTarget(doorTransform, 10, 5); //Look at door
        yield return new WaitForSeconds(0.2f); //Wait 
        doorController.OpenDoor(); //Open door
        yield return new WaitForSeconds(openDoorAnimation.length + 0.3f); //wait for door animation
        TargetGroupSingleton.Instance.RemoveTarget(doorTransform); // Stop looking at camera

        playerEvents.CallEnable();

        onCutsceneOver?.Invoke();
    }
}
