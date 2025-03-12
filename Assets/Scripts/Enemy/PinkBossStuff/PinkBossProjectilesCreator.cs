using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PinkBossProjectilesCreator : Enemy_BaseProjectileCreator
{
    [SerializeField] Enemy_EventSystem events;

    [SerializeField] GameObject PinkProjectile_General;
    [SerializeField] GameObject PinkProjectile_General_Big;
    [SerializeField] GameObject PinkProjectile_Directional;
    [SerializeField] GameObject PinkProjectile_StarDirectional;
    [Header("Burst around player")]
    [SerializeField] float maxDistance_around;
    [SerializeField] float delayBetweenThrows_around;
    [SerializeField] float Radius;
    
    [Header("PinkSaw_polygon")]
    [SerializeField] GameObject PinkSawProjectile_Prefab;
    [SerializeField] float delayBetweenSaws_polygon;
    [SerializeField] int amountOfSaws_polygon;
    [SerializeField] int sfxEach = 3;
    [Header("PinkSaw_spread")]
    [SerializeField] float spreadAngleDeg;
    [SerializeField] float delayBetweenSaws_spread;
    [Header("SFX")]
    [SerializeField] AudioClip ThrowSingleSawSFX;


    void ThrowGeneralProjectile(Vector3 Position)
    {
        GameObject newGeneralProjectile = Instantiate(PinkProjectile_General,Position, Quaternion.identity);
    }
    void ThrowDirectionalProjectile(GameObject Prefab, Vector3 Direction, bool isInstant = false)
    {
        GameObject newDirectionalProjectile = Instantiate(Prefab, transform.position, Quaternion.identity);
        GameObject DirectionChild = newDirectionalProjectile.transform.GetChild(0).gameObject;
        float angleDegToDirection = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        DirectionChild.transform.eulerAngles = new Vector3(0, 0, angleDegToDirection + 90);
        if (isInstant) { newDirectionalProjectile.GetComponent<Animator>().SetTrigger("InstantDrop"); }
    }

    public IEnumerator EV_BurstAroundPlayer(int throws)
    {
        UpdateVectorData();
        for (int i = 0; i < throws; i++)
        {
            Vector2 playerPositionTemporal = player.transform.position;
            if ((originPosition - playerPositionTemporal).magnitude > maxDistance_around)
            {
                Vector2 playerPositionInLocal = -player.transform.InverseTransformPoint(originPosition);
                Vector2 playerDirectionInLocal = playerPositionInLocal.normalized;
                playerPositionTemporal = originPosition + (playerDirectionInLocal * maxDistance_around);
            }
            Vector2 randomPositionAroundPlayer = (Random.insideUnitCircle * Radius) + playerPositionTemporal;
            ThrowGeneralProjectile(randomPositionAroundPlayer);
            yield return new WaitForSeconds(delayBetweenThrows_around);
        }
    }
    public void EV_DirectionalAttack()
    {
        UpdateVectorData();
        ThrowDirectionalProjectile(PinkProjectile_Directional, weaponDirection);
    }
    public void EV_DirectionalStarAttack()
    {
        UpdateVectorData();
        ThrowDirectionalProjectile(PinkProjectile_StarDirectional, weaponDirection);
    }
    public void EV_GeneralFromCenterOfEnemy()
    {
        UpdateVectorData();
        ThrowDirectionalProjectile(PinkProjectile_General,transform.position,true);
       
    }
    public void EV_GeneralFromCenter_Big()
    {
        UpdateVectorData();
        ThrowDirectionalProjectile(PinkProjectile_General_Big, transform.position, true);
    }

    public IEnumerator EV_MultipleSawProjectiles(float Offset)
    {
        UpdateVectorData();
        
        int sawsCount = 0;

        Vector2[] sawDirections = UsefullMethods.GetPolygonPositions(Vector2.zero, amountOfSaws_polygon, 1, Offset);
        for (int i = 0; i < amountOfSaws_polygon; i++)
        {
            GameObject newSaw = Instantiate(PinkSawProjectile_Prefab, originPosition, Quaternion.identity);
            PinkSaw_Projectile projectile = newSaw.GetComponent<PinkSaw_Projectile>();
            projectile.startSawing(originPosition, sawDirections[i]);

            sawsCount++;
            if(sawsCount == sfxEach) { sawsCount = 0; SFX_PlayerSingleton.Instance.playSFX(ThrowSingleSawSFX, 0.2f); }
            yield return new WaitForSeconds(delayBetweenSaws_polygon);
        }
    }
    public IEnumerator EV_SawProjectile_Spread(int amountOfSaws)
    {
        UpdateVectorData();
        SFX_PlayerSingleton.Instance.playSFX(ThrowSingleSawSFX, 0.2f);
        float spreadAngleRad = spreadAngleDeg * Mathf.Deg2Rad;

        Vector2[] spreadDirections = UsefullMethods.GetSpreadDirectionsFromCenter(-weaponDirection, amountOfSaws, spreadAngleRad);

        for (int i = 0; i < amountOfSaws; i++)
        {
            GameObject newSaw = Instantiate(PinkSawProjectile_Prefab, originPosition, Quaternion.identity);
            PinkSaw_Projectile projectile = newSaw.GetComponent<PinkSaw_Projectile>();
            projectile.startSawing(originPosition, spreadDirections[i]);

            yield return new WaitForSeconds(delayBetweenSaws_spread);
        }
    }
    public void EV_PinkSawProjectile()
    {
        UpdateVectorData();
        GameObject newSaw = Instantiate(PinkSawProjectile_Prefab, originPosition, Quaternion.identity);
        PinkSaw_Projectile projectile = newSaw.GetComponent<PinkSaw_Projectile>();
        projectile.startSawing(originPosition, -directionToPlayer);
    }

}
