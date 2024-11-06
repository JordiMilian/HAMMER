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
    public enum TypeofDamage
    {
        Weapon, Static,
    }
    public TypeofDamage typeOfDamage;


    public EventHandler OnGettingParried;
    [SerializeField] Generic_EventSystem eventSystem;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.Object_Hurtbox))
        {
            PublishHitObject(collision);
        }
        switch (EntityTeam)
        {
            case Team.Player:                
                if (collision.CompareTag(TagsCollection.Enemy_Hurtbox))// || collision.GetComponent<Generic_DamageDetector>().EntityTeam == Generic_DamageDetector.Team.Player)
                {
                    bool isChargeable = collision.GetComponent<Generic_DamageDetector>().canChargeSpecialAttack; //Find if the damage Detector is chargeable and pass the info
                    PublishDealtDamageEvent(collision,isChargeable);
                }
                else if (collision.CompareTag(TagsCollection.EnemyParryCollider))
                {
                    PublishGettingParriedEvent();
                }
                break;
                
            case Team.Enemy:
                if(collision.CompareTag(TagsCollection.Player_Hurtbox))
                {
                    PublishDealtDamageEvent(collision);
                }
                if(collision.CompareTag(TagsCollection.ParryCollider) && isParryable) 
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
        collision.gameObject,
        Damage,
        charge
        ));
    }
    void PublishHitObject(Collider2D collision)
    {
        eventSystem.OnHitObject?.Invoke(this, new Generic_EventSystem.DealtDamageInfo(
            collision.ClosestPoint(gameObject.transform.position),
            collision.gameObject,
            Damage
            ));
    }
    void PublishGettingParriedEvent()
    {
        if (eventSystem.OnGettingParried != null) eventSystem.OnGettingParried(weaponIndex);
    }
   
}
