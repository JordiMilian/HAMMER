using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Death : EnemyState
{
    [SerializeField] XP_dropper xP_Dropper;
    [SerializeField] AudioClip SFX_DeathSound;
    [SerializeField] DeadPart_Instantiator deadPartInstantiator;

    public override void OnEnable()
    {
        base.OnEnable();

        simpleVfxPlayer.Instance.playSimpleVFX(simpleVfxPlayer.simpleVFXkeys.BloodExplosion, transform.position);
        EnemyRefs.moveToTarget.SetMovementSpeed(SpeedsEnum.Stopped);
        //unfocus itself? NO
        xP_Dropper.spawnExperiences(EnemyRefs.currentEnemyStats.XpToDrop);

        deadPartInstantiator.InstantiateDeadParts();
        //play sound
        TimeScaleEditor.Instance.SlowMotion(IntensitiesEnum.Medium); //Slow mo feels weird here
        //Hide sprites
        stateMachine.gameObject.SetActive(false);
        Destroy(rootGameObject,1);
    }
}
