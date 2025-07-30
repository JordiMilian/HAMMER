using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TutorialPopUp_Script : MonoBehaviour
{
    [SerializeField] Transform PopUpRoot;
    [SerializeField] Transform BG_Root;
    [SerializeField] TextMeshProUGUI TMP_PressA;
    public bool hasShown;
    public Action OnShownPopUp, OnHiddenPopUp;
    private void Start()
    {
        HideOnStart();
    }
    public void ShowPopUp()
    {
        if(hasShown) { return; }

        TMP_PressA.text = $"Press {InputDetector.Instance.Select_String()} to continue";

        PopUpRoot.gameObject.SetActive(true);
        BG_Root.gameObject.SetActive(true);

        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);

        SubscribeToPress();

        hasShown = true;

        OnShownPopUp?.Invoke();
    }
    void HidePopUp()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;

        playerStateMachine.ForceChangeState(playerRefs.IdleState);

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
