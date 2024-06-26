using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltoMando : MonoBehaviour
{

    [SerializeField] DoorAnimationController[] FourDoors;
    [SerializeField] EnterExitScene_controller[] EnterExitScene;
    [SerializeField] DoorAnimationController FinalDoor;
    [SerializeField] GameState gameState;
    int doorsCompleted;
    private void Awake()
    {
        doorsCompleted = 0;
        Handle4Doors();
        HandleFinalDoor();
    }
    void Handle4Doors()
    {
        for (int i = 0; i < gameState.FourDoors.Length; i++)
        {
            GameState.BossAreaDoor Door = gameState.FourDoors[i];
            Door.DoorController = FourDoors[i];

            Door.DoorController.DisableAutoDoorCloser();
            Door.DoorController.DisableAutoDoorOpener();
            FinalDoor.DisableAutoDoorOpener();
            FinalDoor.DisableAutoDoorCloser();

            EnterExitScene[i].playEnteringCutsceneOnLoad = i == gameState.LastCompletedIndex; //Player respawn on the last completed index

            if (!Door.isCompleted) //Uncompleted Doors
            {
                Door.DoorController.InstaOpen();
            }
            else if (i == gameState.LastCompletedIndex) //last completed door
            {
                doorsCompleted++;
                Door.DoorController.InstaOpen();
                Door.DoorController.CloseDoor();
            }
            else //Already completed doors
            {
                doorsCompleted++;
                Door.DoorController.InstaClose();
            }
        }
    }
    void HandleFinalDoor()
    {
        if (gameState.isFinalDoorOpen) //Final door is already completed
        {
            FinalDoor.InstaOpen();
        }
        else if (doorsCompleted == gameState.FourDoors.Length) //Final door just completed
        {
            FinalDoor.OpenDoor();
            gameState.isFinalDoorOpen = true;
        }
        else //Final door is not open
        {
            gameState.isFinalDoorOpen = false;
        }
    }


}
