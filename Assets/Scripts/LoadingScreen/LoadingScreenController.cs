using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] GameObject LoadingScreenRoot;
    [SerializeField] Image BlackScreenImage;
    [SerializeField] TextMeshProUGUI loadingText; //Placeholder pls change at some point

    public void ShowLoadingScreen()
    {
        LoadingScreenRoot.SetActive(true);
    }
    public void HideLoadingScreen()
    {
        LoadingScreenRoot.SetActive(false);
    }
    [SerializeField] float fadeTime = 0.2f;
    public IEnumerator FadeOutScreen()
    {
        float timer = 0;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            BlackScreenImage.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer / fadeTime));
            yield return null;
        }
        LoadingScreenRoot.SetActive(false);
    }
    public IEnumerator FadeInScreen()
    {
        float timer = 0;

        //Set loading screen
        Color startColor = BlackScreenImage.color;
        LoadingScreenRoot.SetActive(true);
       
        while (timer < fadeTime) //fade in
        {
            timer += Time.deltaTime;
            BlackScreenImage.color = new Color(0, 0, 0, Mathf.Lerp(startColor.a, 1, timer / fadeTime));
            yield return null;
        }
        BlackScreenImage.color = new Color(0, 0, 0, 1);
    }
    public void UpdateLoadingBar(int percentage)
    {
        loadingText.text = percentage.ToString() + "%";
    }
}
