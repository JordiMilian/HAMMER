using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PauseGame : MonoBehaviour
{
    [HideInInspector] public static bool isPaused;
    Transform MouseTarget;
    [SerializeField] Canvas pauseCanvas;
    [SerializeField] UI_ControllerControl controllerControls;

    public Action OnPauseMenu;
    public Action OnUnpauseMenu;
    public Action CallPauseGame;
    public Action CallUnpauseGame;


    private void OnEnable()
    {
        InputDetector.Instance.OnPausePressed += onPausePresed;
    }
    private void OnDisable()
    {
        InputDetector.Instance.OnPausePressed -= onPausePresed;
    }
    void onPausePresed()
    {
        switch (isPaused)
        {
            case false:
                PauseGame_();
                break;

            case true:
                UnpauseGame();
                break;
        }
    }
    public void PauseGame_()
    {
        Debug.Log("paused?");
        isPaused = true; //bool for switch
        Time.timeScale = 0; //stop time
        TargetGroupSingleton.Instance.RemovePlayersTarget(); //Remove mouse influence to TargetGroup
    }
    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        
    }
    public void Pause_andShowPauseUI()
    {
        PauseGame_();

        pauseCanvas.enabled = true; //show UIs
        OnPauseMenu?.Invoke(); //event
        controllerControls.RestartSelection(); //have the first button selected when opening menu
        controllerControls.isReadingInput = true;
    }
    public void Unpause_andHidePauseUI()
    {
        UnpauseGame();

        pauseCanvas.enabled = false;
        OnUnpauseMenu?.Invoke();
        controllerControls.isReadingInput = false;
    }
    public void exit()
    {
        UnpauseGame();
        Application.Quit();
    }
    public void die()
    {
        UnpauseGame();
        Player_StateMachine playerStateMachine = GlobalPlayerReferences.Instance.playerTf.GetComponent<Player_StateMachine>();
        PlayerState deathState = playerStateMachine.GetComponent<Player_References>().DeadState;
        playerStateMachine.ForceChangeState(deathState);
    }
    public void restartLevel()
    {
        UnpauseGame();
        SceneManager.LoadScene("GutWhale");
    }

}
