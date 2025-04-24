using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRoom_Focus_Controller : MonoBehaviour, IRoom, IMultipleRoom
{
    [SerializeField] Focuseable TargetFocusMannequin;
    [SerializeField] Focuseable StartingFocusMannequin;
    [SerializeField] Generic_OnTriggerEnterEvents getCloseToMannequinTrigger;
    [SerializeField] DoorAnimationController exitDoorController;

    void AutoFocusMannequin(Collider2D collider)
    {
        getCloseToMannequinTrigger.OnTriggerEntered -= AutoFocusMannequin;
        Player_SwordRotationController swordRotation = GlobalPlayerReferences.Instance.references.swordRotation;
        swordRotation.FocusNewEnemy(StartingFocusMannequin);
    }
    void OnFocusedTarget()
    {
        exitDoorController.OpenWithCutscene();
        TargetFocusMannequin.OnFocused -= OnFocusedTarget;
    }

    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;
    public void OnRoomLoaded()
    {
        TargetFocusMannequin.OnFocused += OnFocusedTarget;

        getCloseToMannequinTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        getCloseToMannequinTrigger.OnTriggerEntered += AutoFocusMannequin;
        exitDoorController.DisableAutoDoorOpener();
    }
    public void OnRoomUnloaded()
    {
        TargetFocusMannequin.OnFocused -= OnFocusedTarget;
    }
}
