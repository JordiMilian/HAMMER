using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FocusIcon : MonoBehaviour
{
    [SerializeField] SpriteRenderer FocusSprite;
    public void OnFocus()
    {
        FocusSprite.enabled = true;
    }
    public void OnUnfocus()
    {
        FocusSprite.enabled = false;
    }
}
