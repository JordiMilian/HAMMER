using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_DeathUI : MonoBehaviour
{
    [SerializeField] Transform DeathUIRoot;
    [SerializeField] PlayerState_Death playerDeathState;
    private void OnEnable()
    {
        DeathUIRoot.gameObject.SetActive(false);
    }
    //Called from UI
    public void Button_SpawnAgain()
    {
        DeathUIRoot.gameObject.SetActive(false);
        playerDeathState.Button_RespawnPlayer();
    }
    public void EndRun()
    {
        DeathUIRoot.gameObject.SetActive(false);
        playerDeathState.Button_RestartRun();
    }
}
