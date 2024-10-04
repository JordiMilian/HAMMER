using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveToRigidBody : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Animator animator;
    [SerializeField] Transform MovementReference;
    [SerializeField] Transform Pivot;
    [SerializeField] Transform Base;
    public float Strengh;

    private void FixedUpdate()
    {
        //if (animator.GetBool("inIdle")) { return; }
        Vector2 CurrentLocalPosition = MovementReference.localPosition;
        Vector2 CurrentWorldPosition = Pivot.TransformPoint(CurrentLocalPosition);
        Vector2 NewLocalPosition = Base.InverseTransformPoint(CurrentWorldPosition) - Pivot.localPosition;

        //_rigidbody.velocity = NewLocalPosition * Strengh * Time.deltaTime;
    }
}
