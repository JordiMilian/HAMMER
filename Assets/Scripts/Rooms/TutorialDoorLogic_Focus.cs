using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorLogic_Focus : BaseRoomWithDoorLogic
{
    [SerializeField] FocusIcon TargetFocusMannequin;
    [SerializeField] FocusIcon StartingFocusMannequin;
    [SerializeField] Generic_OnTriggerEnterEvents getCloseToMannequinTrigger;

    public override void OnEnable()
    {
        base.OnEnable();

        TargetFocusMannequin.OnFocused += OnFocusedTarget;

        getCloseToMannequinTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        getCloseToMannequinTrigger.OnTriggerEntered += AutoFocusMannequin;
    }
    void AutoFocusMannequin(Collider2D collider)
    {
        Player_FollowMouseWithFocus_V2 playerFocus = GlobalPlayerReferences.Instance.references.followMouse;
        playerFocus.FocusNewEnemy(StartingFocusMannequin);
    }
    void OnFocusedTarget()
    {
        RoomCompleted(true, true);
        TargetFocusMannequin.OnFocused -= OnFocusedTarget;
    }
    

}
