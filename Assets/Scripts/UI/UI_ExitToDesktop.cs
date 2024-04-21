using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ExitToDesktop : UI_BaseAction
{
    public override void Action(UI_Button button)
    {
        Debug.Log("Exited");
        Application.Quit();
    }
}
