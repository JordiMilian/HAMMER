using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Generic_DamageDealer : MonoBehaviour
{
    public float Damage;
    [HideInInspector]public float Stagger = 1f;
    public float Knockback;
    public bool isParryable;
    public bool isBloody;
    public bool player_isChargingSpecialAttack; // I think this is used by the player. For example de special attack shouldnt charge
    public bool isCharginSpecialAttack_whenParried;
    [SerializeField] int weaponIndex = 0;

    public DamagersTeams EntityTeam;

    public IDamageDealer thisDamageDealer;
    public IParryReceiver thisParryReceiver;
    public Transform rootGameObject_DamageDealerTf;
    public Transform rootGameObject_ParryReceiver;

    //We have so, when a single swipe of an attack doesnt hit the same receiver multiple times. We usually reset them in the EV_ShowAttackCollider
    [HideInInspector] public List<Generic_DamageDetector> damagedReceivers = new();
    public void ResetDetectedReceivers() { damagedReceivers.Clear(); }
    private void OnValidate()
    {
        if (rootGameObject_DamageDealerTf != null)
        {
            IDamageDealer damageDealerTemp = rootGameObject_DamageDealerTf.GetComponent<IDamageDealer>();
            if(damageDealerTemp == null)
            {
                Debug.LogWarning("Root doesn't implement IDamageDealer");
                rootGameObject_DamageDealerTf = null;
            }
            else { thisDamageDealer = damageDealerTemp; }
               
        }
        if (rootGameObject_ParryReceiver != null)
        {
            IParryReceiver parryReceiverTemp = rootGameObject_ParryReceiver.GetComponent<IParryReceiver>();
            if (parryReceiverTemp == null)
            {
                Debug.LogWarning("Root doesn't implement IParryReceiver");
                rootGameObject_ParryReceiver = null;
                isParryable = false;
            }
            else
            {
                thisParryReceiver = parryReceiverTemp;
                isParryable = true;
            }
        }
    }
    private void OnEnable()
    {
        OnValidate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Generic_DamageDetector otherDetector = collision.gameObject.GetComponent<Generic_DamageDetector>();
        Generic_ParryDealer otherParryDealer = collision.gameObject.GetComponent<Generic_ParryDealer>();

        if(otherDetector != null) //
        {
            if (damagedReceivers.Contains(otherDetector)) { return; }
            damagedReceivers.Add(otherDetector);

            switch (EntityTeam)
            {
                case DamagersTeams.Player:
                    if (otherDetector.EntityTeam == DamagersTeams.Enemy || otherDetector.EntityTeam == DamagersTeams.Neutral)
                    {
                        PublishDealtDamageEvent(collision, otherDetector.enemy_canChargeSpecialAttack);
                    }
                    break;
                case DamagersTeams.Enemy:
                    if(otherDetector.EntityTeam == DamagersTeams.Player || otherDetector.EntityTeam == DamagersTeams.Neutral)
                    {
                        PublishDealtDamageEvent(collision);
                    }
                    break;
                case DamagersTeams.Neutral:
                    PublishDealtDamageEvent(collision);
                    break;

            }
        }
        /*
        if(otherParryDealer != null)
        {
            if (!isParryable) { return; }

            switch (EntityTeam)
            {
                case DamagersTeams.Player:
                    if(otherParryDealer.EntityTeam == DamagersTeams.Enemy)
                    {
                        PublishGettingParriedEvent(otherParryDealer.gameObject);
                    }
                    break;
                case DamagersTeams.Enemy:
                    if(otherParryDealer.EntityTeam == DamagersTeams.Player)
                    {
                        PublishGettingParriedEvent(otherParryDealer.gameObject);
                    }
                    break;
                case DamagersTeams.Neutral:
                    PublishGettingParriedEvent(otherParryDealer.gameObject);
                    break;
            }
        }
        */
    }
    void PublishDealtDamageEvent(Collider2D collision, bool isReceiverChargeable = false)
    {
        Generic_DamageDetector otherDetector = collision.GetComponent<Generic_DamageDetector>();
        otherDetector.PublishAttackedEvent(GetComponent<Collider2D>());
        float charge = 0;

        if (isReceiverChargeable && player_isChargingSpecialAttack) { charge = Stagger; Debug.Log($"Add {charge} charge player"); } //If the Dealer is charger and the detector is chargeable, then charge

        thisDamageDealer.OnDamageDealt(new DealtDamageInfo(
            collision.ClosestPoint(gameObject.transform.position), //collision point
            otherDetector.rootGameObject.gameObject, //root of detector
            Damage, //Damage
            Stagger,
            otherDetector, //damage detector
            charge //charge
            ));
    }
    public void PublishGettingParriedEvent(GameObject parrier)
    {
        Vector2 ParrierDirection = (gameObject.transform.position - parrier.transform.position).normalized;
        thisParryReceiver.OnParryReceived(new GettingParriedInfo(parrier, weaponIndex, ParrierDirection));
    }
   
}
