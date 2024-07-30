using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    //IS THIS SCRIPT DEPRECATED WTF???!?!?!!?!?!?!!?

    [SerializeField] BaseCutsceneLogic ResetStateCutscene;
    [SerializeField] BaseCutsceneLogic noResetCutscene;

    [SerializeField] GameState gameState;
    private void OnEnable()
    {
        GlobalPlayerReferences.Instance.playerTf.GetComponent<Cutscene_PlayersStartingDeath>().onCutsceneOver += OnManageReset;
    }
     void OnManageReset()
    {
        CutscenesManager.Instance.AddCutscene(noResetCutscene);

        //Testing never reseting
        /* 
        if (checkIfReset())
        {
            CutscenesManager.Instance.AddCutscene(ResetStateCutscene); 
        }
        else
        {
            CutscenesManager.Instance.AddCutscene(noResetCutscene);
        }
        */
    }

    bool checkIfReset()
    {
        return gameState.playerUpgrades.Count == 0;
    }
}
