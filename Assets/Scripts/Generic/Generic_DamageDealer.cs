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
    IDamageDealer IdamageDealer;
    IParryReceiver IparryReceiver;
    [SerializeField] Component damageDealer_InterfaceHolder;
    [SerializeField] Component parryReceiver_InterfaceHolder;
    private void OnValidate()
    {
        if(damageDealer_InterfaceHolder != null && damageDealer_InterfaceHolder.GetComponent<IDamageDealer>() == null)
        {
            damageDealer_InterfaceHolder = null;
        }
        if(parryReceiver_InterfaceHolder != null && parryReceiver_InterfaceHolder.GetComponent<IParryReceiver>() == null)
        {
            parryReceiver_InterfaceHolder = null;
            isParryable = false;
        }
        else
        {
            isParryable = true;
        }
    }
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
                        PublishGettingParriedEvent(otherParryDealer.gameObject);
                    }
                    break;
                case Team.Enemy:
                    if(otherParryDealer.EntityTeam == Generic_ParryDealer.Team.Player)
                    {
                        PublishGettingParriedEvent(otherParryDealer.gameObject);
                    }
                    break;
                case Team.Object:
                    PublishGettingParriedEvent(otherParryDealer.gameObject);
                    break;
            }
        }
    }
    void PublishDealtDamageEvent(Collider2D collision, bool isChargeable = false)
    {
        float charge = 0;
        if (isChargeable && isChargingSpecialAttack) { charge = Damage; } //If the Dealer is charger and the detector is chargeable, then charge

        IdamageDealer.OnDamageDealt(new DealtDamageInfo(
            collision.ClosestPoint(gameObject.transform.position),
            collision.GetComponent<Generic_DamageDetector>().eventSystem.gameObject,
            Damage,
            collision.GetComponent<Generic_DamageDetector>(),
            charge
            ));
    }
    void PublishHitObject(Collider2D collision)
    {
        /*
        eventSystem.OnHitObject?.Invoke(this, new Generic_EventSystem.DealtDamageInfo(
            collision.ClosestPoint(gameObject.transform.position),
            collision.GetComponent<Generic_DamageDetector>().eventSystem.gameObject,
            Damage
            ));
        */
    }
    void PublishGettingParriedEvent(GameObject parrier)
    {
        Vector2 ParrierDirection = (gameObject.transform.position - parrier.transform.position).normalized;
        IparryReceiver.OnParryReceived(new GettingParriedInfo(parrier, weaponIndex, ParrierDirection));
    }
   
}
