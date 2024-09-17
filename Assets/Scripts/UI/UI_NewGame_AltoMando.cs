using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_NewGame_AltoMando : UI_BaseAction
{
    [SerializeField] GameState gameState;
    public override void Action(UI_Button button)
    {
        gameState.ResetState();
        SceneManager.LoadScene("AltoMando_generated", LoadSceneMode.Single);
    }
}
