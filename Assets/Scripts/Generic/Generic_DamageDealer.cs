using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Generic_DamageDealer : MonoBehaviour
{
    public float Damage;
    public float Knockback;
    public float HitStop;
    public bool isParryable;

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


    public EventHandler OnGettingParried;
    [SerializeField] Generic_EventSystem eventSystem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (EntityTeam)
        {
            case Team.Player:                
                if (collision.CompareTag(TagsCollection.Instance.Enemy_Hitbox))
                {
                    PublishDealtDamageEvent(collision);
                }
                else if (collision.CompareTag(TagsCollection.Instance.EnemyParryCollider))
                {
                    PublishGettingParriedEvent();
                }
                break;
                
            case Team.Enemy:
                if(collision.CompareTag(TagsCollection.Instance.PlayerDamageCollider))
                {
                    PublishDealtDamageEvent(collision);
                }
                if(collision.CompareTag(TagsCollection.Instance.ParryCollider) && isParryable) 
                {
                    PublishGettingParriedEvent();
                }
                break;
        }
    }
    void PublishDealtDamageEvent(Collider2D collision)
    {
        if (eventSystem.OnDealtDamage != null)
        {
            eventSystem.OnDealtDamage(this, new Generic_EventSystem.EventArgs_DealtDamageInfo(
            collision.ClosestPoint(gameObject.transform.position)
            ));
        }
    }
    void PublishGettingParriedEvent()
    {
        if (eventSystem.OnGettingParried != null) eventSystem.OnGettingParried();
    }
   
}
