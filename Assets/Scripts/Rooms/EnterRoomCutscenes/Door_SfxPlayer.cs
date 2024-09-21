using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_SfxPlayer : Base_SfxPlayer
{
    [SerializeField] AudioClip OpenDoorSFX, CloseDoorSFX;
    [SerializeField] DoorAnimationController doorController;

    private void OnEnable()
    {
        doorController.OnDoorOpen += playOpenDoor;
        doorController.OnDoorClose += playCloseDoor;
    }
    private void OnDisable()
    {
        doorController.OnDoorOpen -= playOpenDoor;
        doorController.OnDoorClose -= playCloseDoor;
    }
    void playOpenDoor()
    {
        SFX_PlayerSingleton.Instance.playSFX(OpenDoorSFX);
    }
    void playCloseDoor()
    {
        SFX_PlayerSingleton.Instance.playSFX(CloseDoorSFX);
    }
}
