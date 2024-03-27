using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiedEnemy_StateMachine : Enemy_StateMachine
{
    [SerializeField] SpriteRenderer HeadSprite;
    [SerializeField] List<SpriteRenderer> BodySprites;
    [SerializeField] Enemy_VFXManager vfxManager;
    public override void DestroyOnDeath(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        CurrentState = States.Dead;
        HeadSprite.enabled = false;
        vfxManager.groundBloodIntensity = 0.4f;
    }
}
