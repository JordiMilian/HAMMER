using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorLogic_Focus : BaseRoomWithDoorLogic
{
    [SerializeField] GameObject TargetFocusMannequin;
    [SerializeField] GameObject StartingFocusMannequin;
     Player_EventSystem playerEvents;
    [SerializeField] Generic_OnTriggerEnterEvents getCloseToMannequinTrigger;
    bool isCompleted;

    public override void OnEnable()
    {
        base.OnEnable();

        playerEvents = GlobalPlayerReferences.Instance.references.events;
       
        playerEvents.OnFocusEnemy += FocusedEnemy;

        getCloseToMannequinTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        getCloseToMannequinTrigger.OnTriggerEntered += AutoFocusMannequin;
    }
    void AutoFocusMannequin(Collider2D collider)
    {
        Player_FollowMouseWithFocus_V2 playerFocus = GlobalPlayerReferences.Instance.references.followMouse;
        playerFocus.FocusNewEnemy(StartingFocusMannequin.gameObject);
    }
    void FocusedEnemy(GameObject focusedGO)
    {
        if(focusedGO == TargetFocusMannequin)
        {
            RoomCompleted(true, true);
            playerEvents.OnFocusEnemy -= FocusedEnemy;
        }
    }
    

}
