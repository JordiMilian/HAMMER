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
    void InstantiateBllodExplosion(object sender, Generic_EventSystem.Args_DeadCharacter args)
    {
        Instantiate(BloodExplosion, transform.position, Quaternion.identity);
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
    void PlayGroundBlood(object sender, Generic_EventSystem.EventArgs_ReceivedAttackInfo args)
    {
        Vector2 thisPosition = transform.position;
        Vector2 otherPosition = args.Attacker.transform.root.position;
        Vector2 opositeDirection = (thisPosition - otherPosition).normalized;

        if(GroundBloodMaker.Instance ==  null)
        {
            Debug.LogWarning("No Ground Blood instance");
            return;
        }
        GroundBloodMaker.Instance.Play(thisPosition, opositeDirection,groundBloodIntensity);
    }
  
}
