using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveAndRotateToTarget : MonoBehaviour
{
    [Header("Targets")]
    public Transform MovementTarget;
    public Transform LookingTarget;
    public void SetTargets(Transform movementTarget, Transform lookingTarget = null)
    {
        MovementTarget = movementTarget;
        if(lookingTarget == null) { LookingTarget = movementTarget; }
        else { LookingTarget = lookingTarget; }
    }
    [Space(2)]
    [SerializeField] Enemy_References enemyRefs;
    Vector2 finalDirection;
    Vector2 DirectionToTarget;
    Vector2 OpositeDirectionToClosest;
    public bool DoMove;
    public bool DoLook;

    [Header("Avoid Other Enemies")]
    [SerializeField] float MaxDistance;
    [SerializeField] float MinDistance;
    [SerializeField] Generic_OnTriggerEnterEvents proximityTrigger;
    public List<Transform> inRangeEnemies = new List<Transform>();
    Transform ClosestEnemy;

    private void OnEnable()
    {
        proximityTrigger.AddActivatorTag(Tags.Enemy_SinglePointCollider);
        proximityTrigger.AddActivatorTag(Tags.IA_Obstacle);
        proximityTrigger.OnTriggerEntered += AddEnemy;
        proximityTrigger.OnTriggerExited += RemoveEnemy;
    }
    private void OnDisable()
    {
        proximityTrigger.OnTriggerEntered -= AddEnemy;
        proximityTrigger.OnTriggerExited -= RemoveEnemy;
    }
    private void Start()
    {
        InvokeRepeating("CheckClosestTransform", 0, 1f);
    }
    void AddEnemy(Collider2D collision)
    {
        if(collision.gameObject.transform == enemyRefs.ownSinglePointCollider) { return; }
        inRangeEnemies.Add(collision.gameObject.transform);
        CheckClosestTransform();
    }
    void RemoveEnemy(Collider2D collision)
    {
        inRangeEnemies.Remove(collision.gameObject.transform);
        CheckClosestTransform();
    }
    #region Set Movement and Rotation Speed (Enum)
    public void SetMovementSpeed(MovementSpeeds speedType)
    {
        switch (speedType)
        {
            case MovementSpeeds.Regular:
                enemyRefs.currentEnemyStats.Speed = enemyRefs.currentEnemyStats.BaseSpeed;
                break;
            case MovementSpeeds.Fast:
                enemyRefs.currentEnemyStats.Speed = enemyRefs.currentEnemyStats.BaseSpeed * 1.5f;
                break;
            case MovementSpeeds.Slow:
                enemyRefs.currentEnemyStats.Speed = enemyRefs.currentEnemyStats.BaseSpeed * 0.5f;
                break;
            case MovementSpeeds.VerySlow:
                enemyRefs.currentEnemyStats.Speed = enemyRefs.currentEnemyStats.BaseSpeed * 0.25f;
                break;
            case MovementSpeeds.Stopped:
                enemyRefs.currentEnemyStats.Speed = 0;
                break;
        }
    }
    public void SetRotatinSpeed(MovementSpeeds speedType)
    {
        switch (speedType)
        {
            case MovementSpeeds.Regular:
                enemyRefs.currentEnemyStats.RotationSpeed = enemyRefs.baseEnemyStats.RotationSpeed;
                break;
            case MovementSpeeds.Fast:
                enemyRefs.currentEnemyStats.RotationSpeed = enemyRefs.baseEnemyStats.RotationSpeed * 1.5f;
                break;
            case MovementSpeeds.Slow:
                enemyRefs.currentEnemyStats.RotationSpeed = enemyRefs.baseEnemyStats.RotationSpeed * 0.5f;
                break;
            case MovementSpeeds.VerySlow:
                enemyRefs.currentEnemyStats.RotationSpeed = enemyRefs.baseEnemyStats.RotationSpeed * 0.25f;
                break;
            case MovementSpeeds.Stopped:
                enemyRefs.currentEnemyStats.RotationSpeed = 0;
                break;
        }
    }
    #endregion
    void CheckClosestTransform()
    {
        if (inRangeEnemies.Count == 0) { return; }

        Transform currentClosest = transform;
        float closestDistance = 100;
        foreach (Transform t in inRangeEnemies)
        {
            float thisDistance = (t.position - transform.position).magnitude;
            if (thisDistance < closestDistance)
            {
                closestDistance = thisDistance;
                currentClosest = t;
            }
        }
        ClosestEnemy = currentClosest;
    }
    #region DoMove DoLook
    private void FixedUpdate()
    {
        if (DoMove)
        {
            Move();
        }
        if (DoLook)
        {
            Look();
        }

        //
        void Move()
        {
            if (MovementTarget == null)
            {
                return;
            }
            if (inRangeEnemies.Count > 0)
            {
                //The size of the other gameObject multiplies the distance influence (bigger = close)
                float otherSizeMultiplier = (ClosestEnemy.transform.localScale.x + ClosestEnemy.transform.localScale.y) / 2;
                finalDirection = CalculateNewDirection(transform.position, ClosestEnemy.position, MovementTarget.position, otherSizeMultiplier);
            }
            else { finalDirection = (MovementTarget.position - transform.position).normalized; }

            enemyRefs.characterMover.MovementVectorsPerSecond.Add(finalDirection * enemyRefs.currentEnemyStats.Speed);
        }
        void Look()
        {
            if (LookingTarget == null) { return; }

            Vector3 TargetPos = LookingTarget.position;
            Vector3 EnemyPos = transform.position;

            Vector3 rotateTowardsVector = Vector3.RotateTowards(enemyRefs.transform.up, TargetPos - EnemyPos, enemyRefs.currentEnemyStats.RotationSpeed * Time.deltaTime, 10);

            Vector3 planeposition = (Vector3.ProjectOnPlane(rotateTowardsVector, Vector3.forward)).normalized;
            enemyRefs.transform.up = planeposition;

            enemyRefs.spriteFliper.FocusVector = TargetPos;
        }
    }
    #endregion
    #region Calculate New Direction

    Vector2 CalculateNewDirection(Vector2 ownPosition, Vector2 closestPosition, Vector2 targetPosition, float closestPositionMultiplier)
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

        //The size of the other gameObject multiplies the distance influence (bigger = close)
        distanceInfluence *= closestPositionMultiplier;

        //Find the angle between and add more or less angle depending on distance. Also multiply by side
        float dotBetween = Vector2.Dot(DirectionToTarget, OpositeDirectionToClosest);
        float angleBetween = Mathf.Acos(dotBetween);

        float finalInfluence = Mathf.Lerp(0, angleBetween, distanceInfluence);
        float finalAngle = (finalInfluence * side) + angleToTarget;

        return Angle2Vector(finalAngle);
    }
    #endregion
   
    #region Static tools
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
    #endregion

    #region Draw Gizmo
    private void OnDrawGizmos()
    {
        if (inRangeEnemies.Count > 0)
        {
            Vector2 tpos = transform.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(tpos, tpos + finalDirection);
        }
    }
    #endregion

    #region (Deprecated) Slow down speeds
    float slowRotationSpeed;
    float slowMovingSpeed;
    private void Awake()
    {
        slowRotationSpeed = enemyRefs.baseEnemyStats.RotationSpeed / 5;
        slowMovingSpeed = enemyRefs.baseEnemyStats.Speed / 4;
    }
    public void EV_SlowRotationSpeed() { enemyRefs.currentEnemyStats.RotationSpeed = slowRotationSpeed; enemyRefs.spriteFliper.canFlip = false; }
    public void EV_ReturnRotationSpeed() { enemyRefs.currentEnemyStats.RotationSpeed = enemyRefs.baseEnemyStats.RotationSpeed; enemyRefs.spriteFliper.canFlip = true; }
    public void EV_SlowMovingSpeed() { enemyRefs.currentEnemyStats.Speed = slowMovingSpeed; }
    public void EV_ReturnMovingSpeed() { enemyRefs.currentEnemyStats.Speed = enemyRefs.currentEnemyStats.BaseSpeed; }
    public void EV_ReturnAllSpeed()
    {
        EV_ReturnMovingSpeed();
        EV_ReturnRotationSpeed();
    }
    #endregion
}
