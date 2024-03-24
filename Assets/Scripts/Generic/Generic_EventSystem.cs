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
    public class EventArgs_SuccesfulParryInfo : EventArgs
    {
        public Vector3 vector3data;
        public EventArgs_SuccesfulParryInfo(Vector3 data)
        {
            vector3data = data;
        }
    }
    public class Args_DeadCharacter
    {
        public GameObject DeadGameObject;
        public GameObject Killer;
        public Args_DeadCharacter(GameObject deadGameObject, GameObject killer)
        {
            DeadGameObject = deadGameObject;
            Killer = killer;
        }
    }

    public EventHandler<Args_DeadCharacter> OnDeath;
    public Action OnAttackFinished;
    public Action OnUpdatedHealth;
    public EventHandler<EventArgs_DealtDamageInfo> OnDealtDamage;
    public EventHandler<EventArgs_ReceivedAttackInfo> OnReceiveDamage;
    public Action OnGettingParried;
    public EventHandler<EventArgs_SuccesfulParryInfo> OnSuccessfulParry;
    public Action OnHitObject;
}
