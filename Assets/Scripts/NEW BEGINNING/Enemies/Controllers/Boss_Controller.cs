using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Controller : BasicEnemy_Controller
{
    [Header("Boss phase transition")]
    [SerializeField] float PercentOfHealthToChangePhase = 50;
    [SerializeField] EnemyState BossChangePhaseState;
    public Action OnPhaseChange;
    public EnemyState BossIntroAnimationState;
    public override void OnDamageReceived(ReceivedAttackInfo info)
    {
        base.OnDamageReceived(info);

        if (GetCurrentHealth() < (GetMaxHealth() * (PercentOfHealthToChangePhase / 100)))
        {
            EnemyState_Agroo agrooState = (EnemyState_Agroo)enemyRefs.AgrooState;
            agrooState.ForceNextAttack(BossChangePhaseState);
        }
    }
}
