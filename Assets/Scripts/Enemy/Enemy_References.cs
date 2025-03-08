using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_References : Generic_References
{
    [Header("Specific")]
    public Enemy_AttacksProviderV2 attackProvider;
    public Enemy_EventSystem enemyEvents;
    public Enemy_HealthSystem healthSystem;
    public Enemy_MoveAndRotateToTarget moveToTarget;
    public Enemy_StanceMeter stanceMeter;
    public Enemy_VFXManager VFXManager;
    public Enemy01 feedbackManager;
    public Collider2D damageCollider;
    public Enemy_ReusableStateMachine reusableStateMachine;
    public Transform lookingPivotTf;
    public EnemyStats baseEnemyStats;
    public Generic_OnTriggerEnterEvents playerInAgrooCollider;
    [HideInInspector] public EnemyStats currentEnemyStats;
    public Enemy_ShowHideAttackCollider showHideAttackCollider;
    public Transform ownSinglePointCollider;
    private void Awake()
    {
        currentEnemyStats = new EnemyStats();
        currentEnemyStats.CopyStats(baseEnemyStats);
    }
}

