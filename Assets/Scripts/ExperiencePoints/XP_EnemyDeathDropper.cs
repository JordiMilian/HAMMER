using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_EnemyDeathDropper : XP_dropper
{
    [SerializeField] Enemy_EventSystem enemyEvents;
    private void OnEnable()
    {
        enemyEvents.OnDeath += OnEnemyDied;
    }
    private void OnDisable()
    {
        enemyEvents.OnDeath -= OnEnemyDied;
    }
    void OnEnemyDied(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        spawnExperiences();
    }
}
