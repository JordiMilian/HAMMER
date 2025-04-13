using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focuseable : MonoBehaviour
{
    //A GAMEOBJECT WITH THIS SCRIPT MUST USE THE TAG FOCUSEABLEOBJECT IN ORDER TO BE FOUND BY THE PLAYER

    [SerializeField] SpriteRenderer FocusSprite;
    public GameObject RootGameObject;
    public Action OnFocused;
    private void OnEnable()
    {
        FocuseablesManager.Instance.FocusaeblesList.Add(this);
    }
    private void OnDisable()
    {
        FocuseablesManager.Instance.FocusaeblesList.Remove(this);
    }

    public void ShowFocusIcon()
    {
        if(FocusSprite == null) { Debug.LogWarning("No focus sprite found"); return; }

        FocusSprite.enabled = true;
        OnFocused?.Invoke();
    }
    public void HideFocusIcon()
    {
        if (FocusSprite == null) { Debug.LogWarning("No focus sprite found"); return; }
        FocusSprite.enabled = false;
    }
}
