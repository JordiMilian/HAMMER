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
        xP_Dropper.spawnExperiences(rootGameObject.GetComponent<Enemy_References>().currentEnemyStats.XpToDrop);
        //Spawn Head
        //SlowMo?
        stateMachine.gameObject.SetActive(false);
        Destroy(rootGameObject,1);
    }
}
