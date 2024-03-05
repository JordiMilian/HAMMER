using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_VFXManager : MonoBehaviour
{
    [SerializeField] GameObject StanceBrokenVFX;
    [SerializeField] GameObject SucesfullParryVFX;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] Enemy_EventSystem eventSystem;
    
    private void OnEnable()
    {
        eventSystem.OnStanceBroken += InstantiateStanceBrokenVFX;
        eventSystem.OnSuccessfulParry += InstantiateSuccesfulParryVFX;
    }
    private void OnDisable()
    {
        eventSystem.OnStanceBroken -= InstantiateStanceBrokenVFX;
        eventSystem.OnSuccessfulParry -= InstantiateSuccesfulParryVFX;
    }
    void InstantiateStanceBrokenVFX()
    {
        Instantiate(StanceBrokenVFX,transform.position,Quaternion.identity);
    }
    void InstantiateSuccesfulParryVFX(object sender, Generic_EventSystem.EventArgs_SuccesfulParryInfo args)
    {
        Instantiate(SucesfullParryVFX, args.vector3data, Quaternion.identity);
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
