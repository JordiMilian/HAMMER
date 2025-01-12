using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TiedEnemy_StateController : MonoBehaviour, IDeath, IDamageReceiver
{
    [SerializeField] Generic_References genericRefs;
    [SerializeField] Player_Respawner ownRespawner;
    [SerializeField] SpriteRenderer HeadSprite;
    [SerializeField] List<SpriteRenderer> BodySprites;
    [SerializeField] Enemy_VFXManager vfxManager;
    [SerializeField] GameObject dialoguer;
    [SerializeField] Dialoguer dialoguerScript;

    bool isDead;
    private void OnEnable()
    {
        genericRefs.genericEvents.OnDeath += OnDeath;
        dialoguerScript.onFinishedReading += KillRespawnerOnFinishedReading;
    }
    public void OnReceiveDamage(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        if (!isDead)
        {
            genericRefs.flasher.CallDefaultFlasher();
        }
        genericRefs.animator.SetTrigger(Tags.PushBack);

    }
    void KillRespawnerOnFinishedReading(int nose)
    {
        genericRefs.genericEvents.OnReceiveDamage?.Invoke(this, new Generic_EventSystem.ReceivedAttackInfo(
           Vector2.zero,
           Vector2.zero,
           Vector2.zero,
           gameObject,
           50,
           0, 0, false
           ));
    }
    public void OnDeath(object sender, Generic_EventSystem.DeadCharacterInfo info)
    {
        HeadSprite.enabled = false;
        vfxManager.groundBloodIntensity = 0.4f;
        //GameObject.Find(TagsCollection.MainCharacter).GetComponent<Player_HealthSystem>().RestoreAllHealth();
        GlobalPlayerReferences.Instance.references.healthSystem.RestoreAllHealth();
        dialoguer.SetActive(false);
        isDead = true;
        ownRespawner.ActivateRespawner();
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
        foreach (SpriteRenderer sprite in BodySprites)
        {
            sprite.enabled = false;
        }

    }
}
