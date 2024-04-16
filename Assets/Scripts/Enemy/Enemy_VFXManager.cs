using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy_VFXManager : MonoBehaviour
{
    [SerializeField] GameObject StanceBrokenVFX;
    [SerializeField] GameObject SucesfullParryVFX;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] GameObject BloodExplosion;
    [SerializeField] bool notSpawnBlood;
    [SerializeField] Enemy_EventSystem eventSystem;
 

    public float groundBloodIntensity = 0.9f;
    
    private void OnEnable()
    {
        eventSystem.OnStanceBroken += InstantiateStanceBrokenVFX;
        eventSystem.OnSuccessfulParry += InstantiateSuccesfulParryVFX;
        eventSystem.OnReceiveDamage += PlayGroundBlood;
        eventSystem.OnDeath += InstantiateBllodExplosion;
    }
    private void OnDisable()
    {
        eventSystem.OnStanceBroken -= InstantiateStanceBrokenVFX;
        eventSystem.OnSuccessfulParry -= InstantiateSuccesfulParryVFX;
        eventSystem.OnReceiveDamage -= PlayGroundBlood;
        eventSystem.OnDeath -= InstantiateBllodExplosion;

    }
    void InstantiateBllodExplosion(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BloodExplosion, transform.position);
    }
    void InstantiateStanceBrokenVFX()
    {
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.StanceBroken, transform.position);
    }
    void InstantiateSuccesfulParryVFX(object sender, Generic_EventSystem.SuccesfulParryInfo args)
    {
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.StanceBroken, transform.position);
    }
    public void EV_ShowTrail()
    {
        trailRenderer.enabled = true;
    }
    public void EV_HideTrail()
    {
        trailRenderer.enabled= false;
    }
    void PlayGroundBlood(object sender, Generic_EventSystem.ReceivedAttackInfo args)
    {
        if(notSpawnBlood) { return; }
        if (!args.IsBloody) { return; }

        Vector2 thisPosition = transform.position;
        Vector2 otherPosition = args.Attacker.transform.root.position;
        Vector2 opositeDirection = (thisPosition - otherPosition).normalized;

        if(simpleVfxPlayer.Instance ==  null)
        {
            Debug.LogWarning("No Ground Blood instance");
            return;
        }
        GroundBloodPlayer.Instance.PlayGroundBlood(thisPosition, args.GeneralDirection,groundBloodIntensity);
    }
  
}
