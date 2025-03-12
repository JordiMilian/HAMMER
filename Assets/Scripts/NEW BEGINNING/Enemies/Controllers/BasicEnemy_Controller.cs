using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy_Controller : MonoBehaviour, IDamageDealer, IDamageReceiver, IParryReceiver, IHealth, IStats
{ 
    [SerializeField] protected Generic_StateMachine enemyStateMachine;
    [SerializeField] protected Enemy_References enemyRefs;
   
    [Header("Damaged feedback")]
    [SerializeField] AnimationCurve damagedMovementCurve;
    float damagedCurveAverage = -1;
   
    
    public virtual void Awake()
    {
        SetCurrentStats(baseStats);
        SetBaseStats(baseStats);
        enemyStateMachine.ChangeState(enemyRefs.IdleState);

    }
    #region DAMAGE RECEIVED
    public virtual void OnDamageReceived(ReceivedAttackInfo info)
    {
        RemoveHealth(info.Damage);

        #region Push Feedback
        if (damagedCurveAverage < 0)
        {
            damagedCurveAverage = UsefullMethods.GetAverageValueOfCurve(damagedMovementCurve, 10);
        }

        StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(
        enemyRefs.characterMover,
           info.KnockBack,
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

        if(enemyRefs.stanceMeter.IsStanceBrokenAfterRemoval(info.Damage) && enemyStateMachine.currentState.stateTag != StateTags.Dead)
        {
            enemyStateMachine.ChangeState(enemyRefs.StanceBrokenState);
        }
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
            OnZeroHealth();
        }
        else if (enemyRefs.currentEnemyStats.CurrentHp > enemyRefs.currentEnemyStats.MaxHp)
        {
            enemyRefs.currentEnemyStats.CurrentHp = enemyRefs.currentEnemyStats.MaxHp;
        }
    }

    public void OnZeroHealth()
    {
        enemyStateMachine.ChangeState(enemyRefs.DeathState);
        Event_OnZeroHealth?.Invoke();
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
    public Action Event_OnZeroHealth { get; set; }
    #endregion
    #region DAMAGE DEALT
    public virtual void OnDamageDealt(DealtDamageInfo info)
    {

    }
    #endregion
    #region PARRY RECEIVED
    public virtual void OnParryReceived(GettingParriedInfo info)
    {
        enemyStateMachine.ChangeState(enemyRefs.ParriedState);
    }
    #endregion
    #region STATS
    [SerializeField] EnemyStats baseStats;
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
        return baseStats;
    }
    public void SetBaseStats(EntityStats stats)
    {
        enemyRefs.baseEnemyStats = (EnemyStats)stats;
    }

    #region FOCUS ICON
    public virtual void OnFocused()
    {
        enemyRefs.focusIcon.ShowFocusIcon();
    }

    public virtual void OnUnfocused()
    {
       enemyRefs.focusIcon.HideFocusIcon();
    }
    #endregion

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

