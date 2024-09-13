using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor_DialoguesControler : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] Dialoguer finalDoor_Dialoguer;
    private void Awake()
    {
        SetNewContainerIndex();
    }
    void SetNewContainerIndex()
    {
        finalDoor_Dialoguer.dialoguesIndex = gameState.finalDoor_DialogueIndex; 


        //Not read anything
        if(gameState.finalDoor_DialogueIndex == 0)
        {
            finalDoor_Dialoguer.onFinishedReading += onReadFirst;
        }

        //defeated any boss
        if(gameState.justDefeatedBoss)
        {
            if(gameState.LastCompletedBoss == 0) { SetStateDialoguer(2); }
            if(gameState.LastCompletedBoss == 1) { SetStateDialoguer(3); }

            finalDoor_Dialoguer.onFinishedReading += onReadBossFinished;
        }

        void onReadFirst()
        {
            SetStateDialoguer(1);
            finalDoor_Dialoguer.onFinishedReading -= onReadFirst;
        }

        void onReadBossFinished()
        {
            SetStateDialoguer(4);
            finalDoor_Dialoguer.onFinishedReading -= onReadBossFinished;
        }
    }

    void SetStateDialoguer(int index)
    {
        gameState.finalDoor_DialogueIndex = index;
        finalDoor_Dialoguer.dialoguesIndex = index;
        finalDoor_Dialoguer.UpdateDialogues();
    }
}
