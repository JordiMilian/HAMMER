using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    [SerializeField] BaseCutsceneLogic ResetStateCutscene;
    [SerializeField] BaseCutsceneLogic noResetCutscene;

    [SerializeField] GameState gameState;
    private void OnEnable()
    {
        GlobalPlayerReferences.Instance.playerTf.GetComponent<Cutscene_PlayersStartingDeath>().onCutsceneOver += OnManageReset;
    }
     void OnManageReset()
    {
        if(checkIfReset())
        {
            CutscenesManager.Instance.AddCutscene(ResetStateCutscene); 
        }
        else
        {
            CutscenesManager.Instance.AddCutscene(noResetCutscene);
        }
    }

    bool checkIfReset()
    {
        return gameState.playerUpgrades.Count == 0;
    }
}
