using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ValidTrianglesTesting : MonoBehaviour
{
    [SerializeField] Transform pointInside, pointA, pointB;
    [SerializeField] PolygonCollider2D testingPolygon;
    [SerializeField] CircleCollider2D testingCircle;
    [SerializeField] BoxCollider2D testingBox;
    [SerializeField] CircleCollider2D thisCircle;
    [SerializeField] Generic_CharacterMover rootMotionToAccessScript;
    [SerializeField] bool testCircle, testPolygon, testSquare;
    
    private void OnDrawGizmos()
    {
        Matrix4x4 polygonMatrix = Matrix4x4.TRS(testingPolygon.transform.position, testingPolygon.transform.rotation, testingPolygon.transform.localScale);
        Gizmos.matrix = polygonMatrix;

        Gizmos.color = Color.red;
        for (int i = 0; i < testingPolygon.points.Length; i++)
        {
            if(i == testingPolygon.points.Length - 1) { Gizmos.DrawLine(testingPolygon.points[i], testingPolygon.points[0]); }
            else
            {
                Gizmos.DrawLine(testingPolygon.points[i], testingPolygon.points[i + 1]);
            }
            
        }
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.blue;
        if (testCircle)
        {
            Vector2 closestExit = rootMotionToAccessScript.GetExitVector_Circle(testingCircle, pointInside.position);
            Gizmos.DrawLine(pointInside.position, (Vector2)pointInside.position + closestExit);
        }
        if(testingPolygon)
        {
            Vector2 closesExitPolygon = rootMotionToAccessScript.GetExitVector_Polygon(testingPolygon, pointInside.position);
            Gizmos.DrawLine(pointInside.position, (Vector2)pointInside.position + closesExitPolygon);
        }
        if(testSquare)
        {
            Vector2 closestExitSquare = rootMotionToAccessScript.GetExitVector_Square(testingBox, pointInside.position);
            Gizmos.DrawLine(pointInside.position, (Vector2)pointInside.position + closestExitSquare);
        }
    }
    

}
