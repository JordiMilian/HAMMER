using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiedEnemy_StateMachine : Generic_StateMachine
{
    [SerializeField] SpriteRenderer HeadSprite;
    [SerializeField] List<SpriteRenderer> BodySprites;
    [SerializeField] Enemy_VFXManager vfxManager;
    [SerializeField] Dialoguer dialoguer;
    public override void OnDeathState(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        CurrentState = States.Dead;
        HeadSprite.enabled = false;
        vfxManager.groundBloodIntensity = 0.4f;
        dialoguer.enabled = false;
    }
    public void ShowBodies() //Go to Respawner Manager
    {
        foreach (SpriteRenderer sprite in BodySprites)
        {
            sprite.enabled = true;
        }
    }
    public void EV_HideBody()
    {
        foreach(SpriteRenderer sprite in BodySprites)
        {
            sprite.enabled = false;
        }
    }
}
