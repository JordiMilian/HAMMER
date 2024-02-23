using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveToAddForce : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] Transform CurveObject;
    Vector2 LastPosition;
    Vector2 CurrentPosition;
    Vector2 Distance1Frame;
    
    private void Start()
    {
       LastPosition = CurveObject.localPosition;
    CurrentPosition = CurveObject.localPosition;
    }
    private void Update()
    {
        CurrentPosition = CurveObject.localPosition;
        Distance1Frame = CurrentPosition - LastPosition;
        Debug.Log(Distance1Frame);
       
        _rigidbody.AddForce(Distance1Frame, ForceMode2D.Impulse);
        LastPosition = CurrentPosition;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(Vector2.zero, Distance1Frame * 10);
    }

}
