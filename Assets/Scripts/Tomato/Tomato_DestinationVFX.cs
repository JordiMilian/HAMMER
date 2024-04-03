using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Tomato_DestinationVFX : MonoBehaviour
{
    [SerializeField] VisualEffect TomatoSplash;
    public void EV_PlaySplash()
    {
        TomatoSplash.Play();
    }
}
