using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryDetector : MonoBehaviour
{
    [SerializeField] Player_ParryPerformer parryPerformer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy_Attack_hitbox"))
        {
            parryPerformer.PublishSuccessfullParryDone(collision.ClosestPoint(transform.position));
        }
    }
}
