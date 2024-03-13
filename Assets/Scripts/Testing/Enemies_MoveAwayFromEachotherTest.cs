using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies_MoveAwayFromEachotherTest : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform Enemy02;

    public float MaxDistance;
    public float MinDistance;

    [Header("READ ONLY")]
    //public float AngleToPlayer;
    //public float AngleToEnemy;
    public float DotBetween;
    public float ModifiedDot;
    public float AngleBetween;

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


        //Find direction to enemy but oposite
        Vector2 OpositeDirectionToEnemy02 = (EnemyPosition - EnemyPosition2).normalized;

        //Check Which side is the player
        float angleOfPerpendicular = Vector2Angle(OpositeDirectionToEnemy02) + (0.25f * Mathf.PI * 2);
        Vector2 PerpendicularToEnemy = Angle2Vector(angleOfPerpendicular); //Perpendicular vector 

        float WhichSideDot = Vector2.Dot(PerpendicularToEnemy, DirectionToPlayer); //Dot between perpendicular and player direction. Positie or negative depending
        int side = CheckSide(WhichSideDot);

        //More ifluence if player and enemy are perpendicular, less influence if paralel  
        DotBetween = Vector2.Dot(DirectionToPlayer, OpositeDirectionToEnemy02);
        float AbsoluteDot = Mathf.Abs(DotBetween);
        ModifiedDot = Mathf.InverseLerp(1, 0, AbsoluteDot);
        
        //Get the angles of both vectors
        float angleToPlayerRad = Vector2Angle(DirectionToPlayer);
        //float opositeAngleToEnemy = Vector2Angle(OpositeDirectionToEnemy02);


        //Find how close is the other enemy
        float DistanceToEnemy = (EnemyPosition2 - EnemyPosition).magnitude;
        float inverseLerpedDistance = Mathf.InverseLerp(MaxDistance,MinDistance, DistanceToEnemy);

        //Check the angle between and add more or less depending on distance
        AngleBetween = Mathf.Acos(DotBetween);
        float influenceRad = Mathf.Lerp(0, AngleBetween, inverseLerpedDistance); 
        float AddedAngle = (influenceRad * side) + angleToPlayerRad;
        Vector2 finalVector = Angle2Vector(AddedAngle);



        //Extras
        Color playerColor = new Color(0, 1, 0, 0.6f);
        Gizmos.color = playerColor;
        Gizmos.DrawLine(EnemyPosition, EnemyPosition + (DirectionToPlayer * 0.5f));

        Color otherColor = new Color(1,0,0,0.6f);
        Gizmos.color = otherColor;
        Gizmos.DrawLine(EnemyPosition, EnemyPosition + (OpositeDirectionToEnemy02 * 0.5f));

        Gizmos.DrawLine(EnemyPosition, EnemyPosition + (PerpendicularToEnemy * 0.3f));

        Gizmos.color = Color.Lerp(playerColor,otherColor,inverseLerpedDistance);
        Gizmos.DrawLine(EnemyPosition, EnemyPosition + finalVector); //FINAL DRAWLINE
    }
    int CheckSide(float Dot)
    {
        if (Dot >= 0) return -1;
        else { return 1; }
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
