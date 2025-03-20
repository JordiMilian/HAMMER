using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp_FirstDeath : MonoBehaviour
{
    [SerializeField] UI_TutorialPopUp_Script popUpScript;
    [SerializeField] GameState gameState;

    UI_LevelUpSystemMenu levelUp;
    private void OnEnable()
    {
        GameEvents.OnPlayerRespawned += OnPlayerRespawned;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerRespawned -= OnPlayerRespawned;
    }

    void OnPlayerRespawned()
    {
        if(gameState.playerDeaths == 1 && !gameState.hadFirstDeath)
        {
            popUpScript.ShowPopUp();
            gameState.hadFirstDeath = true;

            levelUp =  RespawnersManager.Instance.GetFurthestActiveRespawner().GetComponentInChildren<UI_LevelUpSystemMenu>();

            levelUp.SetLevelUpSystemUnavailable();
            popUpScript.OnHiddenPopUp += ReturnLevelUpAvailable;
        }
    }
    void ReturnLevelUpAvailable() //Hauriem de fer un UI o PopUp Manager plsss en comtes daixo
    {
        StartCoroutine(delayToActivateLevelUp());
        popUpScript.OnHiddenPopUp -= ReturnLevelUpAvailable;
    }
    IEnumerator delayToActivateLevelUp()
    {
        yield return new WaitForSeconds(.5f);
        levelUp.SetLevelUpSystemAvailable();
    }
}
