using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsManager : MonoBehaviour
{
    List<CharacterMover2> CharacterMoversList = new List<CharacterMover2>();
    List<WallCollider> WallsList = new List<WallCollider>();

    public void AddCharacterMover(CharacterMover2 mover2)
    {
        if (CharacterMoversList.Contains(mover2)) { return; }

        CharacterMoversList.Add(mover2);
    }

    public static CollisionsManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate()
    {
        //Circles collisions
        int colisionChecks = 0;
        for (int c = 0; c < CharacterMoversList.Count -1; c++)
        {
            CharacterMover2 characterA = CharacterMoversList[c];
            for(int d = c+1; d < CharacterMoversList.Count; d++)
            {
                CharacterMover2 characterB = CharacterMoversList[d];
                if (CharactersCollision(characterA, characterB, out float depth, out Vector2 directionAtoB))
                {
                    Vector2 characterAForce = -directionAtoB * (depth / 2);
                    Vector2 characterBForce = directionAtoB * (depth / 2);
                    characterA.currentVelocity += characterAForce;
                    characterB.currentVelocity += characterBForce;
                }
                colisionChecks++;
            }
        }
        Debug.Log( "Total colisions: "+CharacterMoversList.Count +  " - Collision checks:" + colisionChecks);

        for (int c = 0;c < WallsList.Count;c++)
        {
            for (int w = 0;w <WallsList.Count;w++) 
            {
                for (int l = 0; l< WallsList[w].polygonCollider.points.Length;l++)
                {

                }
            }
        }
        for (int c = 0; c < CharacterMoversList.Count; c++)
        {
            CharacterMoversList[c].transform.position += (Vector3)CharacterMoversList[c].currentVelocity;
        }
    }
    public static bool CharactersCollision(CharacterMover2 charaA, CharacterMover2 charaB, out float depth, out Vector2 directionAtoB)
    {
        directionAtoB = Vector2.zero;
        depth = 0;
        Vector2 futureAPos = (Vector2)charaA.transform.position + charaA.currentVelocity;
        Vector2 futureBPos = (Vector2)charaB.transform.position + charaB.currentVelocity;

        float distance = (futureAPos - futureBPos).magnitude;
        float combinedRadius = charaA.circleCollider.radius + charaB.circleCollider.radius;

        if(distance > combinedRadius) { return false; }

        depth = combinedRadius - distance;
        directionAtoB = (futureBPos - futureAPos).normalized;
        return true;
    }
}
