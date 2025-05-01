using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltoMando_FadeControl : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] Rooms_FadeInOut mainFader;
    [SerializeField] Rooms_FadeInOut backFader;

    private void Awake()
    {
        /*
        if (gameState.isTutorialComplete && gameState.LastEnteredDoor < 0 || gameState.justDefeatedBoss)
        {
            mainFader.isStartingRoom = true;
            return;
        }
        else if(gameState.LastEnteredDoor >= 0)
        {
            backFader.isStartingRoom = true;
        }
        */

        
    }
}
