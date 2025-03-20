using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Death : EnemyState
{
    [SerializeField] XP_dropper xP_Dropper;

    public override void OnEnable()
    {
        base.OnEnable();

        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BloodExplosion, transform.position);
        EnemyRefs.moveToTarget.SetMovementSpeed(MovementSpeeds.Stopped);
        //unfocus itself?
        xP_Dropper.spawnExperiences(EnemyRefs.currentEnemyStats.XpToDrop);
        //Spawn Head
        //play sound
        //SlowMo?
        stateMachine.gameObject.SetActive(false);
        Destroy(rootGameObject,1);
    }
}
