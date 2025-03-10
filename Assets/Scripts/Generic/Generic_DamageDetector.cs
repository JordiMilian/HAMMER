using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_DamageDetector : MonoBehaviour
{
    public enum Team
    {
        Player, Enemy, Object,
    }
    public Team EntityTeam;
    public EventHandler<EventArgs_ReceivedAttackInfo> OnReceiveDamage;
    public Generic_EventSystem eventSystem;
    bool isInCooldown;
    float CooldownTime = 0.2f;
    Coroutine cooldownRoutine;
    public bool canChargeSpecialAttack;
   
    public class EventArgs_ReceivedAttackInfo : EventArgs
    {
        public Vector3 CollisionPosition;
        public GameObject Attacker;
        public float Damage;
        public float KnockBack;
        public float Hitstop;
        public EventArgs_ReceivedAttackInfo(Vector2 collisionPosition, GameObject attacker, float damage, float knockBack, float hitstop)
        {
            CollisionPosition = collisionPosition;
            Attacker = attacker;
            Damage = damage;
            KnockBack = knockBack;
            Hitstop = hitstop;
        }
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInCooldown)
        {
            //Debug.Log("damage detector was in cooldown"); 
            return;
        }

        Generic_DamageDealer otherDealer = collision.GetComponent<Generic_DamageDealer>();

        if (otherDealer != null)
        {
            switch (EntityTeam)
            {
                case Team.Player:
                    if (otherDealer.EntityTeam == Generic_DamageDealer.Team.Enemy || otherDealer.EntityTeam == Generic_DamageDealer.Team.Object)
                    {
                        PublishAttackedEvent(collision);
                    }
                    break;
                case Team.Enemy:
                    if(otherDealer.EntityTeam == Generic_DamageDealer.Team.Player || otherDealer.EntityTeam == Generic_DamageDealer.Team.Object)
                    {
                        PublishAttackedEvent(collision);
                    }
                    break;
                case Team.Object:
                    PublishAttackedEvent(collision);
                    break;
            }
        }


        return;
        
    }
    void PublishAttackedEvent(Collider2D collision)
    {
        if (eventSystem.OnReceiveDamage != null)
        {
            Generic_DamageDealer damageDealer = collision.gameObject.GetComponent<Generic_DamageDealer>();

            eventSystem.OnReceiveDamage(this, new Generic_EventSystem.ReceivedAttackInfo
                (
                collision.ClosestPoint(gameObject.transform.position), //position
                (transform.position - collision.gameObject.transform.root.position).normalized, //general direction
                (transform.position - collision.gameObject.transform.position).normalized, //concrete direction
                collision.gameObject, //attacker
                damageDealer.Damage,
                damageDealer.Knockback,
                damageDealer.HitStop,
                damageDealer.isBloody
                )) ;
        }
        CooldownCall();
    }
    void PublishBeingTouched(Collider2D collider)
    {
        Vector2 direction = (transform.position - collider.transform.root.position).normalized;
        eventSystem.OnBeingTouchedObject?.Invoke(this, new Generic_EventSystem.ObjectDirectionArgs(direction));
        CooldownCall();
    }
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
}

