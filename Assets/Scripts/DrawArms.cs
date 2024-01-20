using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArms : MonoBehaviour
{
    [SerializeField] Transform OriginPosition;
    [SerializeField] Transform DestinationPosition;
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(OriginPosition.position, DestinationPosition.position);
    }
}
