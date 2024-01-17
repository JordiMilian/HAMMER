using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFXManager : MonoBehaviour
{
    [SerializeField] GameObject StanceBrokenVFX;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] Enemy_StanceMeter stanceMeter;
    private void OnEnable()
    {
        stanceMeter.onStanceBroken += InstantiateStanceBrokenVFX;
    }
    void InstantiateStanceBrokenVFX(object sender, EventArgs args)
    {
        Instantiate(StanceBrokenVFX,transform.position,Quaternion.identity);
    }
    public void EV_ShowTrail()
    {
        trailRenderer.enabled = true;
    }
    public void EV_HideTrail()
    {
        trailRenderer.enabled= false;
    }
}
