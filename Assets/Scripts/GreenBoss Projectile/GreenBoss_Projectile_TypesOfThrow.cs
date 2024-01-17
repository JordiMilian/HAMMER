using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GreenBoss_Projectile_TypesOfThrow : MonoBehaviour
{
    [SerializeField] GreenBoss_ProjectileThrower thrower;
    [SerializeField] Player_Movement playerMovement;
    

    [SerializeField] int pointsAround;
    [SerializeField] float minimDistance;
    [SerializeField] float microDelayBetweenBalls;
    private void Start()
    {
        InvokeRepeating("callCoroutineTest", 0,5);
    }
    void callCoroutineTest()
    {
        StartCoroutine(DoblePolygonThrow());
    }
    IEnumerator SinglePolygonThrow()
    {
        Vector2 originPosition = transform.position;
        
        Vector2 playerPosition = GameObject.FindGameObjectWithTag(TagsCollection.instance.Player_SinglePointCollider).transform.position;
        Vector2 VectorToPlayer = originPosition - playerPosition;
        float angleToPlayer = Mathf.Atan2(VectorToPlayer.y, VectorToPlayer.x);
        float Offset = angleToPlayer / (Mathf.PI * 2);

        float distanceToPlayer = VectorToPlayer.magnitude;
        if (distanceToPlayer < minimDistance) distanceToPlayer = minimDistance;

        for (int i = 0; i < pointsAround; i++)
        {
            float divider = (1.0f / pointsAround) * i;
            Vector2 direction = angleToVector((divider + Offset)*(Mathf.PI*2));
            thrower.GreenBoss_ThrowProjectile(originPosition + (direction * distanceToPlayer));
            yield return new WaitForSeconds(microDelayBetweenBalls);
        }  
    }
    IEnumerator DoblePolygonThrow()
    {
        GameObject player = GameObject.FindGameObjectWithTag(TagsCollection.instance.Player_SinglePointCollider);
        Vector2 originPosition = transform.position;

        Vector2 playerPosition = player.transform.position;
        Vector2 VectorToPlayer = originPosition - playerPosition;
        float angleToPlayer = Mathf.Atan2(VectorToPlayer.y, VectorToPlayer.x);
        float Offset = angleToPlayer / (Mathf.PI * 2);

        float distanceToPlayer = VectorToPlayer.magnitude;
        if (distanceToPlayer < minimDistance) distanceToPlayer = minimDistance;


        for (int i = 0; i < pointsAround; i++)
        {
            float divider = (1.0f / pointsAround) * i;
            Vector2 direction = angleToVector((divider + Offset) * (Mathf.PI * 2));
            thrower.GreenBoss_ThrowProjectile(originPosition + (direction * distanceToPlayer));
            yield return new WaitForSeconds(microDelayBetweenBalls);
            if (i == pointsAround-1) StartCoroutine(SinglePolygonThrow());
        }
    }
    Vector2 angleToVector(float angleRad)
    {
        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);
        return new Vector2(x, y);
    }
}
