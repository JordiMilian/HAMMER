using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_EventSystem : MonoBehaviour
{
    public class DealtDamageInfo
    {
        public Vector3 CollisionPosition;
        public DealtDamageInfo(Vector3 collisionPosition)
        {
            CollisionPosition = collisionPosition;
        }
    }
    public class ReceivedAttackInfo : EventArgs
    {
        public Vector3 CollisionPosition;
        public Vector2 GeneralDirection;
        public Vector2 ConcreteDirection;
        public GameObject Attacker;
        public float Damage;
        public float KnockBack;
        public float Hitstop;
        public bool IsBloody;
        public ReceivedAttackInfo(Vector2 collisionPosition,
            Vector2 Gdirection, Vector2 Cdirection, GameObject attacker, float damage, float knockBack, float hitstop, bool isBloody)
        {
            CollisionPosition = collisionPosition;
            GeneralDirection = Gdirection;
            ConcreteDirection = Cdirection;
            Attacker = attacker;
            Damage = damage;
            KnockBack = knockBack;
            Hitstop = hitstop;
            IsBloody = isBloody;
        }
    }
    public class SuccesfulParryInfo : EventArgs
    {
        public Vector3 ParryPosition;
        public SuccesfulParryInfo(Vector3 data)
        {
            ParryPosition = data;
        }
    }
    public class DeadCharacterInfo
    {
        public GameObject DeadGameObject;
        public GameObject Killer;
        public DeadCharacterInfo(GameObject deadGameObject, GameObject killer)
        {
            DeadGameObject = deadGameObject;
            Killer = killer;
        }
    }
    public class ObjectDirectionArgs : EventArgs
    {
        public Vector2 GeneralDirection;
        public ObjectDirectionArgs(Vector2 Gdirection)
        {
            GeneralDirection = Gdirection;
        }
    }

    public EventHandler<DeadCharacterInfo> OnDeath;
    public Action OnAttackFinished;
    public Action OnUpdatedHealth;
    public EventHandler<DealtDamageInfo> OnDealtDamage;
    public EventHandler<ReceivedAttackInfo> OnReceiveDamage;
    public Action<int> OnGettingParried;
    public EventHandler<SuccesfulParryInfo> OnSuccessfulParry;
    public EventHandler<DealtDamageInfo> OnHitObject;
    public EventHandler<ObjectDirectionArgs> OnBeingTouchedObject;
    public Action OnShowCollider; //Currently for sounds
}
