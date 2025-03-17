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

    [Header("Buttons references")]
    [SerializeField] Player_EventSystem playerEventSystem;

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
                Pause();
                break;

            case true:
                Unpause();
                break;
        }
    }
    void Pause()
    {
        isPaused = true; //bool for switch

        Time.timeScale = 0; //stop time

        TargetGroupSingleton.Instance.RemovePlayersTarget(); //Remove mouse influence to TargetGroup

        pauseCanvas.enabled = true; //show UIs

        OnPauseMenu?.Invoke(); //event

        controllerControls.RestartSelection(); //have the first button selected when opening menu
        controllerControls.isReadingInput = true;
    }
    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        pauseCanvas.enabled = false;
        OnUnpauseMenu?.Invoke();
        controllerControls.isReadingInput = false;
    }
    public void exit()
    {
        Unpause();
        Application.Quit();
    }
    public void die()
    {
        Unpause();
        Player_StateMachine playerStateMachine = GlobalPlayerReferences.Instance.playerTf.GetComponent<Player_StateMachine>();
        PlayerState deathState = playerStateMachine.GetComponent<Player_References>().DeadState;
        playerStateMachine.ForceChangeState(deathState);
    }
    public void restartLevel()
    {
        Unpause();
        SceneManager.LoadScene("GutWhale");
    }

}
