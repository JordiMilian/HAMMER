using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_DamageDetector : MonoBehaviour
{
    public enum Team
    {
        Player, Enemy,
    }
    public Team EntityTeam;
    public EventHandler<EventArgs_ReceivedAttackInfo> OnReceiveDamage;
   
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
            case Team.Player:
                if (collision.CompareTag("Static_Attack_hitbox") || collision.CompareTag("Enemy_Attack_hitbox"))
                {
                    PublishAttackEvent(collision); 
                }
                break;

            case Team.Enemy:
                if (collision.CompareTag("Attack_Hitbox"))
                {
                    PublishAttackEvent(collision);
                }
                break;
        }  
    }
    void PublishAttackEvent(Collider2D collision)
    {
        if (OnReceiveDamage != null)
        {
            Generic_DamageDealer damageDealer = collision.gameObject.GetComponent<Generic_DamageDealer>();

            OnReceiveDamage(this, new EventArgs_ReceivedAttackInfo
                (
                collision.ClosestPoint(gameObject.transform.position),
                collision.gameObject,
                damageDealer.Damage,
                damageDealer.Knockback,
                damageDealer.HitStop
                ));
        }
    }
}

