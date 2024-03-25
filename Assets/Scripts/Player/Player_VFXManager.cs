using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static Generic_EventSystem;

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
        eventSystem.OnReceiveDamage += PlayGroundBlood;
    }
    private void OnDisable()
    {
        eventSystem.OnSuccessfulParry -= InstantiateParryVFX;
        eventSystem.OnDealtDamage -= InstantiateDealDamageVFX;
        eventSystem.OnReceiveDamage -= InstantiateReceiveDamageVFX;
        eventSystem.OnPerformRoll -= PlayDustVFX;
        eventSystem.OnReceiveDamage -= PlayGroundBlood;
    }
    public void InstantiateParryVFX(object sender, SuccesfulParryInfo parryInfo)
    {
        Instantiate(VFX_Parry, parryInfo.ParryPosition, Quaternion.identity);
    }
    void InstantiateDealDamageVFX(object sender, Player_EventSystem.DealtDamageInfo dealtDamageInfo)
    {
        GameObject HitEnemy = Instantiate(VFX_HitEnemy,dealtDamageInfo.CollisionPosition, Quaternion.identity);
    }
    void InstantiateReceiveDamageVFX(object sender, Player_EventSystem.ReceivedAttackInfo receivedDamageInfo)
    {
        GameObject ReceiveDamage = Instantiate(VFX_ReceiveDamage, receivedDamageInfo.CollisionPosition, Quaternion.identity);
    }
    void PlayDustVFX()
    {
        VFX_Roll.Play();
    }
    void PlayGroundBlood(object sender, Generic_EventSystem.ReceivedAttackInfo args)
    {
        Vector2 thisPosition = transform.position;
        Vector2 otherPosition = args.Attacker.transform.root.position;
        Vector2 opositeDirection = (thisPosition - otherPosition).normalized;

        if (GroundBloodMaker.Instance == null)
        {
            Debug.LogWarning("No Ground Blood instance");
            return;
        }
        GroundBloodMaker.Instance.Play(thisPosition, opositeDirection,0.9f);
    }
    public void EV_HideTrail() { WeaponTrail.enabled = false; }
    public void EV_ShowTrail() { WeaponTrail.enabled = true; }
    public void EV_ShowRollTrail() { rollTrail.enabled = true; }
    public void EV_HideRollTrail() { rollTrail.enabled = false; }
}
