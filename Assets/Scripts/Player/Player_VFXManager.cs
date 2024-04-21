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
        eventSystem.CallHideAndDisable += InstantiateBloodExplosion;
    }
    private void OnDisable()
    {
        eventSystem.OnSuccessfulParry -= InstantiateParryVFX;
        eventSystem.OnDealtDamage -= InstantiateDealDamageVFX;
        eventSystem.OnReceiveDamage -= InstantiateReceiveDamageVFX;
        eventSystem.OnPerformRoll -= PlayDustVFX;
        eventSystem.OnReceiveDamage -= PlayGroundBlood;
        eventSystem.CallHideAndDisable -= InstantiateBloodExplosion;
    }
    public void InstantiateParryVFX(object sender, SuccesfulParryInfo parryInfo)
    {
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitEnemyParry, parryInfo.ParryPosition);
    }
    void InstantiateDealDamageVFX(object sender, Player_EventSystem.DealtDamageInfo dealtDamageInfo)
    {
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitEnemy, dealtDamageInfo.CollisionPosition);
    }
    void InstantiateReceiveDamageVFX(object sender, Player_EventSystem.ReceivedAttackInfo receivedDamageInfo)
    {
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitPlayer, receivedDamageInfo.CollisionPosition);
    }
    void InstantiateBloodExplosion()
    {
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BloodExplosion, transform.position);
    }
    void PlayDustVFX()
    {
        VFX_Roll.Play();
    }
    void PlayGroundBlood(object sender, Generic_EventSystem.ReceivedAttackInfo args)
    {
        if (!args.IsBloody) { return; } //if its not bloody pa casa

        Vector2 thisPosition = transform.position;
        Vector2 otherPosition = args.Attacker.transform.root.position;
        Vector2 opositeDirection = (thisPosition - otherPosition).normalized;

        if (simpleVfxPlayer.Instance == null)
        {
            Debug.LogWarning("No Ground Blood instance");
            return;
        }
        GroundBloodPlayer.Instance.PlayGroundBlood(thisPosition, opositeDirection,0.9f);
    }
    public void EV_HideTrail() { WeaponTrail.emitting = false; }
    public void EV_ShowTrail() { WeaponTrail.emitting = true; }
    public void EV_ShowRollTrail() { rollTrail.enabled = true; }
    public void EV_HideRollTrail() { rollTrail.enabled = false; }
}
