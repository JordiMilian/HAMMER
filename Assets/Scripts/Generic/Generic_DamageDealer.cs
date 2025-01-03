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
    public bool isBloody;
    public bool isChargingSpecialAttack; // I think this is used by the player. For example de special attack shouldnt charge
    public bool isCharginSpecialAttack_whenParried;
    [SerializeField] int weaponIndex = 0;

    public enum Team
    {
        Player, Enemy, Object,
    }
    public Team EntityTeam;


    public EventHandler OnGettingParried;
    public Generic_EventSystem eventSystem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Generic_DamageDetector otherDetector = collision.gameObject.GetComponent<Generic_DamageDetector>();
        Generic_ParryDealer otherParryDealer = collision.gameObject.GetComponent<Generic_ParryDealer>();

        if(otherDetector != null) //
        {
            if (otherDetector.EntityTeam == Generic_DamageDetector.Team.Object)
            {
                PublishHitObject(collision);
            }
            switch (EntityTeam)
            {
                case Team.Player:
                    if (otherDetector.EntityTeam == Generic_DamageDetector.Team.Enemy || otherDetector.EntityTeam == Generic_DamageDetector.Team.Object)
                    {
                        PublishDealtDamageEvent(collision, otherDetector.canChargeSpecialAttack);
                    }
                    break;
                case Team.Enemy:
                    if(otherDetector.EntityTeam == Generic_DamageDetector.Team.Player || otherDetector.EntityTeam == Generic_DamageDetector.Team.Object)
                    {
                        PublishDealtDamageEvent(collision);
                    }
                    break;
            }
        }
        if(otherParryDealer != null)
        {
            switch (EntityTeam)
            {
                case Team.Player:
                    if(otherParryDealer.EntityTeam == Generic_ParryDealer.Team.Enemy)
                    {
                        PublishGettingParriedEvent();
                    }
                    break;
                case Team.Enemy:
                    if(otherParryDealer.EntityTeam == Generic_ParryDealer.Team.Player)
                    {
                        PublishGettingParriedEvent();
                    }
                    break;
                case Team.Object:
                    PublishGettingParriedEvent();
                    break;
            }
        }
        return;






        if(collision.CompareTag(Tags.Object_Hurtbox))
        {
            PublishHitObject(collision);
        }
        switch (EntityTeam)
        {
            case Team.Player:                
                if (collision.CompareTag(Tags.Enemy_Hurtbox))// || collision.GetComponent<Generic_DamageDetector>().EntityTeam == Generic_DamageDetector.Team.Player)
                {
                    bool isChargeable = collision.GetComponent<Generic_DamageDetector>().canChargeSpecialAttack; //Find if the damage Detector is chargeable and pass the info
                    PublishDealtDamageEvent(collision,isChargeable);
                }
                else if (collision.CompareTag(Tags.EnemyParryCollider))
                {
                    PublishGettingParriedEvent();
                }
                break;
                
            case Team.Enemy:
                if(collision.CompareTag(Tags.Player_Hurtbox))
                {
                    PublishDealtDamageEvent(collision);
                }
                if(collision.CompareTag(Tags.ParryCollider) && isParryable) 
                {
                    PublishGettingParriedEvent();
                }
                break;
        }
    }
    void PublishDealtDamageEvent(Collider2D collision, bool isChargeable = false)
    {
        float charge = 0;
        if (isChargeable && isChargingSpecialAttack) { charge = Damage; } //If the Dealer is charger and the detector is chargeable, then charge

        eventSystem.OnDealtDamage?.Invoke(this, new Generic_EventSystem.DealtDamageInfo(
        collision.ClosestPoint(gameObject.transform.position),
        collision.GetComponent<Generic_DamageDetector>().eventSystem.gameObject,
        Damage,
        charge
        ));
    }
    void PublishHitObject(Collider2D collision)
    {
        eventSystem.OnHitObject?.Invoke(this, new Generic_EventSystem.DealtDamageInfo(
            collision.ClosestPoint(gameObject.transform.position),
            collision.GetComponent<Generic_DamageDetector>().eventSystem.gameObject,
            Damage
            ));
    }
    void PublishGettingParriedEvent()
    {
        if (eventSystem.OnGettingParried != null) eventSystem.OnGettingParried(weaponIndex);
    }
   
}
