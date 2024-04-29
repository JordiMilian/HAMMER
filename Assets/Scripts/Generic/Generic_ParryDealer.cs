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
                    PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position));
                }
                break;
            case Team.Enemy:
                if(collision.CompareTag(TagsCollection.Player_Hitbox))
                {
                    if (collision.GetComponent<Generic_DamageDealer>().isParryable)
                    {
                        PublishSuccesfullParry(collision.ClosestPoint(VFXPositionTransform.position));
                    }
                }
                break;
        }
    }
    void PublishSuccesfullParry(Vector3 collisionPoint)
    {
        if(eventSystem.OnSuccessfulParry != null) { eventSystem.OnSuccessfulParry(this, new Generic_EventSystem.SuccesfulParryInfo(collisionPoint)); }
        
    }
}
