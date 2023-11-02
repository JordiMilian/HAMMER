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
        if (collision.CompareTag("Static_Attack_hitbox") || collision.CompareTag("Enemy_Attack_hitbox"))
        {

            if (playerController.receivingDamage == false)
            {
                StartCoroutine(playerController.ReceiveDamage(collision.gameObject.GetComponent<Enemy_AttackCollider>()));
            }
        }
    }
}

