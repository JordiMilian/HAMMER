using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_MoveAwayFromEachotherTest : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform Enemy02;

    public float MaxDistance;
    public float MinDistance;

    Vector2 EnemyPosition;
    Vector2 EnemyPosition2;
    Vector2 PlayerPosition;
    Vector2 DirectionToPlayer;
    private void OnDrawGizmos()
    {
        //Get the positions of everyone
        EnemyPosition = transform.position;
        EnemyPosition2 = Enemy02.position;
        PlayerPosition = Player.position;

        //Find direction to player
        DirectionToPlayer = (PlayerPosition - EnemyPosition).normalized;
        Gizmos.DrawLine(EnemyPosition, EnemyPosition + DirectionToPlayer);

        //Find direction to enemy but oposite
        Vector2 OpositeDirectionToEnemy02 = (EnemyPosition - EnemyPosition2).normalized;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(EnemyPosition,EnemyPosition + OpositeDirectionToEnemy02);

        //Get the angles of both these vectors
        float angleToPlayerRad = Vector2Angle(DirectionToPlayer);
        float opositeAngleToEnemy = Vector2Angle(OpositeDirectionToEnemy02);

        //Find how close is the other enemy
        float DistanceToEnemy = (EnemyPosition2 - EnemyPosition).magnitude;
        float inverseLerpedDistance = Mathf.InverseLerp(MaxDistance,MinDistance, DistanceToEnemy);

        //Lerp the angles
        float lerpedAngle = Mathf.Lerp(angleToPlayerRad, opositeAngleToEnemy, inverseLerpedDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(EnemyPosition, EnemyPosition + Angle2Vector( lerpedAngle));
    }

    float Vector2Angle(Vector2 vector)
    {
        return Mathf.Atan2(vector.y,vector.x);
    }
    Vector2 Angle2Vector (float angleRad)
    {
        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);
        return new Vector2(x,y);
    }
}
