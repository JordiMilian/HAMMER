using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidTrianglesTesting : MonoBehaviour
{
    [SerializeField] Transform pointInside, pointA, pointB;
    [SerializeField] PolygonCollider2D testingPolygon;
    private void OnDrawGizmos()
    {
        GetClosestPositionOnEdgeOfPolygon(testingPolygon, pointInside.position);
    }
    bool DoPointsMakeAValidTriangle(Vector2 PInside, Vector2 P01, Vector2 P02)
    {
        float resultRad1 = Mathf.Atan2(PInside.y - P01.y, PInside.x - P01.x) -
                        Mathf.Atan2(P02.y - P01.y, P02.x - P01.x);
        float absoluteResult_1 = Mathf.Abs(resultRad1 * Mathf.Rad2Deg);
        if (absoluteResult_1 > 90) { return false; }

        float resultRad_2 = Mathf.Atan2(PInside.y - P02.y, PInside.x - P02.x) -
                        Mathf.Atan2(P01.y - P02.y, P01.x - P02.x);
        float absoluteResult_2 = Mathf.Abs(resultRad_2 * Mathf.Rad2Deg);
        
        if (absoluteResult_2 > 90) { return false; }

        Debug.Log(absoluteResult_1 + " _ " + absoluteResult_2);
        return true;
    }
    Vector2 GetClosestPositionOnEdgeOfPolygon(PolygonCollider2D polygon, Vector2 positionInside)
    {
        Vector2Int closestPoint = Vector2Int.zero;
        List<Vector2Int> validPairs = new List<Vector2Int>();
        Vector2 polygonPositionOffset = (Vector2)polygon.transform.position;

        for (int i = 0; i < polygon.points.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(polygonPositionOffset + polygon.points[i], 0.1f);

            if(i == polygon.points.Length - 1)
            {
                if (DoPointsMakeAValidTriangle(positionInside, polygonPositionOffset + polygon.points[0], polygonPositionOffset + polygon.points[i]))
                {
                    validPairs.Add(new Vector2Int(0, i));
                }
            }
            else if (DoPointsMakeAValidTriangle(positionInside, polygonPositionOffset + polygon.points[i + 1], polygonPositionOffset + polygon.points[i]))
            {
                validPairs.Add(new Vector2Int(i + 1, i));
            }
        }
        Gizmos.color = Color.green;
        foreach (Vector2Int pair in validPairs)
        {
            Gizmos.DrawWireSphere(polygon.points[pair.x], 0.1f);
            Gizmos.DrawWireSphere(polygon.points[pair.y], 0.1f);
        }
        return closestPoint;
    }
}
