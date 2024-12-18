using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TutorialPopUp_Script : MonoBehaviour
{
    [SerializeField] Transform PopUpRoot;
    [SerializeField] Transform BG_Root;
    Player_EventSystem playerEvents;
    public bool hasShown;
    public Action OnShownPopUp, OnHiddenPopUp;
    private void Start()
    {
        HideOnStart();
    }
    public void ShowPopUp()
    {
        if(hasShown) { return; }

        PopUpRoot.gameObject.SetActive(true);
        BG_Root.gameObject.SetActive(true);

        if (playerEvents == null) { playerEvents = GlobalPlayerReferences.Instance.references.events; }
        playerEvents.CallDisable?.Invoke();

        SubscribeToPress();

        hasShown = true;

        OnShownPopUp?.Invoke();
    }
    void HidePopUp()
    {
        if (playerEvents == null) { playerEvents = GlobalPlayerReferences.Instance.references.events; }
        playerEvents.CallEnable?.Invoke();

        PopUpRoot.gameObject.SetActive(false);
        BG_Root.gameObject.SetActive(false);

        UnsubscribeToPress();

        OnHiddenPopUp?.Invoke();
    }
    void HideOnStart()
    {
        PopUpRoot.gameObject.SetActive(false);
        BG_Root.gameObject.SetActive(false);
    }
    void SubscribeToPress()
    {
        InputDetector.Instance.OnSelectPressed += HidePopUp;
    }
    void UnsubscribeToPress()
    {
        InputDetector.Instance.OnSelectPressed -= HidePopUp;
    }

}
