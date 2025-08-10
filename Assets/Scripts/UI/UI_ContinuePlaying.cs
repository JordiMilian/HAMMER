using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ContinuePlaying : UI_BaseAction
{
    public override void Action(UI_Button button)
    {
        //find the pauseGame script in root and unpause
        button.transform.root.GetComponent<PauseGame>().Unpause_andHidePauseUI();
    }
}
