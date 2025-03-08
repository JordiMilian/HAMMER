using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiedEnemy_StateMachine : Generic_StateMachine
{
    [SerializeField] SpriteRenderer HeadSprite;
    [SerializeField] List<SpriteRenderer> BodySprites;
    [SerializeField] Enemy_VFXManager vfxManager;
    [SerializeField] GameObject dialoguer;
    [SerializeField] Dialoguer dialoguerScript;
    [SerializeField] Generic_EventSystem tiedEvents;
 
    void killDude(int i)
    {
        tiedEvents.OnReceiveDamage?.Invoke(this, new Generic_EventSystem.ReceivedAttackInfo(
            Vector2.zero,
            Vector2.zero,
            Vector2.zero,
            gameObject,
            50,
            0, 0, false
            ));
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
