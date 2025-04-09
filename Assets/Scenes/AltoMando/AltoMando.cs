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
    [SerializeField] TiedEnemy_Controller tutorialEndRespawner;
    [SerializeField] Cutscene_Death_ResetState ResetStateCutscene;
    int doorsCompleted;
    private void Awake()
    {
        doorsCompleted = 0;
        //Handle4Doors();
        Handle4DoorInOrder();
        HandleFinalDoor();

        if (gameState.isTutorialComplete && gameState.LastEnteredDoor < 0 || gameState.justDefeatedBoss )
        {
            tutorialEndRespawner.ActivateRespawner();

            //Revisar tot aixo un altre dia
            /*
            if (gameState.isSpawnWithouUpgrades)
            {
                ResetStateCutscene.dontResetState = false;
            }
            else
            {
                ResetStateCutscene.dontResetState = true;
            }
            CutscenesManager.Instance.AddCutscene(ResetStateCutscene);
            */
        }

        gameState.isSpawnWithouUpgrades = false;

        tutorialEndRespawner.OnRespawnerActivated += TutorialCompleted;
    }
    private void OnDisable()
    {
        tutorialEndRespawner.OnRespawnerActivated -= TutorialCompleted;
        if (gameState.justDefeatedBoss) { gameState.justDefeatedBoss = false; }
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

            if (!gameState.justDefeatedBoss)
            {
                EnterExitScene[i].playEnteringCutsceneOnLoad = i == gameState.LastEnteredDoor; //Player respawn on the last exited index
            }


            if (!Door.isCompleted) //Uncompleted Doors
            {
                Door.DoorController.InstaOpen();
            }
            else if (i == gameState.LastEnteredDoor) //last completed door
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
    void Handle4DoorInOrder()
    {
        int lastCompletedIndex = -1;
        for (int i = 0; i < gameState.FourDoors.Length; i++)
        {
            GameState.BossAreaDoor Door = gameState.FourDoors[i];
            Door.DoorController = FourDoors[i];

            Door.DoorController.DisableAutoDoorCloser();
            Door.DoorController.DisableAutoDoorOpener();
            FinalDoor.DisableAutoDoorOpener();
            FinalDoor.DisableAutoDoorCloser();

            if (!gameState.justDefeatedBoss)
            {
                EnterExitScene[i].playEnteringCutsceneOnLoad = i == gameState.LastEnteredDoor; //Player respawn on the last exited index
            }

            if (Door.isCompleted) { lastCompletedIndex = i; }

            Door.DoorController.InstaClose();
        }
        //if all completed nothing, else open the next to the last completed
        if(lastCompletedIndex == gameState.FourDoors.Length - 1) { return; }
        else { gameState.FourDoors[lastCompletedIndex + 1 ].DoorController.OpenDoor(); }
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

    void TutorialCompleted()
    {
        gameState.isTutorialComplete = true;
    }
}
