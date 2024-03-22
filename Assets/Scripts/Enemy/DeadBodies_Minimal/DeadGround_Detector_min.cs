using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadGround_Detector_min : MonoBehaviour
{
    [SerializeField] DeadPartV3_Min deadPartV3;
    [SerializeField] DeadPart_EventSystem_min eventSystem;
    [SerializeField] float exitSpeed = 0.01f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BlockingWalls"))
        {
            
            eventSystem.OnHitWall?.Invoke();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BlockingWalls"))
        {

                //Cheap stuff to make sure it doesnt go throw walls
                Vector2 impactpoint = collision.ClosestPoint(transform.position);
                Vector2 ownPosition = transform.position;
                Vector2 exitDirection = (ownPosition - impactpoint).normalized;
                Debug.DrawLine(ownPosition, ownPosition + exitDirection);

                deadPartV3.movingParent.Translate(exitDirection * exitSpeed);
            
        }
    }
}
