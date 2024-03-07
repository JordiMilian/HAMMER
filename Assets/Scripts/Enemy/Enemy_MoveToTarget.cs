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
    List<Transform> inRangeEnemies = new List<Transform>();
    Transform ClosestEnemy;

    private void OnEnable()
    {
        proximityTrigger.AddActivatorTag(TagsCollection.Instance.Enemy);
        proximityTrigger.OnTriggerEntered += AddEnemy;
        proximityTrigger.OnTriggerExited += RemoveEnemy;
    }
    private void Start()
    {
        InvokeRepeating("CheckClosestTransform", 0, 0.25f);
    }
    void AddEnemy(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo collisionInfo)
    {
        inRangeEnemies.Add(collisionInfo.Collision.gameObject.transform);
        CheckClosestTransform();
    }
    void RemoveEnemy(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo collisionInfo)
    {
        inRangeEnemies.Remove(collisionInfo.Collision.gameObject.transform);
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

    private void Update()
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
            _rigidBody.AddForce( finalDirection * Velocity * Time.deltaTime);
        }
    }

    Vector2 CalculateNewDirection(Vector2 ownPosition, Vector2 closestPosition, Vector2 targetPosition)
    {
        //Find the direction to Target and the oposite direction to the enemy
        DirectionToTarget = (targetPosition - ownPosition).normalized;
        OpositeDirectionToClosest = (ownPosition - closestPosition).normalized;

        //Get the angles of both directions
        float angleToTarget = Vector2Angle(DirectionToTarget);
        float opositeAngleToClosest = Vector2Angle(OpositeDirectionToClosest);

        //Find how close is the enemy and find the relation to 1 with min/max
        float distanceToClosest = (closestPosition - ownPosition).magnitude;
        float inverseLerpedDistance = Mathf.InverseLerp(MaxDistance, MinDistance, distanceToClosest);

        //Lerp the angle transform to vector2
        float lerpedAngle = Mathf.Lerp(angleToTarget, opositeAngleToClosest, inverseLerpedDistance);
        return Angle2Vector(lerpedAngle);
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
}
