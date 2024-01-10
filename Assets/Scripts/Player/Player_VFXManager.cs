using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static Generic_DamageDealer;
using static Generic_DamageDetector;
using static Player_ParryPerformer;

public class Player_VFXManager : MonoBehaviour
{
    [Header("SCRIPTS")]
    [SerializeField] Player_ParryPerformer SuccesfullParryDetector;
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Generic_DamageDetector damageDetector;
    [SerializeField] Player_Movement playerMovement;
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
        SuccesfullParryDetector.OnSuccessfulParry += InstantiateParryVFX;
        damageDealer.OnDealtDamage += InstantiateDealDamageVFX;
        damageDetector.OnReceiveDamage += InstantiateReceiveDamageVFX;
        playerMovement.OnPerformRoll += PlayDustVFX;
    }
    private void OnDisable()
    {
        SuccesfullParryDetector.OnSuccessfulParry -= InstantiateParryVFX;
        damageDealer.OnDealtDamage -= InstantiateDealDamageVFX;
        damageDetector.OnReceiveDamage -= InstantiateReceiveDamageVFX;
        playerMovement.OnPerformRoll -= PlayDustVFX;
    }
    public void InstantiateParryVFX(object sender, EventArgs_ParryInfo parryInfo)
    {
        Instantiate(VFX_Parry, parryInfo.vector3data, Quaternion.identity);
    }
    void InstantiateDealDamageVFX(object sender, EventArgs_DealtDamageInfo dealtDamageInfo)
    {
        GameObject HitEnemy = Instantiate(VFX_HitEnemy,dealtDamageInfo.CollisionPosition, Quaternion.identity);
    }
    void InstantiateReceiveDamageVFX(object sender, EventArgs_ReceivedAttackInfo receivedDamageInfo)
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
