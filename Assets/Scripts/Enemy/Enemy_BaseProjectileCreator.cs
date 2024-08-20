using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BaseProjectileCreator : MonoBehaviour
{
     public GameObject WeaponFollowPlayer;
    [HideInInspector] public Vector2 originPosition;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Vector2 playerPosition;
    [HideInInspector] public Vector2 VectorToPlayer;
    [HideInInspector] public float angleToPlayerRad;
    [HideInInspector] public float distanceToPlayer;
    [HideInInspector] public Vector2 directionToPlayer;
    [HideInInspector] public Vector2 weaponDirection;

    public void UpdateVectorData()
    {
        originPosition = transform.position;
        player = GlobalPlayerReferences.Instance.playerTf.gameObject;
        playerPosition = player.transform.position;
        VectorToPlayer = originPosition - playerPosition;
        angleToPlayerRad = Mathf.Atan2(VectorToPlayer.y, VectorToPlayer.x);
        distanceToPlayer = VectorToPlayer.magnitude;
        directionToPlayer = VectorToPlayer.normalized;
        weaponDirection = -WeaponFollowPlayer.transform.up;
    }
}
