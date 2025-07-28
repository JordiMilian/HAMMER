using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_ParryDealer : MonoBehaviour
{
    
    [SerializeField] Transform VFXPositionTransform;
    [SerializeField] Generic_DamageDetector ownDamageDetector;

    IParryDealer thisParryDealer;
    public GameObject rootGameObject;
    private void OnValidate()
    {
        if (rootGameObject != null)
        {
            UsefullMethods.CheckIfGameobjectImplementsInterface<IParryDealer>(ref rootGameObject, ref thisParryDealer);
        }
    }
    private void OnEnable()
    {
        OnValidate();
    }


    public DamagersTeams EntityTeam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Generic_DamageDealer otherDealer = collision.GetComponent<Generic_DamageDealer>();

        if(otherDealer != null && otherDealer.isParryable && !otherDealer.damagedReceivers.Contains(ownDamageDetector))
        {
            switch(EntityTeam)
            {
                case DamagersTeams.Enemy:
                    if(otherDealer.EntityTeam == DamagersTeams.Player || otherDealer.EntityTeam == DamagersTeams.Neutral )
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position), otherDealer, otherDealer.isCharginSpecialAttack_whenParried);
                    }
                    break;
                case DamagersTeams.Player:
                    if(otherDealer.EntityTeam == DamagersTeams.Enemy || otherDealer.EntityTeam == DamagersTeams.Neutral)
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position), otherDealer, otherDealer.isCharginSpecialAttack_whenParried);
                    }
                    break;
                case DamagersTeams.Neutral:
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position), otherDealer, otherDealer.isCharginSpecialAttack_whenParried);
                        break;
                    }
            }
        }
        return;
    }
    void PublishSuccesfullParry(Vector3 collisionPoint, Generic_DamageDealer dealer, bool canCharge)
    {
        thisParryDealer.OnParryDealt(new SuccesfulParryInfo(collisionPoint,dealer, canCharge, dealer.rootGameObject_ParryReceiver.gameObject));
        dealer.PublishGettingParriedEvent(gameObject);
        
    }
}
