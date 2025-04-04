using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_MoveTowardsPlayer : MonoBehaviour
{
    [SerializeField] CharacterMover2 xpMover;
    [SerializeField] float speedPerSecond;
    Transform Target;

    bool isPlayerInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            Target = collision.transform;
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            Target = null;
            isPlayerInRange = false;
        }
    }
    private void Update()
    {
        if(isPlayerInRange && Target != null)
        {
            Vector2 directionToTarget = (Target.position - transform.position).normalized;
            Vector2 movementVector = directionToTarget * speedPerSecond;
            xpMover.MovementVectorsPerSecond.Add(movementVector);
        }
    }
}
