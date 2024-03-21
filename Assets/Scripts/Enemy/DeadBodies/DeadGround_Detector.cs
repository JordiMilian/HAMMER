using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadGround_Detector : MonoBehaviour
{
    [SerializeField] DeadPartV3 deadPartV3;
    [SerializeField] float exitSpeed = 0.01f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(TagsCollection.BlockingWalls))
        {
            if (deadPartV3.currentPush != null)
            {
                deadPartV3.triggerDetector.GetComponent<Collider2D>().enabled = true;
            }
            //Push in the other direction
            Vector2 originPosition = transform.root.position;
            Vector2 colisionPoint = collision.ClosestPoint(originPosition);
            Vector2 opositeDirection = (colisionPoint - originPosition).normalized;

            deadPartV3.OnHitWall();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(TagsCollection.BlockingWalls))
        {


                Vector2 impactpoint = collision.ClosestPoint(transform.position);
                Vector2 ownPosition = transform.position;
                Vector2 exitDirection = (ownPosition - impactpoint).normalized;
                Debug.DrawLine(ownPosition, ownPosition + exitDirection);

                deadPartV3.movingParent.Translate(exitDirection * exitSpeed);
            
        }
    }
}
