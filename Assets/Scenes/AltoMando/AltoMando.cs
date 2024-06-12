using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltoMando : MonoBehaviour
{

    [SerializeField] DoorAnimationController[] FourDoors;
    [SerializeField] GameState gameState; 
    private void Awake()
    {
        for (int i = 0; i < gameState.FourDoors.Length; i++)
        {
            GameState.BossAreaDoor Door = gameState.FourDoors[i];
            Door.DoorController = FourDoors[i];
            Door.DoorController.DisableAutoDoorCloser();
            if (!Door.isCompleted)
            {
                Debug.Log("Door incompleted: " + i);

                Door.DoorController.OpenDoor();
            }
            else
            {
                Door.DoorController.CloseDoor();
                Door.DoorController.DisableAutoDoorOpener();
            }
        }
    }
    //public DoorAnimationController[] FourDoors;
    /*
    private void Awake()
    {
        foreach (BossAreaDoor bossDoor in FourDoors)
        {
            bossDoor.DoorController.DisableAutoDoorCloser();
            if (!bossDoor.isCompleted)
            {
                bossDoor.DoorController.OpenDoor();
            }
            else
            {
                bossDoor.DoorController.CloseDoor();
            }
        }
    }
    void EnableCloseDoor(int doorIndex)
    {
        FourDoors[doorIndex].DoorController.EnableAutoDoorCloser();
    }
    */

}
