using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GreenBoss_Projectile_TypesOfThrow : MonoBehaviour
{
    [SerializeField] GreenBoss_ProjectileThrower thrower;
    [SerializeField] Player_Movement playerMovement;

    [Header("Polygon Throw")]
    [SerializeField] int pointsAround;
    [SerializeField] float minimDistance;
    [SerializeField] float delayBetweenThrows_polygon;
    [Header("Burst To Player")]
    [SerializeField] int AmountOfThrows;
    [SerializeField] float delayBetweenThrows_burst;

    
    Vector2 originPosition;
    GameObject player;
    Vector2 playerPosition;
    Vector2 VectorToPlayer;
    float angleToPlayerRad;
    float distanceToPlayer;

    void UpdateVectorData ()
    {
         originPosition = transform.position;
         player = GameObject.FindGameObjectWithTag(TagsCollection.instance.Player_SinglePointCollider);
         playerPosition = player.transform.position;
         VectorToPlayer = originPosition - playerPosition;
         angleToPlayerRad = Mathf.Atan2(VectorToPlayer.y, VectorToPlayer.x);
         distanceToPlayer = VectorToPlayer.magnitude;
    }
    private void Start()
    {
        InvokeRepeating("callCoroutineTest", 0,5);
    }
    void callCoroutineTest()
    {
        StartCoroutine(BurstToPlayer());
    }
    IEnumerator SinglePolygonThrow()
    {
        UpdateVectorData();
        float OffsetRot = angleToPlayerRad / (Mathf.PI * 2);
        if (distanceToPlayer < minimDistance) distanceToPlayer = minimDistance;

        for (int i = 0; i < pointsAround; i++)
        {
            float divider = (1.0f / pointsAround) * i;
            Vector2 direction = angleToVector((divider + OffsetRot)*(Mathf.PI*2));
            thrower.GreenBoss_ThrowProjectile(originPosition + (direction * distanceToPlayer));
            yield return new WaitForSeconds(delayBetweenThrows_polygon);
        }  
    }
    IEnumerator DoblePolygonThrow()
    {
        UpdateVectorData();
        float OffsetRot = angleToPlayerRad / (Mathf.PI * 2);
        if (distanceToPlayer < minimDistance) distanceToPlayer = minimDistance;

        for (int i = 0; i < pointsAround; i++)
        {
            float divider = (1.0f / pointsAround) * i;
            Vector2 direction = angleToVector((divider + OffsetRot) * (Mathf.PI * 2));
            thrower.GreenBoss_ThrowProjectile(originPosition + (direction * distanceToPlayer));
            yield return new WaitForSeconds(delayBetweenThrows_polygon);
        }
        StartCoroutine(SinglePolygonThrow());
    }
    IEnumerator BurstToPlayer()
    {
        UpdateVectorData();
        for(int i = 0; i < AmountOfThrows; i++)
        {
            Vector2 playerPositionTemporal = player.transform.position;
            thrower.GreenBoss_ThrowProjectile(playerPositionTemporal);
            yield return new WaitForSeconds(delayBetweenThrows_burst);
        }
    }
    Vector2 angleToVector(float angleRad)
    {
        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);
        return new Vector2(x, y);
    }
}
