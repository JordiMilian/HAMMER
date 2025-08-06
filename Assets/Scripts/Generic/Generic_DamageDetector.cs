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

   
    public bool enemy_canChargeSpecialAttack;
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
    private void OnEnable()
    {
        OnValidate();
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Generic_DamageDealer otherDealer = collision.GetComponent<Generic_DamageDealer>();
        if(otherDealer.damagedReceivers.Contains(this))
        {
            return; 
        }

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
                    if (otherDealer.EntityTeam == DamagersTeams.Player || otherDealer.EntityTeam == DamagersTeams.Neutral)
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
    */
    
    //Called from DamageDealer of course
    public void PublishAttackedEvent(Collider2D attackerCollider)
    {
        Generic_DamageDealer damageDealer = attackerCollider.gameObject.GetComponent<Generic_DamageDealer>();

        Debug.Log($"{rootGameObject.name} received attack from {attackerCollider.GetComponent<Generic_DamageDealer>().rootGameObject_DamageDealerTf.name}");

        thisIDamageReceiver.OnDamageReceived(new ReceivedAttackInfo
            (
            attackerCollider.ClosestPoint(gameObject.transform.position), //position
            (rootGameObject.transform.position - damageDealer.rootGameObject_DamageDealerTf.position).normalized, //roots direction
            (transform.position - attackerCollider.transform.position).normalized, //colliders direction
            rootGameObject.gameObject, //attacker Root
            damageDealer,
            damageDealer.Damage,
            damageDealer.Knockback,
            damageDealer.isBloody,
            damageDealer.Stagger
            ));
    }

}

