using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ScreenSaver : MonoBehaviour
{
    [SerializeField] InputDetector inputDetector;
    public List<Action> inputingActionsList = new List<Action>();
    [SerializeField] float WaitAfterInput = 5;
    [SerializeField] GameObject VideoUIObject;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] UI_ControllerControl MainMenuButtonsController;
    bool isPlaying;

    Coroutine currentWaitForVideo;
    private void Awake()
    {
        inputDetector.OnAttackPressed += onPressedAnyButton;
        inputDetector.OnSelectPressed += onPressedAnyButton;
        inputDetector.OnDownPressed += onPressedAnyButton;
        inputDetector.OnUpPressed += onPressedAnyButton;
        inputDetector.OnRightPressed += onPressedAnyButton;
        inputDetector.OnLeftPressed += onPressedAnyButton;

        for (int i = 0; i < inputingActionsList.Count; i++)
        {
            Debug.Log("Subscribed to some");
            inputingActionsList[i] += onPressedAnyButton;
        }
    }
    private void Start()
    {
        currentWaitForVideo = StartCoroutine(waitingAfterInput());
    }
    void onPressedAnyButton()
    {
        Debug.Log("pressed some");
        if (isPlaying) { stopVideo(); }
        
        if(currentWaitForVideo != null) { StopCoroutine(currentWaitForVideo); }
        currentWaitForVideo = StartCoroutine(waitingAfterInput());

        isPlaying = false;
    }
    IEnumerator waitingAfterInput()
    {
        Debug.Log("Lets wait for: " + WaitAfterInput);
        yield return new WaitForSecondsRealtime(WaitAfterInput);
        playVideo();
        isPlaying = true;
        
    }
    void playVideo()
    {
        Debug.Log("playing video");
        MainMenuButtonsController.isReadingInput = false;
        VideoUIObject.SetActive(true);
        videoPlayer.Play();
        StartCoroutine(waitForEndVideo());
    }
    void stopVideo()
    {
        MainMenuButtonsController.isReadingInput = true;
        VideoUIObject.SetActive(false);
        videoPlayer.Stop();
    }
    IEnumerator waitForEndVideo()
    {
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        onPressedAnyButton();
    }
}
