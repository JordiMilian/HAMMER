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

    public Action OnPauseMenu;
    public Action OnUnpauseMenu;
    public Action CallPauseGame;
    public Action CallUnpauseGame;

    [Header("Buttons references")]
    [SerializeField] Player_EventSystem playerEventSystem;

    private void Awake()
    {
        MouseTarget = GameObject.Find(TagsCollection.MouseCameraTarget).transform;
    }
    private void OnEnable()
    {
        InputDetector.Instance.OnPausePressed += onPausePresed;
    }
    private void OnDisable()
    {
        InputDetector.Instance.OnPausePressed -= onPausePresed;
    }
    private void Start()
    {
        Unpause();
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

        TargetGroupSingleton.Instance.RemoveTarget(MouseTarget); //Remove mouse influence to TargetGroup

        pauseCanvas.enabled = true; //show UIs

        OnPauseMenu?.Invoke(); //event
    }
    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
        TargetGroupSingleton.Instance.AddTarget(MouseTarget, 1f, 0);
        pauseCanvas.enabled = false;
        OnUnpauseMenu?.Invoke();
    }
    public void exit()
    {
        Unpause();
        Application.Quit();
    }
    public void die()
    {
        Unpause();
        playerEventSystem.OnDeath?.Invoke(this, new Generic_EventSystem.DeadCharacterInfo(playerEventSystem.gameObject, gameObject));
    }
    public void restartLevel()
    {
        Unpause();
        SceneManager.LoadScene("GutWhale");
    }

}
