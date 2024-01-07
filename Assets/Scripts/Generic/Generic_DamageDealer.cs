using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_DamageDealer : MonoBehaviour
{
    public float Damage;
    public float Knockback;
    public float HitStop;
    public enum Team
    {
        Player, Enemy,
    }
    public Team EntityTeam;
    public enum TypeofDamage
    {
        Weapon, Static,
    }
    public TypeofDamage typeOfDamage;

    public class EventArgs_DealtDamageInfo
    {
        public Vector3 CollisionPosition;
        public EventArgs_DealtDamageInfo(Vector3 collisionPosition)
        {
            CollisionPosition = collisionPosition;
        }
    }
    public EventHandler<EventArgs_DealtDamageInfo> OnDealtDamage;
    public EventHandler OnGettingParried;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (EntityTeam)
        {
            case Team.Player:                
                if (collision.CompareTag("Enemy_Hitbox"))
                {
                    PublishDealtDamageEvent(collision);
                }
                break;
                
            case Team.Enemy:
                if(collision.CompareTag("PlayerDamageCollider"))
                {
                    PublishDealtDamageEvent(collision);
                }
                if(collision.CompareTag("ParryCollider"))
                {
                    PublishGettingParriedEvent();
                }
                break;
        }
    }
    void PublishDealtDamageEvent(Collider2D collision)
    {
        if (OnDealtDamage != null) OnDealtDamage(this, new EventArgs_DealtDamageInfo(
            collision.ClosestPoint(gameObject.transform.position)
            ));
    }
    void PublishGettingParriedEvent()
    {
        if (OnGettingParried != null) OnGettingParried(this, EventArgs.Empty);
    }
}
