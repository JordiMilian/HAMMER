using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static Player_EventSystem;

public class Player_VFXManager : MonoBehaviour
{
    [Header("SCRIPTS")]
    [SerializeField] Player_EventSystem eventSystem;
    [Header("PREFABS")]
    [SerializeField] GameObject VFX_Parry;
    [SerializeField] GameObject VFX_HitEnemy;
    [SerializeField] GameObject VFX_HitObject;
    [SerializeField] GameObject VFX_ReceiveDamage;
    [SerializeField] VisualEffect VFX_Roll;
    [Header("TRAILS")]
    [SerializeField] TrailRenderer WeaponTrail;
    [SerializeField] TrailRenderer rollTrail;
    private void OnEnable()
    {
        eventSystem.OnSuccessfulParry += InstantiateParryVFX;
        eventSystem.OnDealtDamage += InstantiateDealDamageVFX;
        eventSystem.OnReceiveDamage += InstantiateReceiveDamageVFX;
        eventSystem.OnPerformRoll += PlayDustVFX;
    }
    private void OnDisable()
    {
        eventSystem.OnSuccessfulParry -= InstantiateParryVFX;
        eventSystem.OnDealtDamage -= InstantiateDealDamageVFX;
        eventSystem.OnReceiveDamage -= InstantiateReceiveDamageVFX;
        eventSystem.OnPerformRoll -= PlayDustVFX;
    }
    public void InstantiateParryVFX(object sender, EventArgs_ParryInfo parryInfo)
    {
        Instantiate(VFX_Parry, parryInfo.vector3data, Quaternion.identity);
    }
    void InstantiateDealDamageVFX(object sender, Player_EventSystem.EventArgs_DealtDamageInfo dealtDamageInfo)
    {
        GameObject HitEnemy = Instantiate(VFX_HitEnemy,dealtDamageInfo.CollisionPosition, Quaternion.identity);
    }
    void InstantiateReceiveDamageVFX(object sender, Player_EventSystem.EventArgs_ReceivedAttackInfo receivedDamageInfo)
    {
        GameObject ReceiveDamage = Instantiate(VFX_ReceiveDamage, receivedDamageInfo.CollisionPosition, Quaternion.identity);
    }
    void PlayDustVFX(object sender, EventArgs args)
    {
        VFX_Roll.Play();
    }
    public void EV_HideTrail() { WeaponTrail.enabled = false; }
    public void EV_ShowTrail() { WeaponTrail.enabled = true; }
    public void EV_ShowRollTrail() { rollTrail.enabled = true; }
    public void EV_HideRollTrail() { rollTrail.enabled = false; }
}
