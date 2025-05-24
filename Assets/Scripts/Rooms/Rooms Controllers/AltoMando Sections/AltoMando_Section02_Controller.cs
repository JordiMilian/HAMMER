using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltoMando_Section02_Controller : MonoBehaviour, IRoom, IMultipleRoom
{

    [SerializeField] DoorAnimationController doorController_A1;
    [SerializeField] DoorAnimationController doorController_A2;
    [SerializeField] DoorAnimationController doorController_A3;
    [SerializeField] GameState gameState;
    [Header("Multiple Rooms")]
    [SerializeField] Transform _tf_ExitPos;
    public Vector2 ExitPos => _tf_ExitPos.position;

    [SerializeField] Generic_OnTriggerEnterEvents _combinedCollider;
    public Generic_OnTriggerEnterEvents combinedCollider => _combinedCollider;

    public void OnRoomLoaded()
    {

        doorController_A1.EnableAutoDoorOpener();
        doorController_A2.DisableAutoDoorOpener();
        doorController_A3.DisableAutoDoorOpener();
        /*
        if (gameState.FourDoors[0].isCompleted) 
        {
            doorController_A1.InstaOpen();
            if (!gameState.FourDoors[1].isCompleted) { doorController_A2.EnableAutoDoorOpener(); }
        }
        if (gameState.FourDoors[1].isCompleted) 
        {
            doorController_A2.InstaOpen();
            if (!gameState.FourDoors[2].isCompleted) { doorController_A3.EnableAutoDoorOpener(); }
        }
        if (gameState.FourDoors[2].isCompleted) { doorController_A3.InstaOpen(); }
        */
    }

    public void OnRoomUnloaded()
    {
    }
}
