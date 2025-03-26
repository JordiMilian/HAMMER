using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_DamageDetector : MonoBehaviour
{

    public DamagersTeams EntityTeam;
    //public Generic_EventSystem eventSystem;
    IDamageReceiver thisIDamageReceiver;
    public Transform rootGameObject;

   
    public bool canChargeSpecialAttack;
    private void OnValidate()
    {
        if (rootGameObject != null)
        {
            IDamageReceiver tempDamageReceiver = rootGameObject.GetComponent<IDamageReceiver>();

            if (tempDamageReceiver == null)
            {
                rootGameObject = null;
                Debug.LogWarning("Root gameobject doesn't implement IDamageReceiver");
                return;
            }
            thisIDamageReceiver = tempDamageReceiver;
        }
      
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Generic_DamageDealer otherDealer = collision.GetComponent<Generic_DamageDealer>();

        if (otherDealer != null)
        {
            switch (EntityTeam)
            {
                case DamagersTeams.Player:
                    if (otherDealer.EntityTeam == DamagersTeams.Enemy || otherDealer.EntityTeam == DamagersTeams.Neutral)
                    {
                        PublishAttackedEvent(collision);
                    }
                    break;
                case DamagersTeams.Enemy:
                    if(otherDealer.EntityTeam == DamagersTeams.Player || otherDealer.EntityTeam == DamagersTeams.Neutral)
                    {
                        PublishAttackedEvent(collision);
                    }
                    break;
                case DamagersTeams.Neutral:
                    PublishAttackedEvent(collision);
                    break;
            }
        }
        return;
    }
    void PublishAttackedEvent(Collider2D attackerCollider)
    {
        Generic_DamageDealer damageDealer = attackerCollider.gameObject.GetComponent<Generic_DamageDealer>();

        thisIDamageReceiver.OnDamageReceived(new ReceivedAttackInfo
            (
            attackerCollider.ClosestPoint(gameObject.transform.position), //position
            (rootGameObject.transform.position - damageDealer.rootGameObject_DamageDealerTf.position).normalized, //roots direction
            (transform.position - attackerCollider.transform.position).normalized, //colliders direction
            attackerCollider.gameObject, //attacker Root
            damageDealer,
            damageDealer.Damage,
            damageDealer.Knockback,
            damageDealer.isBloody
            ));
        //CooldownCall();
    }
    //What is this????
    void PublishBeingTouched(Collider2D collider)
    {
        //Vector2 direction = (transform.position - collider.transform.root.position).normalized;
        //eventSystem.OnBeingTouchedObject?.Invoke(this, new Generic_EventSystem.ObjectDirectionArgs(direction));
        //CooldownCall();
    }

    //LETS TRY WITHOUT COOLDOWN
    /* 
    bool isInCooldown;
    float CooldownTime = 0.2f;
    Coroutine cooldownRoutine;
    void CooldownCall()
    {
        if(gameObject.activeInHierarchy == false) { return; }
        if (cooldownRoutine != null) { StopCoroutine(cooldownRoutine); }
        cooldownRoutine = StartCoroutine(Cooldown());
    }
    IEnumerator Cooldown()
    {
        isInCooldown = true;
        float timer = 0;
        while (timer < CooldownTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        isInCooldown = false;
    }
    */
}

