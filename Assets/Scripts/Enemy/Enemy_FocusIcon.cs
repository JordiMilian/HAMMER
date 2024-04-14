using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FocusIcon : MonoBehaviour
{
    [SerializeField] SpriteRenderer FocusSprite;
    [SerializeField] Enemy_References enemyRefs;
    private void OnEnable()
    {
        enemyRefs.enemyEvents.OnFocused += OnFocus;
        enemyRefs.enemyEvents.OnUnfocused += OnUnfocus;
    }
    public void OnFocus()
    {
        if(FocusSprite == null) { Debug.LogWarning("No focus sprite found"); return; }

        FocusSprite.enabled = true;

    }
    public void OnUnfocus()
    {
        if (FocusSprite == null) { Debug.LogWarning("No focus sprite found"); return; }
        FocusSprite.enabled = false;
    }
}
