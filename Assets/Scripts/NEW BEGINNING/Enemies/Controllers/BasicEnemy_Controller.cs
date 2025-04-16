using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy_Controller : MonoBehaviour, IDamageDealer, IDamageReceiver, IParryReceiver, IHealth, IStats, IKilleable, IAttackedWhileRecovery
{ 
    [SerializeField] protected Generic_StateMachine enemyStateMachine;
    [SerializeField] protected Enemy_References enemyRefs;

    [Header("Damaged feedback")]
    [SerializeField] AnimationCurve damagedMovementCurve;
    float damagedCurveAverage = -1;
   
    
    public virtual void Awake()
    {
        enemyStateMachine.ChangeState(enemyRefs.IdleState);
    }
    #region DAMAGE RECEIVED
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public virtual void OnDamageReceived(ReceivedAttackInfo info)
    {
        RemoveHealth(info.Damage);
        if(GetCurrentHealth() <= 0)
        {
            OnKilled(new DeadCharacterInfo(gameObject, info.AttackerRoot_Go, info.OtherDamageDealer));
            return;
        }

        #region Push Feedback
        if (damagedCurveAverage < 0)
        {
            damagedCurveAverage = UsefullMethods.GetAverageValueOfCurve(damagedMovementCurve, 10);
        }

        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
        enemyRefs.characterMover,
           info.KnockBack * (1-enemyRefs.currentEnemyStats.KnockBackResistance),
        0.25f,
           info.CollidersDirection,
           damagedMovementCurve,
           damagedCurveAverage));

        if (info.IsBloody)
        {
            GroundBloodPlayer.Instance.PlayGroundBlood(transform.position, info.CollidersDirection, 0.9f);
        }

        if (enemyRefs.groundDetector.currentGround == Generic_TypeOFGroundDetector.TypesOfGround.puddle)
        {
            simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BigPuddleStep, transform.position);
        }

        #endregion

        
        enemyRefs.flasher.CallDefaultFlasher();

        if(enemyRefs.stanceMeter.IsStanceBrokenAfterRemoval(info.Damage))
        {
            enemyStateMachine.ChangeState(enemyRefs.StanceBrokenState);
        }
        else { enemyRefs.animator.SetTrigger("Hit"); }

        if (isInRecovery)
        {
            OnAttackedWhileRecovery();
        }

        OnDamageReceived_event?.Invoke(info);
    }
    #endregion
    #region HP MANAGEMENT
    public void RemoveHealth(float health)
    {
        if (enemyRefs.currentEnemyStats.CurrentHp <= 0) { Debug.LogWarning("Damaged something that should be already Dead?"); return; }

        enemyRefs.currentEnemyStats.CurrentHp -= health;

        if (enemyRefs.currentEnemyStats.CurrentHp <= 0)
        {
            enemyRefs.currentEnemyStats.CurrentHp = 0;
        }
        else if (enemyRefs.currentEnemyStats.CurrentHp > enemyRefs.currentEnemyStats.MaxHp)
        {
            enemyRefs.currentEnemyStats.CurrentHp = enemyRefs.currentEnemyStats.MaxHp;
        }
    }


    public void RestoreAllHealth()
    {
        enemyRefs.currentEnemyStats.CurrentHp = enemyRefs.currentEnemyStats.MaxHp;
    }
    public float GetCurrentHealth()
    {
        return enemyRefs.currentEnemyStats.CurrentHp;
    }
    public float GetMaxHealth()
    {
        return enemyRefs.currentEnemyStats.MaxHp;
    }
    #endregion
    #region DAMAGE DEALT
    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }
    public virtual void OnDamageDealt(DealtDamageInfo info)
    {
        UsefullMethods.CameraShakeAndHitstopFromDamage(info.DamageDealt);
        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.HitPlayer, info.CollisionPosition);
        OnDamageDealt_event?.Invoke(info);
    }
    #endregion
    #region PARRY RECEIVED
    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }
    public virtual void OnParryReceived(GettingParriedInfo info)
    {
        enemyStateMachine.ChangeState(enemyRefs.ParriedState);
        OnParryReceived_event?.Invoke(info);
    }
    #endregion
    #region STATS
    public EntityStats GetCurrentStats()
    {
        return enemyRefs.currentEnemyStats;
    }
    public void SetCurrentStats(EntityStats stats)
    {
        EnemyStats newStats = new EnemyStats();
        newStats.CopyStats((EnemyStats)stats);
        enemyRefs.currentEnemyStats = newStats;
    }

    public EntityStats GetBaseStats()
    {
        return enemyRefs.baseEnemyStats ;
    }
    public void SetBaseStats(EntityStats stats)
    {
        enemyRefs.baseEnemyStats = (EnemyStats)stats;
    }




    public Action<DeadCharacterInfo> OnKilled_event { get; set; }
    

    public void OnKilled(DeadCharacterInfo info)
    {
        enemyRefs.stateMachine.ChangeState(enemyRefs.DeathState);
        OnKilled_event?.Invoke(info);
    }
    #endregion
    #region Attacked While Recovery
    [Header ("Attacked While in Recovery")]
    [SerializeField] bool canAttackWhileRecovery = false;
    [SerializeField] EnemyState State_AttackedWhileRecoveryTransitionState;
    [SerializeField] float seconds_AttackedWhileRecoveryCooldown = 5;
    public bool isInRecovery { get; set; }
    bool isInCooldown;
    public void OnAttackedWhileRecovery()
    {
        if (!isInCooldown && canAttackWhileRecovery)
        {
            isInRecovery = false;
            enemyRefs.stateMachine.ChangeState(State_AttackedWhileRecoveryTransitionState);
            StartCoroutine(AttackedWhileRecovery_Cooldown());
        }
    }
    IEnumerator AttackedWhileRecovery_Cooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(seconds_AttackedWhileRecoveryCooldown);
        isInCooldown = false;
    }
    #endregion
    #region CHANGE STATE BY TYPE
    /*
    protected State GetStateByTag(StateTags tag)
    {
        foreach (StateReference stateRef in BasicStates)
        {
            if (stateRef.state.stateTag == tag)
            {
                return stateRef.state;
            }
        }
        Debug.LogError(tag + " state not found");  
        return null;
    }
    public void ChangeStateByType(StateTags tag)
    {
        enemyStateMachine.ChangeState(GetStateByTag(tag));
    }
    */
    #endregion

}

