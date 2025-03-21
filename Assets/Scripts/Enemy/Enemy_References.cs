using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_References : Generic_References
{
    [Header("Specific")]
    public Enemy_MoveAndRotateToTarget moveToTarget;
    public Enemy_StanceMeter stanceMeter;
    public Collider2D damageCollider;
    public Enemy_ReusableStateMachine reusableStateMachine;
    public Transform lookingPivotTf;
    public EnemyStats baseEnemyStats;
    public Generic_OnTriggerEnterEvents playerInAgrooCollider;
    [HideInInspector] public EnemyStats currentEnemyStats;
    public Enemy_ShowHideAttackCollider showHideAttackCollider;
    public Transform ownSinglePointCollider;
    public Generic_StateMachine stateMachine;
    public FocusIcon focusIcon;
    public Generic_TypeOFGroundDetector groundDetector;

    public EnemyState IdleState, AgrooState, ParriedState, StanceBrokenState, DeathState;
    private void Awake()
    {
        currentEnemyStats = new EnemyStats();
        currentEnemyStats.CopyStats(baseEnemyStats);
    }
}

