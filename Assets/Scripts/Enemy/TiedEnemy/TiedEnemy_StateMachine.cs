using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiedEnemy_StateMachine : Enemy_StateMachine
{
    [SerializeField] SpriteRenderer HeadSprite;
    [SerializeField] Enemy_VFXManager vfxManager;
    public override void DestroyOnDeath(object sender, Generic_EventSystem.Args_DeadCharacter args)
    {
        CurrentState = States.Dead;
        HeadSprite.enabled = false;
        vfxManager.groundBloodIntensity = 0.4f;

    }
}
