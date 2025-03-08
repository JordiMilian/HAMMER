using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_ParryDealer : MonoBehaviour
{
    [SerializeField] Generic_EventSystem eventSystem;
    IParryDealer IparryDealer;
    [SerializeField] Transform VFXPositionTransform;
    public enum Team
    {
        Player, Enemy,
    }
    public Team EntityTeam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Generic_DamageDealer otherDealer = collision.GetComponent<Generic_DamageDealer>();

        if(otherDealer != null && otherDealer.isParryable)
        {
            switch(EntityTeam)
            {
                case Team.Enemy:
                    if(otherDealer.EntityTeam == Generic_DamageDealer.Team.Player)
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position), otherDealer, otherDealer.isCharginSpecialAttack_whenParried);
                    }
                    break;
                case Team.Player:
                    if(otherDealer.EntityTeam == Generic_DamageDealer.Team.Enemy)
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position), otherDealer, otherDealer.isCharginSpecialAttack_whenParried);
                    }
                    break;
            }
        }
        return;
    }
    void PublishSuccesfullParry(Vector3 collisionPoint, Generic_DamageDealer dealer, bool canCharge)
    {
        if(eventSystem.OnSuccessfulParry != null) { eventSystem.OnSuccessfulParry(this, new Generic_EventSystem.SuccesfulParryInfo(collisionPoint,dealer, canCharge)); }
        
    }
}
