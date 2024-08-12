using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PinkBossProjectilesCreator : MonoBehaviour
{
    [SerializeField] GameObject PinkProjectile_General;
    [SerializeField] GameObject PinkProjectile_General_Big;
    [SerializeField] GameObject PinkProjectile_Directional;
    [SerializeField] GameObject PinkProjectile_StarDirectional;
    [Header("Burst around player")]
    [SerializeField] float maxDistance_around;
    [SerializeField] float delayBetweenThrows_around;
    [SerializeField] float Radius;
    [SerializeField] GameObject WeaponFollowPlayer;
    
    [Header("PinkSaw_polygon")]
    [SerializeField] GameObject PinkSawProjectile_Prefab;
    [SerializeField] int amountOfSaws;
    [SerializeField] float delayBetweenSaws;

    Vector2 originPosition;
    GameObject player;
    Vector2 playerPosition;
    Vector2 VectorToPlayer;
    float angleToPlayerRad;
    float distanceToPlayer;
    Vector2 directionToPlayer;
    Vector2 weaponDirection;

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
    private void UpdateVectorData()
    {
        originPosition = transform.position;
        player = GameObject.FindGameObjectWithTag(TagsCollection.Player_SinglePointCollider);
        playerPosition = player.transform.position;
        VectorToPlayer = originPosition - playerPosition;
        angleToPlayerRad = Mathf.Atan2(VectorToPlayer.y, VectorToPlayer.x);
        distanceToPlayer = VectorToPlayer.magnitude;
        directionToPlayer = VectorToPlayer.normalized;
        weaponDirection = -WeaponFollowPlayer.transform.up;
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
        Vector2[] sawDestinations = UsefullMethods.GetPolygonPositions(originPosition, amountOfSaws, 1, Offset);
        for (int i = 0; i < amountOfSaws; i++)
        {
            GameObject newSaw = Instantiate(PinkSawProjectile_Prefab, originPosition, Quaternion.identity, transform);
            PinkSaw_Projectile projectile = newSaw.GetComponent<PinkSaw_Projectile>();
            projectile.startSawing(originPosition, sawDestinations[i]);

            yield return new WaitForSeconds(delayBetweenSaws);
        }
    }
    public void EV_PinkSawProjectile()
    {
        UpdateVectorData();
        GameObject newSaw = Instantiate(PinkSawProjectile_Prefab, originPosition, Quaternion.identity, transform);
        PinkSaw_Projectile projectile = newSaw.GetComponent<PinkSaw_Projectile>();
        projectile.startSawing(originPosition, directionToPlayer);
    }

}
