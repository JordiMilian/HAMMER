using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AltoMando_Section01_Controller : MonoBehaviour, IRoom, IMultipleRoom
{
    [SerializeField] DoorAnimationController EnterDoor;

    [Header("Multiple rooms")]
    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;
    [SerializeField] FinalDoor_Script FinalDoor;

    public void OnRoomLoaded()
    {
        //Check weapons to display
        FinalDoor.CheckStateAndUpdateDoor();
        EnterDoor.InstaOpen();
        EnterDoor.CloseDoor();
        EnterDoor.DisableAutoDoorCloser();
    }

    public void OnRoomUnloaded()
    {
    }


}
