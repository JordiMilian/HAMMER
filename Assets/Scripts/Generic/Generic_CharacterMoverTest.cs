using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Generic_CharacterMoverTest : MonoBehaviour
{
    //Generic_CharacterMover

    //do not ROTATE or SCALE polygon colliders 

    //do not ROTATE square colliders

    //do whatever with circle colliders

    public List<Vector2> MovementVectors = new List<Vector2>(); //Any movement stuff must be added here everyframe
    public float RootMotionMultiplier = 1; //This could be useful to adapt attacks to distance of player

    [SerializeField] Animator animator;

    List<Vector2> collisionPositions = new List<Vector2>(); 
    List<Collider2D> collidersInside = new List<Collider2D>();

    CircleCollider2D ownCollider;
    
    [Header("Testing")]
    [SerializeField] Vector2 TestDirectionToMove;
    [SerializeField] float speedMultiplier = 0.01f;
    
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        ownCollider = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        MovementVectors.Add(TestDirectionToMove);
        MovementVectors.Add(Vector2.zero);
    }
    private void Update()
    {
        collisionPositions.Clear();

        Vector2 calculatedDirection = Vector2.zero;

        MovementVectors.Add(TestDirectionToMove * speedMultiplier); //For testing delete

        // ----  MOVEMENTS  ----

        foreach(Vector2 movement in MovementVectors)
        {
            calculatedDirection += movement * Time.deltaTime;
        }
        MovementVectors.Clear(); //Not sure if I should clear this here

        // ----  ROOT MOTION  ----

        calculatedDirection += (Vector2)animator.deltaPosition * RootMotionMultiplier;

        // ----  COLISIONS  ---- 

            //Coliding colliders
        foreach (var collider in collidersInside)
        {
            Vector2 closestPoint = collider.ClosestPoint(transform.position);
            Vector2 direction = (closestPoint - (Vector2)transform.position).normalized;
            collisionPositions.Add(closestPoint);

            float distanceToColisionPoint = (closestPoint - (Vector2)transform.position).magnitude;

            float collisionDepth = (ownCollider.radius * ownCollider.transform.localScale.x) - distanceToColisionPoint;
            calculatedDirection += (-direction * (calculatedDirection.magnitude+ collisionDepth));
        }


            //Being inside a collider
        foreach(var collider in collidersInside)
        {
            if(collider.OverlapPoint(transform.position))
            {
                Vector2 exitVector = Vector2.positiveInfinity;
                //Aixo es una mica guarro i s'haurie de fer amb interfaces o yo que se, cada tipo de calcul potser s'haurie de fer en un script apart
                if(collider is PolygonCollider2D)
                {
                    if(collider.transform.rotation.z > 0.01f || collider.transform.rotation.z < -0.01f || collider.transform.localScale.x > 1.01f || collider.transform.localScale.x < 0.99f) 
                    { Debug.LogWarning("POLYGON COLLIDER WRONGLY MODIFIED: " + collider.gameObject.name); }

                    exitVector = GetExitVector_Polygon((PolygonCollider2D)collider, (Vector2)ownCollider.transform.position + ownCollider.offset);
                }
                else if (collider is BoxCollider2D)
                {
                    if (collider.transform.rotation.z > 0.01f || collider.transform.rotation.z < -0.01f)
                    { Debug.LogWarning("SQUARE COLLIDER WRONGLY MODIFIED: " + collider.gameObject.name); }

                    exitVector = GetExitVector_Square((BoxCollider2D)collider, (Vector2)ownCollider.transform.position + ownCollider.offset);
                }
                else if(collider is CircleCollider2D)
                {
                    exitVector = GetExitVector_Circle((CircleCollider2D)collider, (Vector2)ownCollider.transform.position + ownCollider.offset);
                }
                else
                {
                    Debug.LogError("Collision Type not considered, pls add more code lol");
                    return;
                }

                //Add the radius of the own circle to the direction 
                Vector2 exitDirection = exitVector.normalized;
                Vector2 radiusVector = exitDirection * ownCollider.radius * ownCollider.transform.localScale.x;
                calculatedDirection += exitVector + radiusVector;
            }
        }

        transform.position += (Vector3)calculatedDirection;
    }


    private void OnTriggerEnter2D(Collider2D collision) //The Layers set-up must be cleaned
    {
        collidersInside.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collidersInside.Remove(collision);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach(Vector2 position in collisionPositions)
        {
            Gizmos.DrawWireSphere(position, 0.1f);
        }
        foreach(Collider2D collider in collidersInside)
        {
            Vector2 closestPoint = collider.ClosestPoint(transform.position);
            Gizmos.DrawWireSphere(closestPoint, 0.2f);
        }
    }
    #region Polygon Colision Detection
    public Vector2 GetExitVector_Polygon(PolygonCollider2D polygon, Vector2 positionInside)
    {
        Vector2 closestVector = Vector2.zero;
        List<Vector2Int> validPairs = new List<Vector2Int>();
        Vector2 polygonPositionOffset = (Vector2)polygon.transform.position + polygon.offset;

        //Get valid pair of points in polygon
        for (int i = 0; i < polygon.points.Length; i++)
        {
            if (i == polygon.points.Length - 1)
            {
                Debug.DrawLine(polygonPositionOffset + polygon.points[0], polygonPositionOffset + polygon.points[i], Color.red);
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
        float closestPerpendiculatDistance = Mathf.Infinity;

        //Find perpendicular point and return the shortest
        for (int i = 0; i < validPairs.Count; i++)
        {
            Vector2 directionBetweenPoints = ((polygon.points[validPairs[i].y]) - (polygon.points[validPairs[i].x])).normalized;
            Vector2 perpendicularDirection = new Vector2(directionBetweenPoints.y, -directionBetweenPoints.x);

            Vector2 thisClosestPoint = GetPointOfIntersection(positionInside, polygonPositionOffset + polygon.points[validPairs[i].x], perpendicularDirection, directionBetweenPoints);

            float distanceToPoint = (positionInside - thisClosestPoint).magnitude;

            if (distanceToPoint < closestPerpendiculatDistance)
            {
                closestVector = thisClosestPoint - positionInside;
                closestPerpendiculatDistance = distanceToPoint;
            }
        }

        //Check vertices too
        float closestVertexDistance = Mathf.Infinity;
        int closestVertexIntex = -1;
        for (int i = 0; i < polygon.points.Length; i++)
        {
            float thisVertexDistance = (polygonPositionOffset + polygon.points[i] - positionInside).magnitude;

            if (thisVertexDistance < closestVertexDistance)
            {
                closestVertexDistance = thisVertexDistance;
                closestVertexIntex = i;
            }
        }

        if(closestVertexDistance < closestPerpendiculatDistance)
        {
            closestVector = (polygonPositionOffset + polygon.points[closestVertexIntex] - positionInside);
        }

        return closestVector;
    }
    bool DoPointsMakeAValidTriangle(Vector2 PInside, Vector2 P01, Vector2 P02)
    {
        //Check angle with point 01
        float resultRad1 = Mathf.Atan2(PInside.y - P01.y, PInside.x - P01.x) -
                        Mathf.Atan2(P02.y - P01.y, P02.x - P01.x);
        float absoluteResult_1 = Mathf.Abs(resultRad1 * Mathf.Rad2Deg);
        if (absoluteResult_1 > 90) { return false; }

        //Check angle with point 01
        float resultRad_2 = Mathf.Atan2(PInside.y - P02.y, PInside.x - P02.x) -
                        Mathf.Atan2(P01.y - P02.y, P01.x - P02.x);
        float absoluteResult_2 = Mathf.Abs(resultRad_2 * Mathf.Rad2Deg);
        if (absoluteResult_2 > 90) { return false; }

        return true;
    }

    Vector2 GetPointOfIntersection(Vector2 p1, Vector2 p2, Vector2 n1, Vector2 n2)
    {
        Vector2 p1End = p1 + n1; // another point in line p1->n1
        Vector2 p2End = p2 + n2; // another point in line p2->n2

        float m1 = (p1End.y - p1.y) / (p1End.x - p1.x); // slope of line p1->n1
        float m2 = (p2End.y - p2.y) / (p2End.x - p2.x); // slope of line p2->n2

        float b1 = p1.y - m1 * p1.x; // y-intercept of line p1->n1
        float b2 = p2.y - m2 * p2.x; // y-intercept of line p2->n2

        float px = (b2 - b1) / (m1 - m2); // collision x
        float py = m1 * px + b1; // collision y

        return new Vector2(px, py); // return statement
    }
    #endregion
    #region Circle Colision Detection
    public Vector2 GetExitVector_Circle(CircleCollider2D otherCollider, Vector2 insidePositon)
    {
        Vector2 otherCenterPos = (Vector2)otherCollider.transform.position + otherCollider.offset;

        Vector2 otherRadiusVector = (insidePositon - otherCenterPos).normalized * otherCollider.radius * otherCollider.transform.localScale.x;
        Vector2 exitVector = otherRadiusVector - (insidePositon - otherCenterPos);

        return exitVector;
    }
    #endregion
    #region Square Colision Detection
    public Vector2 GetExitVector_Square(BoxCollider2D otherCollider, Vector2 insidePosition)
    {
        Vector2 BoxCenter = otherCollider.bounds.center;
        Vector2 distanceToTopRightVertex = insidePosition - (BoxCenter + (Vector2)otherCollider.bounds.extents);
        Vector2 distanceToBottomLeftVertex = insidePosition - (BoxCenter - (Vector2)otherCollider.bounds.extents);

        float shortestDistance = Mathf.Abs(distanceToTopRightVertex.y);
        Vector2 movingDirection = new Vector2(0, 1);

        if (Mathf.Abs(distanceToTopRightVertex.x) < shortestDistance)
        {
            shortestDistance = Mathf.Abs(distanceToTopRightVertex.x);
            movingDirection = new Vector2(1, 0);
        }
        if (Mathf.Abs(distanceToBottomLeftVertex.y) < shortestDistance)
        {
            shortestDistance = Mathf.Abs(distanceToBottomLeftVertex.y);
            movingDirection = new Vector2(0, -1);
        }
        if (Mathf.Abs(distanceToBottomLeftVertex.x) < shortestDistance)
        {
            shortestDistance = Mathf.Abs(distanceToBottomLeftVertex.x);
            movingDirection = new Vector2(-1, 0);
        }
        return movingDirection * shortestDistance;
    }
    #endregion
}
