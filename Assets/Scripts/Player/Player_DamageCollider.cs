using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DamageCollider : MonoBehaviour
{
   Player_Controller playerController;
    private void Start()
    {
        playerController = GetComponentInParent<Player_Controller>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy_Attack_hitbox")
        {
            if (playerController.receivingDamage == false) StartCoroutine(playerController.ReceiveDamage(collision.gameObject));
        }
    }
}

