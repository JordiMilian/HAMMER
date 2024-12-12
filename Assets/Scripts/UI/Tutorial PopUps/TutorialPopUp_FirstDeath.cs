using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp_FirstDeath : MonoBehaviour
{
    [SerializeField] UI_TutorialPopUp_Script popUpScript;
    [SerializeField] GameState gameState;

    private void OnEnable()
    {
        GameEvents.OnPlayerReappear += OnPlayerRespawned;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerReappear -= OnPlayerRespawned;
    }

    void OnPlayerRespawned()
    {
        if(gameState.playerDeaths == 1 && !gameState.hadFirstDeath)
        {
            popUpScript.ShowPopUp();
            gameState.hadFirstDeath = true;
        }
    }
}
