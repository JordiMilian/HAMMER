using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RootMotionTesting : MonoBehaviour
{
    public List<Vector2> MovementVectors = new List<Vector2>(); //Any movement stuff must be added here everyframe
    public float RootMotionMultiplier = 1; //This could be useful to adapt attacks to distance of player

    [SerializeField] Animator animator;

    List<Vector2> collisionDirections = new List<Vector2>(); //For DrawGizmo. Delete at some point
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
        
        collisionDirections.Clear();
        collisionPositions.Clear();

        Vector2 calculatedDirection = Vector2.zero;

        MovementVectors.Add(TestDirectionToMove * speedMultiplier); //For testing delete

        //MOVEMENTS
        foreach(Vector2 movement in MovementVectors)
        {
            calculatedDirection += movement * Time.deltaTime;
        }
        MovementVectors.Clear(); //Not sure if I should clear this here

        //ROOT MOTION
        calculatedDirection += (Vector2)animator.deltaPosition * RootMotionMultiplier;

        //COLISIONS 
        //Afegim la mateix força aplicada en la direccio contraria a la pared
        foreach (var collider in collidersInside)
        {
            Vector2 closestPoint = collider.ClosestPoint(transform.position);
            if( collider.OverlapPoint(transform.position)) //If colision inside
            {

            }
            Vector2 direction = (closestPoint - (Vector2)transform.position).normalized;
            collisionDirections.Add(direction); //This is for draw gizmos. Can be deleted
            collisionPositions.Add(closestPoint);

            float distanceToColisionPoint = (closestPoint - (Vector2)transform.position).magnitude;

            float collisionDepth = ownCollider.radius - distanceToColisionPoint;
            calculatedDirection += (-direction * (calculatedDirection.magnitude+ collisionDepth));
        }

        transform.Translate(calculatedDirection);
        
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
        Gizmos.color = Color.red;
        foreach(Vector2 direction in collisionDirections)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + direction, 0.1f);
        }
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

    Vector2 GetClosestPositionOnEdgeOfPolygon(PolygonCollider2D polygon, Vector2 positionInside)
    {
        Vector2Int closestPoint = Vector2Int.zero;
        List<Vector2Int> validPairs = new List<Vector2Int>();

        for (int i = 0; i < polygon.points.Length; i++)
        {
            if (DoPointsMakeAValidTriangle(positionInside, polygon.points[i+1], polygon.points[i]))
            {
                validPairs.Add(new Vector2Int(i+1, i));
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
}
