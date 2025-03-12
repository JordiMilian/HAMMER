using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusIcon : MonoBehaviour
{
    [SerializeField] SpriteRenderer FocusSprite;
    public GameObject RootGameObject;
    public Action OnFocused;

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
