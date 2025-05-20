using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour, IRoom
{
    List<MaskableGraphic> images = new List<MaskableGraphic>();
    GameObject EndScreenRootImage;
    bool isDisplaying;

    public void OnRoomLoaded()
    {
        InputDetector.Instance.OnPausePressed += onPausePressed;
       // images = GetComponentsInChildren<MaskableGraphic>().ToList();
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);
    }

    public void OnRoomUnloaded()
    {
        InputDetector.Instance.OnPausePressed -= onPausePressed;
    }
    void onPausePressed()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    
    /*
    void FadeOut(float time)
    {
        foreach (MaskableGraphic i in images) 
        {
            StartCoroutine(FadeOutUiElement(i, time));
        }
        isDisplaying = false;
        EndScreenRootImage.SetActive(false);
    }
    void FadeIn(float time)
    {
        foreach (MaskableGraphic i in images)
        {
            StartCoroutine(FadeInUIElement(i, time));
        }
        isDisplaying = true;
        
    }
    IEnumerator FadeOutUiElement(MaskableGraphic im, float duration)
    {
        float timer = 0;
        float inverseNormalizedTime = 0;
        Color baseColor = im.color; //get the original color so we dont mess up
        im.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            inverseNormalizedTime = timer/duration;
            im.color = new Color(baseColor.r, baseColor.g, baseColor.b, inverseNormalizedTime);
            yield return null;
        }
        im.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
    }
    IEnumerator FadeInUIElement(MaskableGraphic im, float duration)
    {
        float timer = 0;
        float NormalizedTime = 0;
        Color baseColor = im.color;
        im.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
        while (timer < duration)
        {
            timer += Time.deltaTime;
            NormalizedTime = timer / duration;
            im.color = new Color(baseColor.r, baseColor.g, baseColor.b, NormalizedTime);
            yield return null;
        }
        im.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1);
    }
    */

}
