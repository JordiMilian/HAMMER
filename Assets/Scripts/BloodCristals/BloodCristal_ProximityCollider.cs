using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCristal_ProximityCollider : MonoBehaviour
{
    BloodCristals_FollowPlayer followPlayer;
    private void Awake()
    {
        followPlayer = GetComponentInParent<BloodCristals_FollowPlayer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            followPlayer.playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            followPlayer.playerInRange = false;
        }
    }
}
