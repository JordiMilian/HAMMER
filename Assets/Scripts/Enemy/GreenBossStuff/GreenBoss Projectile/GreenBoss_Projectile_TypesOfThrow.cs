using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class GreenBoss_Projectile_TypesOfThrow : Enemy_BaseProjectileCreator
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
    [Header("Tornado polygon")]
    [SerializeField] float minDistanceTornado;
    [SerializeField] float maxDistanceTornado;
    [SerializeField] int firstPolygonSides;
    [SerializeField] int amountOfPolygons;
    [SerializeField] int addedSidesPerPolygon;
    [SerializeField] float delayBetweenThrows_tornado;
    [SerializeField] bool testTornado;
    [SerializeField] float testOffset;
    [Header("SFX")]
    [SerializeField] AudioClip ThrowGreenProjectileSFX;


    private void Update()
    {
        if(testTornado)
        {
            StartCoroutine( EV_TornadoPolygon(testOffset));
            testTornado = false;
        }
    }
    void ThrowProjectile(Vector2 destination)
    {
        GameObject Instantiated_DestinationUI = Instantiate(DestinationPrefab, destination, Quaternion.identity);
        GameObject InstantiatedProjectile = Instantiate(ProjectilePrefab, ThrowingOrigin.position, Quaternion.identity);
        InstantiatedProjectile.GetComponent<GreenBoss_ProjectileLogic>().ThrowItself(Instantiated_DestinationUI, ThrowingOrigin.position, destination);

        SFX_PlayerSingleton.Instance.playSFX(ThrowGreenProjectileSFX, 0.1f);

    }
    void TransitionThrow(Vector2 destination)
    {
        GameObject Instantiated_DestinationUI = Instantiate(BigDestinationPrefab, destination, Quaternion.identity);
        GameObject InstantiatedProjectile = Instantiate(EmptyProjectilePrefab, ThrowingOrigin.position, Quaternion.identity);
        InstantiatedProjectile.GetComponent<GreenBoss_ProjectileLogic>().ThrowItself(Instantiated_DestinationUI, ThrowingOrigin.position, destination);
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
    public IEnumerator EV_TornadoPolygon(float offset)
    {
        UpdateVectorData();
        int thisPolygonSides = firstPolygonSides;
        float addedDistancePerPolygon = (maxDistanceTornado - minDistanceTornado) / amountOfPolygons;
        for (int i = 0; i < amountOfPolygons; i++)
        {
            Vector2[] thisPolygonVectors = UsefullMethods.GetPolygonPositions(
                originPosition, 
                thisPolygonSides, 
                minDistanceTornado + (addedDistancePerPolygon * i),
                offset);
            for (int u = 0; u < thisPolygonVectors.Length; u++)
            {
                ThrowProjectile(thisPolygonVectors[u]);
                yield return new WaitForSeconds(delayBetweenThrows_tornado);
            }
            thisPolygonSides += addedSidesPerPolygon;
        }
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
