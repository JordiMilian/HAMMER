using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Death : State
{
    [SerializeField] XP_dropper xP_Dropper;

    public override void OnEnable()
    {
        xP_Dropper.spawnExperiences(rootGameObject.GetComponent<Enemy_References>().currentEnemyStats.XpToDrop);
        //Spawn Head
        //SlowMo?
        Destroy(rootGameObject);
        
    }
}
