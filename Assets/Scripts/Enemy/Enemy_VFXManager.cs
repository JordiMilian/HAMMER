using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFXManager : MonoBehaviour
{
    [SerializeField] TrailRenderer trailRenderer;
    public void EV_ShowTrail()
    {
        trailRenderer.enabled = true;
    }
    public void EV_HideTrail()
    {
        trailRenderer.enabled= false;
    }
}
