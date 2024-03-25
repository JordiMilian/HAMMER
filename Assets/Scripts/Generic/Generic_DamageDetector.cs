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
    [SerializeField] Generic_EventSystem eventSystem;
   
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
        switch (EntityTeam)
        {
            case Team.Object:
                if(collision.CompareTag(TagsCollection.Enemy_Hitbox) || collision.CompareTag(TagsCollection.Player_Hitbox))
                {
                    PublishAttackedEvent(collision);
                }
                if(collision.CompareTag(TagsCollection.Player) || collision.CompareTag(TagsCollection.Enemy))
                {
                    PublishBeingTouched(collision);
                }
                break;
            case Team.Player:
                if (collision.CompareTag("Static_Attack_hitbox") || collision.CompareTag(TagsCollection.Enemy_Hitbox))
                {
                    PublishAttackedEvent(collision); 
                }
                break;

            case Team.Enemy:
                if (collision.CompareTag(TagsCollection.Player_Hitbox))
                {
                    PublishAttackedEvent(collision);
                }
                break;
        }  
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
                (transform.position - collision.gameObject.transform.position ).normalized, //concrete direction
                collision.gameObject, //attacker
                damageDealer.Damage, 
                damageDealer.Knockback,
                damageDealer.HitStop
                ));
        }
    }
    void PublishBeingTouched(Collider2D collider)
    {
        Vector2 direction = (transform.position - collider.transform.root.position).normalized;
        eventSystem.OnBeingTouchedObject?.Invoke(this, new Generic_EventSystem.ObjectDirectionArgs(direction));
    }
}

