using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] GameObject LoadingScreenRoot;
    [SerializeField] TextMeshProUGUI loadingText; //Placeholder pls change at some point
    public void ShowLoadingScreen()
    {
        LoadingScreenRoot.SetActive(true);
    }
    public void HideLoadingScreen()
    {
        LoadingScreenRoot.SetActive(false);
    }
    public void UpdateLoadingBar(int percentage)
    {
        loadingText.text = percentage.ToString() + "%";
    }
}
