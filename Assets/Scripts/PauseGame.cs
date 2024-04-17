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

    public Action OnPause;
    public Action OnUnpause;

    [Header("Buttons references")]
    [SerializeField] Player_EventSystem playerEventSystem;

    private void Awake()
    {
        MouseTarget = GameObject.Find(TagsCollection.MouseCameraTarget).transform;
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
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
    }
    void Pause()
    {
        isPaused = true; //bool for switch

        Time.timeScale = 0; //stop time

        TargetGroupSingleton.Instance.RemoveTarget(MouseTarget); //Remove mouse influence to TargetGroup

        pauseCanvas.enabled = true; //show UIs

        OnPause?.Invoke(); //event
    }
    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
        TargetGroupSingleton.Instance.AddTarget(MouseTarget, 1f, 0);
        pauseCanvas.enabled = false;
        OnUnpause?.Invoke();
    }
    public void exit()
    {
        Unpause();
        Debug.LogError(" Main Menu doesn't exist yet");
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
