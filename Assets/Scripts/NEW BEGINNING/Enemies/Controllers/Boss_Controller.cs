using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Controller : BasicEnemy_Controller
{
    [SerializeField] float PercentOfHealthToActivat = 50;
    public override void OnDamageReceived(ReceivedAttackInfo info)
    {
        base.OnDamageReceived(info);

        if (GetCurrentHealth() < (GetMaxHealth() * (PercentOfHealthToActivat / 100)))
        {
            ChangeStateByType(StateTags.BossPhaseTransition);
        }
    }
}
