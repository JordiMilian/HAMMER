using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : Generic_CharacterHealthSystem
{
    /*
    [SerializeField] Enemy_References enemyRefs;
    private void Awake()
    {
        currentStats = enemyRefs.currentEnemyStats;
        baseStats = enemyRefs.baseEnemyStats;
    }
    public override void Death(GameObject killer)
    {
        if(enemyRefs.stateMachine.CurrentState == Enemy_StateMachine.States.Dead) { Debug.Log("already dead"); return; }

        enemyRefs.genericEvents.OnDeath?.Invoke(this, new Generic_EventSystem.DeadCharacterInfo(gameObject, killer));

        enemyRefs.animator.SetTrigger(Tags.death);
    }
    */
}
