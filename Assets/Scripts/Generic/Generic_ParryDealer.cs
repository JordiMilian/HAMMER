using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_ParryDealer : MonoBehaviour
{
    [SerializeField] Generic_EventSystem eventSystem;
    [SerializeField] Transform VFXPositionTransform;
    public enum Team
    {
        Player, Enemy,
    }
    public Team EntityTeam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (EntityTeam)
        {
            case Team.Player:
                if (collision.CompareTag(TagsCollection.Enemy_Hitbox))
                {
                    Generic_DamageDealer dealer = collision.GetComponent<Generic_DamageDealer>();
                    if (dealer.isParryable)
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position),dealer);
                    }    
                }
                break;
            case Team.Enemy:
                if(collision.CompareTag(TagsCollection.Player_Hitbox))
                {
                    Generic_DamageDealer dealer = collision.GetComponent<Generic_DamageDealer>();
                    if (dealer.isParryable)
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position), dealer);
                    }
                }
                break;
        }
    }
    void PublishSuccesfullParry(Vector3 collisionPoint, Generic_DamageDealer dealer)
    {
        if(eventSystem.OnSuccessfulParry != null) { eventSystem.OnSuccessfulParry(this, new Generic_EventSystem.SuccesfulParryInfo(collisionPoint,dealer)); }
        
    }
}
