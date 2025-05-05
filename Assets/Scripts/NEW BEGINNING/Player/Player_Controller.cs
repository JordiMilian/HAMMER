using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour, IDamageReceiver, IDamageDealer, IHealth, IStats, IParryDealer, IParryReceiver, IKilleable
{
   [SerializeField] protected Player_References playerRefs;
    [SerializeField] AudioClip SFX_DealtDamage, SFX_ReceiveDamage, SFX_ParryDealt;
    private void Start()
    {
        RestoreAllHealth();
    }

    #region HEALTH MANAGEMENT
    public float GetCurrentHealth()
    {
        return playerRefs.currentStats.CurrentHp;
    }
    public float GetMaxHealth()
    {
        return playerRefs.currentStats.MaxHp;
    }

    public void RemoveHealth(float health)
    {
        if (playerRefs.currentStats.CurrentHp <= 0) { Debug.LogWarning("Player is dead! Who damged it?"); return; }

        playerRefs.currentStats.CurrentHp -= health;

        if (playerRefs.currentStats.CurrentHp > playerRefs.currentStats.MaxHp)
        {
            playerRefs.currentStats.CurrentHp = playerRefs.currentStats.MaxHp;
        }
    }
    public void RestoreAllHealth()
    {
        playerRefs.currentStats.CurrentHp = playerRefs.currentStats.MaxHp;
        //Update UI??
    }

    #endregion
    #region DAMAGE DEALT
    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }
    public void OnDamageDealt(DealtDamageInfo info)
    {
        
       addSpecialCharge(info.ChargeGiven);

        UsefullMethods.CameraShakeAndHitstopFromDamage(info.DamageDealt);
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitEnemy, info.CollisionPosition);
        SFX_PlayerSingleton.Instance.playSFX(SFX_DealtDamage, 0.1f);

        //Focus enemy on attacked
        playerRefs.swordRotation.AttemptFocusAttackedEnemy(info);

        OnDamageDealt_event?.Invoke(info);
        
    }
    #endregion
    #region DAMAGE RECEIVED
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }

    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        RemoveHealth(info.Damage);
        if(GetCurrentHealth() <= 0)
        {
            OnKilled(new DeadCharacterInfo(gameObject, info.AttackerRoot_Go, info.OtherDamageDealer));
            OnDamageReceived_event?.Invoke(info);
            return;
        }

        playerRefs.playerStamina.RemoveStamina(0.1f * info.Damage); //make it depend on damage

        playerRefs.flasher.CallDefaultFlasher();
        UsefullMethods.CameraShakeAndHitstopFromDamage(info.Damage);

        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
                playerRefs.characterMover,
                info.KnockBack,
                0.2f,
                info.CollidersDirection,
                AnimationCurve.Linear(0, 1, 1, 0)
                ));

        if(info.IsBloody)
        {
            GroundBloodPlayer.Instance.PlayGroundBlood(transform.position, info.CollidersDirection, 0.9f);
        }

        SFX_PlayerSingleton.Instance.playSFX(SFX_ReceiveDamage, .1f);

        OnDamageReceived_event?.Invoke(info);

        // STANCE?

    }
    #endregion
    #region STATS
    [Header("Stats")]
    [SerializeField] PlayerStats currentStats;
    [SerializeField] PlayerStats baseStats;
    public EntityStats GetBaseStats()
    {
        return playerRefs.baseStats;
    }
    public EntityStats GetCurrentStats()
    {
        return playerRefs.currentStats;
    }
    public void SetBaseStats(EntityStats stats)
    {
        throw new System.NotImplementedException();
    }
    public void SetCurrentStats(EntityStats stats)
    {
        throw new System.NotImplementedException();
    }
    #endregion
    #region PARRY
    public Action<SuccesfulParryInfo> OnParryDealt_event { get; set; }
    public void OnParryDealt(SuccesfulParryInfo info)
    {
        if (info.canChargeSpecialAttack) { addSpecialCharge(1.25f); }

        TimeScaleEditor.Instance.HitStop(IntensitiesEnum.Big);
        TimeScaleEditor.Instance.SlowMotion(IntensitiesEnum.VerySmall);
        CameraShake.Instance.ShakeCamera(IntensitiesEnum.Medium);
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitEnemyParry, info.ParryPosition);
        SFX_PlayerSingleton.Instance.playSFX(SFX_ParryDealt);

        OnParryDealt_event?.Invoke(info);
    }
    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }
    public void OnParryReceived(GettingParriedInfo info)
    {
        playerRefs.stateMachine.ForceChangeState(playerRefs.ParriedState);
        OnParryReceived_event?.Invoke(info);
    }
    #endregion
    #region KILLEABLE
    public Action<DeadCharacterInfo> OnKilled_event { get; set; }
    public void OnKilled(DeadCharacterInfo info)
    {
        playerRefs.stateMachine.ForceChangeState(playerRefs.DeadState);
        OnKilled_event?.Invoke(info);
    }
    #endregion
    #region SPECIAL CHARGE
     bool isChargeFilled()
    {
        return playerRefs.currentStats.CurrentBloodFlow! < playerRefs.currentStats.MaxBloodFlow;
    }
     void addSpecialCharge(float amount)
    {
        float temporalValue = playerRefs.currentStats.CurrentBloodFlow + (amount * playerRefs.currentStats.BloodflowMultiplier);

        if (temporalValue > playerRefs.currentStats.MaxBloodFlow) { temporalValue = playerRefs.currentStats.MaxBloodFlow; }
        else if (temporalValue < 0) { temporalValue = 0; }

        playerRefs.currentStats.CurrentBloodFlow = temporalValue;
    }
     void restartCharge()
    {
        playerRefs.currentStats.CurrentBloodFlow = 0;
    }
    #endregion
}
