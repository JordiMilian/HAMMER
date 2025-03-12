using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Generic_EventSystem;

public class Player_Controller : MonoBehaviour, IDamageReceiver, IDamageDealer, IHealth, IStats, IParryDealer, IParryReceiver
{
    Player_References playerRefs;

    #region HEALTH MANAGEMENT
    public float GetCurrentHealth()
    {
        return playerRefs.currentStats.CurrentHp;
    }
    public float GetMaxHealth()
    {
        return playerRefs.currentStats.MaxHp;
    }
    public void OnZeroHealth()
    {
        playerRefs.stateMachine.ForceChangeState(playerRefs.DeadState);
        Event_OnZeroHealth?.Invoke();
    }
    public void RemoveHealth(float health)
    {
        if (playerRefs.currentStats.CurrentHp <= 0) { Debug.LogWarning("Player is dead! Who damged it?"); return; }

        playerRefs.currentStats.CurrentHp -= health;

        if (playerRefs.currentStats.CurrentHp <= 0)
        {
            OnZeroHealth();
        }
        else if (playerRefs.currentStats.CurrentHp > playerRefs.currentStats.MaxHp)
        {
            playerRefs.currentStats.CurrentHp = playerRefs.currentStats.MaxHp;
        }
    }
    public void RestoreAllHealth()
    {
        playerRefs.currentStats.CurrentHp = playerRefs.currentStats.MaxHp;
        //Update UI??
    }
    public Action Event_OnZeroHealth { get; set; }
    #endregion
    #region DAMAGE DEALT
    public void OnDamageDealt(DealtDamageInfo info)
    {
       playerRefs.specialAttack.addCharge(info.ChargeGiven);

        // This should not depend on damage. Lets make enums of shake amounts pls
        CameraShake.Instance.ShakeCamera(1 * playerRefs.DamageDealersList[0].Damage, 0.1f * playerRefs.DamageDealersList[0].Damage);
        TimeScaleEditor.Instance.HitStop(0.1f);
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitEnemy, info.CollisionPosition);

        //Focus enemy on attacked
        FocusIcon maybeIcon = info.AttackedRoot.GetComponentInChildren<FocusIcon>();
        if (maybeIcon != null)
        {
            if(info.AttackedRoot.GetComponent<IHealth>().GetCurrentHealth() > 0)
            {
                playerRefs.followMouse.FocusNewEnemy(maybeIcon);
            }
        }
    }
    #endregion
    #region DAMAGE RECEIVED
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        RemoveHealth(info.Damage);

        playerRefs.playerStamina.RemoveStamina(0.1f); //make it depend on damage

        playerRefs.flasher.CallDefaultFlasher();

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
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitPlayer, info.CollisionPosition);

        // STANCE?

    }
    #endregion
    #region STATS
    public EntityStats GetBaseStats()
    {
        throw new System.NotImplementedException();
    }
    public EntityStats GetCurrentStats()
    {
        throw new System.NotImplementedException();
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
    public void OnParryDealt(SuccesfulParryInfo info)
    {
        if (info.canChargeSpecialAttack) { playerRefs.specialAttack.addCharge(1.25f); }

        TimeScaleEditor.Instance.HitStop(0.3f);
        CameraShake.Instance.ShakeCamera(0.6f, 0.1f);
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitEnemyParry, info.ParryPosition);
    }

    public void OnParryReceived(GettingParriedInfo info)
    {
        playerRefs.stateMachine.ForceChangeState(playerRefs.ParriedState);
    }
}
