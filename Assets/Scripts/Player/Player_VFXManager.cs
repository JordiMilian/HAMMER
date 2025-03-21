using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player_VFXManager : MonoBehaviour
{
    [Header("SCRIPTS")]
    [SerializeField] Player_EventSystem eventSystem;
    [Header("PREFABS")]
    [SerializeField] VisualEffect VFX_Roll;
    [Header("TRAILS")]
    [SerializeField] TrailRenderer WeaponTrail;
    [SerializeField] TrailRenderer rollTrail;
    [Header("Puddle")]
    [SerializeField] Generic_TypeOFGroundDetector groundDetector;
    /*
    private void OnEnable()
    {
        eventSystem.OnSuccessfulParry += InstantiateParryVFX;
        eventSystem.OnDealtDamage += InstantiateDealDamageVFX;
        eventSystem.OnReceiveDamage += InstantiateReceiveDamageVFX;
        eventSystem.OnPerformRoll += PlayDustVFX;
        eventSystem.OnReceiveDamage += PlayGroundBlood;
        eventSystem.CallShowAndEnable += InstantiateBloodExplosion;

        eventSystem.OnReceiveDamage += (object sender, Generic_EventSystem.ReceivedAttackInfo info) => PlayBigPuddleStep();
        eventSystem.OnPerformRoll += PlayBigPuddleStep;
        eventSystem.OnDeath += (object sender, Generic_EventSystem.DeadCharacterInfo info) => PlayBigPuddleStep();
    }
    private void OnDisable()
    {
        eventSystem.OnSuccessfulParry -= InstantiateParryVFX;
        eventSystem.OnDealtDamage -= InstantiateDealDamageVFX;
        eventSystem.OnReceiveDamage -= InstantiateReceiveDamageVFX;
        eventSystem.OnPerformRoll -= PlayDustVFX;
        eventSystem.OnReceiveDamage -= PlayGroundBlood;
        eventSystem.CallShowAndEnable -= InstantiateBloodExplosion;
        eventSystem.OnAttackStarted -= PlayBigPuddleStep;
        eventSystem.OnPerformRoll -= PlayBigPuddleStep;
    }
    */


    void InstantiateBloodExplosion()
    {
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BloodExplosion, transform.position);
    }
    void PlayBigPuddleStep()
    {
        if(groundDetector.currentGround == Generic_TypeOFGroundDetector.TypesOfGround.puddle)
        {
            simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BigPuddleStep, transform.position);
        }
    }

    public void EV_HideTrail() { WeaponTrail.emitting = false; }
    public void EV_ShowTrail() { WeaponTrail.emitting = true; }
    public void EV_ShowRollTrail() { rollTrail.enabled = true; }
    public void EV_HideRollTrail() { rollTrail.enabled = false; }
}
