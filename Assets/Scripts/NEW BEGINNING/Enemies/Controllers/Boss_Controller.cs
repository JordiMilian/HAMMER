using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Controller : BasicEnemy_Controller
{
    [SerializeField] float PercentOfHealthToChangePhase = 50;
    [SerializeField] EnemyState BossChangePhaseState;
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
