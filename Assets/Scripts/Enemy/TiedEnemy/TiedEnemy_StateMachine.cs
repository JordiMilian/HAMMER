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
    private void OnEnable()
    {
        eventSystem.OnDeath += OnDeathState;
        dialoguerScript.onFinishedReading += killDude;
    }
    public override void OnDeathState(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        CurrentState = States.Dead;
        HeadSprite.enabled = false;
        vfxManager.groundBloodIntensity = 0.4f;
        //GameObject.Find(TagsCollection.MainCharacter).GetComponent<Player_HealthSystem>().RestoreAllHealth();
        GlobalPlayerReferences.Instance.references.healthSystem.RestoreAllHealth();
        dialoguer.SetActive(false);
        
    }
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
