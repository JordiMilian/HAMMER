using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UI_Button : MonoBehaviour
{
    [SerializeField] VisualEffect bloodEffect;
    [SerializeField] UI_BaseAction Action;
    public void EV_playEffect()
    {
        bloodEffect.Play();
    }
    public void EV_PressedAction()
    {
        Action.Action(this);
    }

}
