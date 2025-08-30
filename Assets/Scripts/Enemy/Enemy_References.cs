using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_References : Generic_References
{
    [Header("Specific")]
    public Enemy_MoveAndRotateToTarget moveToTarget;
    public Enemy_StanceMeter stanceMeter;
    public Collider2D damageCollider;
    public Transform lookingPivotTf;
    public EnemyStats baseEnemyStats;
    [HideInInspector] public EnemyStats currentEnemyStats;
    public Transform ownSinglePointCollider;
    public Focuseable Focuseable;
    public Generic_TypeOFGroundDetector groundDetector;
    public BasicEnemy_AnimationEvents basicAnimationEvents;

    [Header("STATES")]
    public Generic_StateMachine stateMachine;
    public EnemyState IdleState, AgrooState, ParriedState, StanceBrokenState, DeathState;
    public Transform AttackStatesParent;
    private void Awake()
    {
        currentEnemyStats = new EnemyStats();
        currentEnemyStats.CopyStats(baseEnemyStats);
    }
}

