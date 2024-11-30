using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_EnemyDeathDropper : XP_dropper
{
    [SerializeField] Enemy_References enemyRefs;
    private void OnEnable()
    {
        enemyRefs.enemyEvents.OnDeath += OnEnemyDied;
    }
    private void OnDisable()
    {
        enemyRefs.enemyEvents.OnDeath -= OnEnemyDied;
    }
    void OnEnemyDied(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        spawnExperiences(enemyRefs.currentEnemyStats.XpToDrop);
    }
}
