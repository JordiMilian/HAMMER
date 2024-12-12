using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene_Death_SetUpResetState : BaseCutsceneLogic
{
    [SerializeField] GameState gameState;
    [SerializeField]
    public override void playThisCutscene()
    {
        gameState.NewGameResetState();

        gameState.isSpawnWithouUpgrades = true;

        onCutsceneOver?.Invoke();

        SceneManager.LoadScene("AltoMando");
    }
}
