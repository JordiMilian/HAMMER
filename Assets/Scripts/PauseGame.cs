using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PauseGame : MonoBehaviour
{
    [HideInInspector] public bool isPaused;
    Transform MouseTarget;
    [SerializeField] Canvas pauseCanvas;

    public Action OnPause;
    public Action OnUnpause;

    [SerializeField] VisualEffect constantBloodEffect;
    [SerializeField] List<VisualEffect> explosionsEffects = new List<VisualEffect>();
    [SerializeField] Animator pentagonAnimator;
    private void Awake()
    {
        MouseTarget = GameObject.Find(TagsCollection.MouseCameraTarget).transform;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
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
        isPaused = true;

        Time.timeScale = 0; //stop

        TargetGroupSingleton.Instance.RemoveTarget(MouseTarget);
        //TargetGroupSingleton.Instance.AddTarget(MouseTarget, 0.25f, 0);

        pauseCanvas.enabled = true; //show UIs

        OnPause?.Invoke(); //event

        
        constantBloodEffect.Play(); //play constant blood
        constantBloodEffect.enabled = true;

        pentagonAnimator.SetTrigger("play");

        foreach (VisualEffect efect in explosionsEffects)
        {
            efect.enabled = true;
        }
    }
    void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1;
        //TargetGroupSingleton.Instance.RemoveTarget(MouseTarget);
        TargetGroupSingleton.Instance.AddTarget(MouseTarget, 1f, 0);
        pauseCanvas.enabled = false;
        OnUnpause?.Invoke();
        constantBloodEffect.Stop();
        constantBloodEffect.enabled = false;
        foreach(VisualEffect efect in explosionsEffects)
        {
            efect.enabled = false;
        }
    }
}
