using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_ParryDealer : MonoBehaviour
{
    IParryDealer thisParryDealer;
    public Transform rootGameObject;
    [SerializeField] Transform VFXPositionTransform;

    private void OnValidate()
    {
        if (rootGameObject != null)
        {
            IParryDealer parryDealerTemp = rootGameObject.GetComponent<IParryDealer>();

            if (parryDealerTemp == null) 
            {
                Debug.LogWarning("Root Game)Object doesn't implement IParryDealer");
                rootGameObject = null;
                return;
            }
        }
    }

    public DamagersTeams EntityTeam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Generic_DamageDealer otherDealer = collision.GetComponent<Generic_DamageDealer>();

        if(otherDealer != null && otherDealer.isParryable)
        {
            switch(EntityTeam)
            {
                case DamagersTeams.Enemy:
                    if(otherDealer.EntityTeam == DamagersTeams.Player)
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position), otherDealer, otherDealer.isCharginSpecialAttack_whenParried);
                    }
                    break;
                case DamagersTeams.Player:
                    if(otherDealer.EntityTeam == DamagersTeams.Enemy)
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
        thisParryDealer.OnParryDealt(new SuccesfulParryInfo(collisionPoint,dealer, canCharge, dealer.rootGameObject_ParryReceiver.gameObject));
        
    }
}
