using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveToTarget : MonoBehaviour
{
    public Transform Target;
    [SerializeField] Rigidbody2D _rigidBody;
    public float Velocity;
    Vector2 finalDirection;
    Vector2 DirectionToTarget;
    Vector2 OpositeDirectionToClosest;
    public bool DoMove;

    [Header("Avoid Other Enemies")]
    [SerializeField] float MaxDistance;
    [SerializeField] float MinDistance;
    [SerializeField] Generic_OnTriggerEnterEvents proximityTrigger;
    public List<Transform> inRangeEnemies = new List<Transform>();
    Transform ClosestEnemy;

    private void OnEnable()
    {
        proximityTrigger.AddActivatorTag(TagsCollection.Enemy);
        proximityTrigger.AddActivatorTag(TagsCollection.IA_Obstacle);
        proximityTrigger.OnTriggerEntered += AddEnemy;
        proximityTrigger.OnTriggerExited += RemoveEnemy;
    }
    private void Start()
    {
        InvokeRepeating("CheckClosestTransform", 0, 1f);
    }
    void AddEnemy(Collider2D collision)
    {
        inRangeEnemies.Add(collision.gameObject.transform);
        CheckClosestTransform();
    }
    void RemoveEnemy(Collider2D collision)
    {
        inRangeEnemies.Remove(collision.gameObject.transform);
        CheckClosestTransform();
    }

    void CheckClosestTransform()
    {
        if(inRangeEnemies.Count == 0) { return; }

        Transform currentClosest = transform;
        float closestDistance = 100;
        foreach(Transform t in inRangeEnemies)
        {
            float thisDistance = (t.position - transform.position).magnitude;
            if ( thisDistance < closestDistance)
            {
                closestDistance  = thisDistance;
                currentClosest = t;
            }
        }
        ClosestEnemy = currentClosest;
    }

    private void FixedUpdate()
    {
        if(DoMove)
        {
            if (Target == null)
            {
                Debug.Log(gameObject.name + " has no target");
                return;
            }
            if(inRangeEnemies.Count > 0)
            {
                finalDirection = CalculateNewDirection(transform.position, ClosestEnemy.position, Target.position);
            }
            else { finalDirection = (Target.position - transform.position).normalized; }

            //Add the force
            _rigidBody.AddForce( finalDirection * Velocity * Time.fixedDeltaTime);
        }
    }

    Vector2 CalculateNewDirection(Vector2 ownPosition, Vector2 closestPosition, Vector2 targetPosition)
    {
        //Find the direction to Target and the oposite direction to the enemy
        DirectionToTarget = (targetPosition - ownPosition).normalized;
        OpositeDirectionToClosest = (ownPosition - closestPosition).normalized;
        Debug.DrawLine(ownPosition, ownPosition + OpositeDirectionToClosest, new Color(1, 0, 0, 0.3f));
        Debug.DrawLine(ownPosition, ownPosition + DirectionToTarget, new Color(0, 1, 0, 0.3f));

        //Check which side is the Target respecto al Enemigo. First make perpendicular to enemy
        float perpendicularAngle = Vector2Angle(OpositeDirectionToClosest) + (0.25f * Mathf.PI + 2);
        Vector2 perpendicularVector = Angle2Vector(perpendicularAngle);
        //Dot that to find out side
        float SideDot = Vector2.Dot(perpendicularVector, DirectionToTarget);
        int side = CheckSide(SideDot);

        //Get the angles of both directions
        float angleToTarget = Vector2Angle(DirectionToTarget);
        float opositeAngleToClosest = Vector2Angle(OpositeDirectionToClosest);

        //Find how close is the enemy and find the relation to 1 with min/max
        float distanceToClosest = (closestPosition - ownPosition).magnitude;
        float distanceInfluence = Mathf.InverseLerp(MaxDistance, MinDistance, distanceToClosest);

        //Find the angle between and add more or less angle depending on distance. Also multiply by side
        float dotBetween = Vector2.Dot(DirectionToTarget, OpositeDirectionToClosest);
        float angleBetween = Mathf.Acos(dotBetween);

        float finalInfluence = Mathf.Lerp(0, angleBetween, distanceInfluence);
        float finalAngle = (finalInfluence * side) + angleToTarget;

        return Angle2Vector(finalAngle);
    }
    private void OnDrawGizmos()
    {
        if(inRangeEnemies.Count > 0)
        {
            Vector2 tpos = transform.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(tpos,tpos + finalDirection);
        }
    }
    float Vector2Angle(Vector2 vector)
    {
        return Mathf.Atan2(vector.y, vector.x);
    }
    Vector2 Angle2Vector(float angleRad)
    {
        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);
        return new Vector2(x, y);
    }
    int CheckSide(float Dot)
    {
        if (Dot >= 0) return -1;
        else { return 1; }
    }
}
