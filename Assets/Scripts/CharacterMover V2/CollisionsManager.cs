using Cinemachine.Utility;
using Pathfinding.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public enum CollisionLayers
{
    AllCollision, Jumpable, JumpingPlayer
}
public class CollisionsManager : MonoBehaviour
{
    List<CharacterMover2> CharacterMoversList = new List<CharacterMover2>();
    List<RoomCollider> RoomsList = new List<RoomCollider>();

    #region Singleton Logic
    public static CollisionsManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    #endregion
    #region Add and Remove Entities
    public void AddCharacterMover(CharacterMover2 mover2)
    {
        if (CharacterMoversList.Contains(mover2)) { return; }

        CharacterMoversList.Add(mover2);
    }
    public void RemoveCharacterMover(CharacterMover2 mover2)
    {
        CharacterMoversList.Remove(mover2);
    }
    public void AddRoomCollider(RoomCollider col)
    {
        if(RoomsList.Contains(col)) { return; }
        RoomsList.Add(col);
    }
    public void RemoveRoomCollider(RoomCollider col)
    {
        RoomsList.Remove(col);
    }
    #endregion

    //This is everything
    private void LateUpdate()
    {
        Handle_CharactersCollisions();

        Handle_WallsCollisions();

        MoveCharacters();
    }
    void Handle_CharactersCollisions()
    {
        //Go throw every posible combination of characters and check if they overlap
        //If they do, add velocity to separate them
        int colisionChecks = 0;
        for (int c = 0; c < CharacterMoversList.Count - 1; c++)
        {
            CharacterMover2 characterA = CharacterMoversList[c];
            if (characterA.ignoreCollisions) { continue; }
            for (int d = c + 1; d < CharacterMoversList.Count; d++)
            {
                CharacterMover2 characterB = CharacterMoversList[d];
                if (characterB.ignoreCollisions) { continue; }
                if (doCharactersCollide(characterA, characterB, out float depth, out Vector2 directionAtoB))
                {
                    normalizeResistanceValues(characterA.InvertedResistance, characterB.InvertedResistance, out float normalizedA, out float normalizedB);

                    Vector2 characterAForce = -directionAtoB * (depth * normalizedA);
                    Vector2 characterBForce = directionAtoB * (depth * normalizedB);
                    characterA.currentVelocity += characterAForce;
                    characterB.currentVelocity += characterBForce;
                }
                colisionChecks++;
            }
        }
    }
    static bool doCharactersCollide(CharacterMover2 charaA, CharacterMover2 charaB, out float depth, out Vector2 directionAtoB)
    {
        directionAtoB = Vector2.zero;
        depth = 0;
        Vector2 futureAPos = (Vector2)charaA.transform.position + charaA.currentVelocity;
        Vector2 futureBPos = (Vector2)charaB.transform.position + charaB.currentVelocity;

        float distance = (futureAPos - futureBPos).magnitude;
        float combinedRadius = charaA.circleCollider.radius + charaB.circleCollider.radius;

        if (distance > combinedRadius) { return false; }

        depth = combinedRadius - distance;
        directionAtoB = (futureBPos - futureAPos).normalized;
        return true;
    }
    static void normalizeResistanceValues(float valueA, float valueB, out float normalizedA, out float normalizedB)
    {
        float sum = valueA + valueB;

        if (Mathf.Approximately(sum,0)) { normalizedA = 0; normalizedB = 0; return; } //If they are both 0, nobody moves?

        normalizedA = valueA / sum;
        normalizedB = valueB / sum;
    }
    void Handle_WallsCollisions()
    {
        //For every character, go throw all the walls.
        //First check if they are perpendicular to that wall using DOT
        //If they are check if they are too close to that wall
        //If they are, add velocity to separate from wall

        //Segurament podries estar mes limpio i ordenat en una funcio que se digue "doCharacterAndWallCollide", pero crec que la performance es delicada,
        //asi que no tocare per evitar fer variables innecesaries

        for (int r = 0; r < RoomsList.Count; r++)
        {
            RoomCollider room = RoomsList[r];

            for (int c = 0; c < CharacterMoversList.Count; c++)
            {
                CharacterMover2 character = CharacterMoversList[c];


                if (ShouldCharacterColliderWithRoom(character, room) == false) { continue; } //If there is no need to calculate the collision with this chara, continue

                Vector2 futureCharaPos = (Vector2)character.transform.position + character.currentVelocity;

                for (int w = 0; w < room.wallInfosList.Count; w++)
                {
                    wallInfo wall = room.wallInfosList[w];
                    if (room.IgnoreCollisionsIndexes.Contains(w)) { continue; }

                    //find closest point to wall
                    Vector2 diferencePos1ToChara = futureCharaPos - wall.Pos1;
                    float rawDotProduct = Vector2.Dot(diferencePos1ToChara, wall.DiferenceVector1to2.normalized);
                    float normalizedDot = rawDotProduct / wall.Lenght;
                    if (normalizedDot < 0 - character.circleCollider.radius / wall.Lenght || normalizedDot > 1 + character.circleCollider.radius / wall.Lenght) { continue; }
                    normalizedDot = Mathf.Clamp01(normalizedDot);

                    //If player is perpendicular to line
                    Vector2 closestPoint = wall.Pos1 + wall.DiferenceVector1to2 * normalizedDot;
                    Vector2 VectorToPlayer = futureCharaPos - closestPoint;
                    float distanceToPlayer = VectorToPlayer.magnitude;
                    if (distanceToPlayer > character.circleCollider.radius) { continue; }

                    //If player is too close to line
                    float depth = character.circleCollider.radius - distanceToPlayer;
                    float dotplayerDirection = Vector2.Dot(VectorToPlayer, wall.Normal);

                    if (dotplayerDirection > 0) { character.currentVelocity += wall.Normal * depth; }//its outside
                    else { character.currentVelocity += -wall.Normal * depth; } //is inside

                }
            }
        }
        //
        static bool ShouldCharacterColliderWithRoom(CharacterMover2 character, RoomCollider room)
        {
            if (character.ignoreCollisions) { return false; }
            if (room.collisionLayer == CollisionLayers.Jumpable && character.collisionLayer == CollisionLayers.JumpingPlayer) { return false; } //If its jumping

            bool isInsideCollider = room.polygonCollider.OverlapPoint(character.transform.position);
            if (isInsideCollider && room.ignoreInnerWalls) { return false; }
            if (!isInsideCollider && room.ignoreOuterWalls) { return false; }

            return true;
        }
    }
     static bool doCharacterAndWallCollide(wallInfo wall, CharacterMover2 character, out float depth, out Vector2 VectorWallToPlayer)
    {
        depth = 0;
        VectorWallToPlayer = Vector2.zero;
        return false;
        //Should be done some day
    }
    void MoveCharacters()
    {
        for (int c = 0; c < CharacterMoversList.Count; c++)
        {
            if (CharacterMoversList[c].currentVelocity.IsNaN()) { return; } //Cutrisim
            CharacterMoversList[c].transform.position += (Vector3)CharacterMoversList[c].currentVelocity;
        }
    }
    private void OnDrawGizmos()
    {
        for (int r = 0; r < RoomsList.Count; r++)
        {
            RoomCollider room = RoomsList[r];
            //Draw walls
            for (int wl = 0; wl < room.wallInfosList.Count; wl++)
            {
               
                wallInfo wall = room.wallInfosList[wl];
                if (room.IgnoreCollisionsIndexes.Contains(wl))
                {
                    Gizmos.color = Color.red;
                }
                else { Gizmos.color = Color.green; }
                   
                Gizmos.DrawLine(wall.Pos1, wall.Pos2);
                //Draw normals
                Vector2 centerPos = wall.Pos1 + (wall.DiferenceVector1to2 / 2);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(centerPos, centerPos + wall.Normal);
            }
        }
    }
}
