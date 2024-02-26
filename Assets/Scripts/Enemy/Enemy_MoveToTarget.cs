using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveToTarget : MonoBehaviour
{
    public Transform Target;
    [SerializeField] Rigidbody2D _rigidBody;
    public float Velocity;
    Vector2 DirectionToTarget;
    public bool DoMove;
    private void Update()
    {
        if(DoMove)
        {
            if (Target == null)
            {
                Debug.Log(gameObject.name + " has no target");
                StopMoving();
                return;
            }
            DirectionToTarget = (Target.position - transform.position).normalized;
            _rigidBody.AddForce( DirectionToTarget * Velocity * Time.deltaTime);

        }
        else { StopMoving(); }
    }
    void StopMoving()
    {
        _rigidBody.velocity = Vector2.zero;
    }
    // TO DO maybe
    IEnumerator SlowDown()
    {
        yield return null;
    }
}
