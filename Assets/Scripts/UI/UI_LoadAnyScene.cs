using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LoadAnyScene : UI_BaseAction
{
    [SerializeField] string SceneName;
    public override void Action(UI_Button button)
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }
}
