using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayUI_Effects : MonoBehaviour
{
    [SerializeField] VisualEffect bloodEffect;
    public void EV_playEffect()
    {
        bloodEffect.Play();
    }
}
