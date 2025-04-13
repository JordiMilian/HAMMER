using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorLogic_Focus : BaseRoomWithDoorLogic
{
    [SerializeField] Focuseable TargetFocusMannequin;
    [SerializeField] Focuseable StartingFocusMannequin;
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
        Player_SwordRotationController swordRotation = GlobalPlayerReferences.Instance.references.swordRotation;
        swordRotation.FocusNewEnemy(StartingFocusMannequin);
    }
    void OnFocusedTarget()
    {
        RoomCompleted(true, true);
        TargetFocusMannequin.OnFocused -= OnFocusedTarget;
    }
    

}
