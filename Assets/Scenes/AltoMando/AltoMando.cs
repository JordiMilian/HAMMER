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
    private void Awake()
    {
        int doorsOpen = 0;
        for (int i = 0; i < gameState.FourDoors.Length; i++)
        {
            GameState.BossAreaDoor Door = gameState.FourDoors[i];
            Door.DoorController = FourDoors[i];
            Door.DoorController.DisableAutoDoorCloser();
            FinalDoor.DisableAutoDoorOpener();
            FinalDoor.DisableAutoDoorCloser();

            EnterExitScene[i].playEnteringCutsceneOnLoad = i == gameState.LastCompletedIndex;

            
            if (!Door.isCompleted)
            {
                Debug.Log("Door incompleted: " + i);

                Door.DoorController.OpenDoor();
            }
            else
            {
                doorsOpen++;
                Door.DoorController.CloseDoor();
                Door.DoorController.DisableAutoDoorOpener();
            }
        }
        if(doorsOpen == gameState.FourDoors.Length || !gameState.isFinalDoorOpen)
        {
            FinalDoor.OpenDoor();
            gameState.isFinalDoorOpen = true;
            FinalDoor.EnableAutoDoorOpener();
            //Algo que deixe la porta oberta sense haber de passar per al animacio porfaaa
        }
    }


}
