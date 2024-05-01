using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LoadGreenBossRoom : UI_BaseAction
{
    public override void Action(UI_Button button)
    {
        SceneManager.LoadScene("BossScene",LoadSceneMode.Single);
    }
}
