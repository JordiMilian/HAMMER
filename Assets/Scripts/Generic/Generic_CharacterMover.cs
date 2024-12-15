using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Generic_CharacterMover : MonoBehaviour
{
    //do not ROTATE square colliders

    //The circle collider of charactesr should not be offseted by anything

    
    public float RootMotionMultiplier = 1; //This could be useful to adapt attacks to distance of player
    [SerializeField] bool drawLines, stopMove;
    Vector2 _currentVelocity;
    float _currentMagnitude;
    public Vector2 currentVelocity
    {
        get { return _currentVelocity; }
    }

    [SerializeField] Animator animator;
    [SerializeField] bool ignoreCollisions;

    List<Vector2> collisionPositions = new List<Vector2>();
   public List<Collider2D> collidersInside = new List<Collider2D>();

    CircleCollider2D ownCollider;

    [SerializeField] float velocityLimit;
    [Range(0,1)]
    [SerializeField] float collisionDampint = 0.77f;
    [Range(0, 1)]
    [SerializeField] float collisioninsideDampint = 0.25f;
    [Header("Testing")]
    [SerializeField] Vector2 TestDirectionToMove;
    [SerializeField] float speedMultiplier = 0.01f;
    [SerializeField] LayerMask layersToHit;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        ownCollider = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        MovementVectorsPerSecond.Add(TestDirectionToMove);
        MovementVectorsPerSecond.Add(Vector2.zero);
    }


    public List<Vector2> MovementVectorsPerSecond = new List<Vector2>(); //Any movement stuff must be added here everyframe

    private void Update()
    {
        collisionPositions.Clear();
        float ownRadius = 0;
        if(ownCollider != null) { ownRadius = ownCollider.radius * Mathf.Max(ownCollider.transform.localScale.x, ownCollider.transform.localScale.y); }
        
        MovementVectorsPerSecond.Add(TestDirectionToMove * speedMultiplier); //For testing delete

        Vector2 calculatedDirection = Vector2.zero;

        // ----  MOVEMENTS  ----

        foreach (Vector2 movement in MovementVectorsPerSecond)
        {
            calculatedDirection += movement * Time.deltaTime;
        }

        MovementVectorsPerSecond.Clear();

        // ----  ROOT MOTION  ----

        if(animator != null)
        {
            calculatedDirection += (Vector2)animator.deltaPosition * RootMotionMultiplier;
        }

        //  ---- RAYCAST TO NOT ENTER COLLIDERS ----
        
        float currentMagnitude = calculatedDirection.magnitude;
        Vector2 raycastOrigin = (Vector2)transform.position + (calculatedDirection.normalized * -ownCollider.radius);


       
        RaycastHit2D[] raycasts = Physics2D.RaycastAll(raycastOrigin
            ,calculatedDirection.normalized
            ,currentMagnitude + (ownCollider.radius * 2)
            ,layersToHit
            );
        Debug.DrawLine(raycastOrigin, raycastOrigin + (calculatedDirection.normalized * ((ownCollider.radius * 2) + currentMagnitude)), Color.red);
        foreach (RaycastHit2D ray in raycasts)
        {
            if (ray && ray.collider != ownCollider && currentMagnitude > 0)
            {
                float collisionDepth = (ownCollider.radius * 2) - ray.distance;

                calculatedDirection = calculatedDirection.normalized * -collisionDepth;
                Debug.Log("I myself: " + gameObject.name + " detected a bad wall in " + ray.transform.gameObject.name);
            }
        }
       
        
        


       // ----  COLISIONS  ---- 




            foreach (var collider in collidersInside)
        {
            //Colliders outside
            if (!collider.OverlapPoint(transform.position))
            {
                Vector2 closestPoint = collider.ClosestPoint(transform.position);
                Vector2 direction = (closestPoint - (Vector2)transform.position).normalized;
                collisionPositions.Add(closestPoint);

                float distanceToColisionPoint = (closestPoint - (Vector2)transform.position).magnitude;

                float collisionDepth = ownRadius - distanceToColisionPoint;
                calculatedDirection += (-direction * (calculatedDirection.magnitude + collisionDepth)) * collisionDampint;
            }

            //Colliders inside
            else
            {
                Debug.Log("Collider inside of: " + collider.name + " let's teleport");
                Vector2 exitVector = Vector2.positiveInfinity;
                //Aixo es una mica guarro i s'haurie de fer amb interfaces o yo que se, cada tipo de calcul potser s'haurie de fer en un script apart
                if (collider is PolygonCollider2D)
                {
                    exitVector = GetExitVector_Polygon((PolygonCollider2D)collider, (Vector2)ownCollider.transform.position + ownCollider.offset);
                }
                else if (collider is BoxCollider2D)
                {
                    if (collider.transform.rotation.z > 0.01f || collider.transform.rotation.z < -0.01f)
                    { Debug.LogWarning("SQUARE COLLIDER WRONGLY MODIFIED: " + collider.gameObject.name); }

                    exitVector = GetExitVector_Square((BoxCollider2D)collider, (Vector2)ownCollider.transform.position + ownCollider.offset);
                }
                else if (collider is CircleCollider2D)
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
                
                Vector2 radiusVector = exitDirection * ownRadius;
                calculatedDirection += (exitVector + radiusVector) * collisioninsideDampint;

                if (drawLines)
                {
                    Debug.DrawLine(transform.position, transform.position + (Vector3)exitVector, Color.blue);
                    Debug.DrawLine(transform.position + (Vector3)exitVector, transform.position + (Vector3)exitVector + (Vector3)radiusVector, Color.yellow);
                }
            }
        }

        if (float.IsNaN(calculatedDirection.x) || float.IsNaN(calculatedDirection.y))
        {
            Debug.LogWarning("Fallo de calculas al moure???¿?¿?¿: " + gameObject.name);
            return;
        }
        
        _currentVelocity = calculatedDirection;

        _currentMagnitude = _currentVelocity.magnitude;

        if(_currentMagnitude > velocityLimit)
        {
            Debug.LogWarning("Either a math bug or an attack variable is too hight. Anyway, " + _currentMagnitude + " is too much, so chill out");
            _currentVelocity = _currentVelocity.normalized * velocityLimit;
        }
    }
    private void LateUpdate()
    {
        if (stopMove) { return; }
        transform.position += (Vector3)_currentVelocity;
    }
    private void OnAnimatorMove()
    {
        return; //I believe this overrunes the default rootmotions
    }

    private void OnTriggerEnter2D(Collider2D collision) //The Layers set-up must be cleaned
    {
        if (ignoreCollisions) { return; }
        if (collidersInside.Contains(collision)) { return; }
        if(collision.gameObject.layer == 20 || collision.gameObject.layer == 16) //mainCollision or JumpableWall
        {
            collidersInside.Add(collision);
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ignoreCollisions) { return; }
        if (collision.gameObject.layer == 20 || collision.gameObject.layer == 16)
        {
            collidersInside.Remove(collision);
        }
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.blue;
        foreach (Vector2 position in collisionPositions)
        {
            Gizmos.DrawWireSphere(position, 0.1f);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + ((Vector3)_currentVelocity));
        /*
        foreach (Collider2D collider in collidersInside)
        {
            Vector2 closestPoint = collider.ClosestPoint(transform.position);
            Gizmos.DrawWireSphere(closestPoint, 0.2f);
        
        */
    }
    #region Polygon Colision Detection
    public Vector2 GetExitVector_Polygon(PolygonCollider2D polygon, Vector2 positionInside)
    {
        Vector2 closestVector = Vector2.zero;
        
        //Convert point into world space
        List<Vector2> polygonPointsInWorld = new List<Vector2>();
        Matrix4x4 polygonMatrix = Matrix4x4.TRS(polygon.transform.position, polygon.transform.rotation, polygon.transform.localScale);
        for (int i = 0; i < polygon.points.Length; i++)
        {
            polygonPointsInWorld.Add(polygonMatrix.MultiplyPoint(polygon.points[i]));
        }
        //Get valid pair of points in polygon
        List<Vector2Int> validPairs = new List<Vector2Int>();
        for (int i = 0; i < polygon.points.Length; i++)
        {
            //GIZMO DRAWING
            //if (i == polygon.points.Length - 1) { Gizmos.DrawLine(polygonPointsInWorld[i], polygonPointsInWorld[0]); }
            //else { Gizmos.DrawLine(polygonPointsInWorld[i], polygonPointsInWorld[i + 1]); }

            if (i == polygon.points.Length - 1)
            {
                if (DoPointsMakeAValidTriangle(positionInside, polygonPointsInWorld[0], polygonPointsInWorld[i]))
                {
                    validPairs.Add(new Vector2Int(0, i));
                }
            }
            else if (DoPointsMakeAValidTriangle(positionInside, polygonPointsInWorld[i + 1], polygonPointsInWorld[i]))
            {
                validPairs.Add(new Vector2Int(i + 1, i));
            }
        }
        float closestPerpendiculatDistance = Mathf.Infinity;

        //Find perpendicular point and return the shortest
        for (int i = 0; i < validPairs.Count; i++)
        {
            Vector2 directionBetweenPoints = ((polygonPointsInWorld[validPairs[i].y]) - (polygonPointsInWorld[validPairs[i].x])).normalized;
            Vector2 perpendicularDirection = new Vector2(directionBetweenPoints.y, -directionBetweenPoints.x);

            Vector2 thisClosestPoint = GetPointOfIntersection(positionInside, polygonPointsInWorld[validPairs[i].x], perpendicularDirection, directionBetweenPoints);

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
            float thisVertexDistance = (polygonPointsInWorld[i] - positionInside).magnitude;

            if (thisVertexDistance < closestVertexDistance)
            {
                closestVertexDistance = thisVertexDistance;
                closestVertexIntex = i;
            }
        }

        if (closestVertexDistance < closestPerpendiculatDistance)
        {
            closestVector = (polygonPointsInWorld[closestVertexIntex] - positionInside);
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
    bool IsPerpendicular(Vector2 A, Vector2 P1, Vector2 P2) //BASURA DE INTERNET QUE NO VA
    {
        Vector2 segment = P2 - P1;   // The segment direction vector
        Vector2 AP1 = A - P1;        // Vector from P1 to A

        // Project A onto the infinite line defined by P1 and P2
        float t = Vector3.Dot(AP1, segment) / Vector3.Dot(segment, segment);

        // If t is between 0 and 1, the projection lies on the segment
        if (t >= 0f && t <= 1f)
        {
            // Get the projection point on the segment
            Vector2 projection = P1 + t * segment;

            // Check if the vector from A to the projection is perpendicular to the segment
            Vector2 projectionToA = A - projection;
            float dotProduct = Vector3.Dot(projectionToA, segment);

            // If the dot product is near zero, the connection is perpendicular
            return Mathf.Abs(dotProduct) < 1e-6f; // Use a small tolerance to handle floating point inaccuracies
        }

        return false; //
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
        Vector2 directionfromOtherCenter = (insidePositon - otherCenterPos).normalized;

        //Debug.DrawLine(otherCenterPos, insidePositon, Color.magenta);

        Vector2 otherRadiusVector = directionfromOtherCenter * otherCollider.radius * otherCollider.transform.localScale.x;
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
