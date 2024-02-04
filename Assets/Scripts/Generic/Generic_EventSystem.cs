using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_EventSystem : MonoBehaviour
{
    public class EventArgs_DealtDamageInfo
    {
        public Vector3 CollisionPosition;
        public EventArgs_DealtDamageInfo(Vector3 collisionPosition)
        {
            CollisionPosition = collisionPosition;
        }
    }
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

    public EventHandler OnDeath;
    public EventHandler OnUpdatedHealth;
    public EventHandler<EventArgs_DealtDamageInfo> OnDealtDamage;
    public EventHandler<EventArgs_ReceivedAttackInfo> OnReceiveDamage;
    public EventHandler OnGettingParried;
}
