using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class GreenBoss_Projectile_TypesOfThrow : MonoBehaviour
{
    [SerializeField] GameObject ProjectilePrefab;
    [SerializeField] GameObject EmptyProjectilePrefab;
    [SerializeField] GameObject DestinationPrefab;
    [SerializeField] GameObject BigDestinationPrefab;
    [SerializeField] Transform ThrowingOrigin;

    [Header("Polygon Throw")]
    [SerializeField] int pointsAround;
    [SerializeField] float minimDistance;
    [SerializeField] float delayBetweenThrows_polygon;
    [SerializeField] float maxDistance_polygon;
    [Header("Burst To Player")]
    [SerializeField] int AmountOfThrows;
    [SerializeField] float delayBetweenThrows_burst;
    [SerializeField] float maxDistance_burst;
    [Header("Random burst around player ")]
    [SerializeField] float Radius;
    [SerializeField] float delayBetweenThrows_around;
    [SerializeField] float maxDistance_around;

    
    Vector2 originPosition;
    GameObject player;
    Vector2 playerPosition;
    Vector2 VectorToPlayer;
    float angleToPlayerRad;
    float distanceToPlayer;

    void ThrowProjectile(Vector2 destination)
    {
        GameObject Instantiated_DestinationUI = Instantiate(DestinationPrefab, destination, Quaternion.identity);
        GameObject InstantiatedProjectile = Instantiate(ProjectilePrefab, ThrowingOrigin.position, Quaternion.identity);
        InstantiatedProjectile.GetComponent<GreenBoss_ProjectileLogic>().ThrowItself(Instantiated_DestinationUI, ThrowingOrigin.position, destination);
    }
    void TransitionThrow(Vector2 destination)
    {
        GameObject Instantiated_DestinationUI = Instantiate(BigDestinationPrefab, destination, Quaternion.identity);
        GameObject InstantiatedProjectile = Instantiate(EmptyProjectilePrefab, ThrowingOrigin.position, Quaternion.identity);
        InstantiatedProjectile.GetComponent<GreenBoss_ProjectileLogic>().ThrowItself(Instantiated_DestinationUI, ThrowingOrigin.position, destination);
    }
    private void UpdateVectorData ()
    {
         originPosition = transform.position;
         player = GameObject.FindGameObjectWithTag(TagsCollection.Player_SinglePointCollider);
         playerPosition = player.transform.position;
         VectorToPlayer = originPosition - playerPosition;
         angleToPlayerRad = Mathf.Atan2(VectorToPlayer.y, VectorToPlayer.x);
         distanceToPlayer = VectorToPlayer.magnitude;
    }
    public IEnumerator SinglePolygonThrow()
    {
        UpdateVectorData();
        float OffsetRot = angleToPlayerRad / (Mathf.PI * 2);
        int BasePointAround = pointsAround;
        if (distanceToPlayer < minimDistance) { distanceToPlayer = minimDistance; pointsAround -= 2; }
        else if (distanceToPlayer > minimDistance * 2) pointsAround += 2;
        if (distanceToPlayer > maxDistance_polygon) distanceToPlayer = maxDistance_polygon;

        for (int i = 0; i < pointsAround; i++)
        {
            float divider = (1.0f / pointsAround) * i;
            Vector2 direction = angleToVector((divider + OffsetRot)*(Mathf.PI*2));
            ThrowProjectile(originPosition + (direction * distanceToPlayer));
            yield return new WaitForSeconds(delayBetweenThrows_polygon);
        }
        pointsAround = BasePointAround;
    }
    public IEnumerator DoblePolygonThrow()
    {
        UpdateVectorData();
        float OffsetRot = angleToPlayerRad / (Mathf.PI * 2);
        int BasePointAround = pointsAround;
        if (distanceToPlayer < minimDistance) { distanceToPlayer = minimDistance; pointsAround -= 2; }
        else if (distanceToPlayer > minimDistance * 2) pointsAround += 2;
        if (distanceToPlayer > maxDistance_polygon) distanceToPlayer = maxDistance_polygon;

        for (int i = 0; i < pointsAround; i++)
        {
            float divider = (1.0f / pointsAround) * i;
            Vector2 direction = angleToVector((divider + OffsetRot) * (Mathf.PI * 2));
            ThrowProjectile(originPosition + (direction * distanceToPlayer));
            yield return new WaitForSeconds(delayBetweenThrows_polygon);
        }
        pointsAround = BasePointAround;
        StartCoroutine(SinglePolygonThrow());
    }
    public IEnumerator BurstToPlayer(int throws)
    {
        UpdateVectorData();
        for(int i = 0; i < throws; i++)
        {
            Vector2 playerPositionTemporal = player.transform.position;
            if((originPosition - playerPositionTemporal).magnitude > maxDistance_burst)
            {
                Vector2 playerPositionInLocal = -player.transform.InverseTransformPoint(originPosition);
                Vector2 playerDirectionInLocal = playerPositionInLocal.normalized;
                playerPositionTemporal = originPosition + (playerDirectionInLocal * maxDistance_burst);
            }
            ThrowProjectile(playerPositionTemporal);
            yield return new WaitForSeconds(delayBetweenThrows_burst);
        }
    }
    public IEnumerator BurstAroundPlayer(int throws)
    {
        UpdateVectorData();
        for (int i = 0; i< throws; i++)
        {
            Vector2 playerPositionTemporal = player.transform.position;
            if ((originPosition - playerPositionTemporal).magnitude > maxDistance_around)
            {
                Vector2 playerPositionInLocal = -player.transform.InverseTransformPoint(originPosition);
                Vector2 playerDirectionInLocal = playerPositionInLocal.normalized;
                playerPositionTemporal = originPosition + (playerDirectionInLocal * maxDistance_burst);
            }
            Vector2 randomPositionAroundPlayer = (Random.insideUnitCircle * Radius) + playerPositionTemporal;
            ThrowProjectile(randomPositionAroundPlayer);
            yield return new WaitForSeconds(delayBetweenThrows_around);
        }
    }
    public void TransitionBurst()
    {
        Vector2 origin = gameObject.transform.position;
        TransitionThrow(origin);

    }
    Vector2 angleToVector(float angleRad)
    {
        float x = Mathf.Cos(angleRad);
        float y = Mathf.Sin(angleRad);
        return new Vector2(x, y);
    }
}
