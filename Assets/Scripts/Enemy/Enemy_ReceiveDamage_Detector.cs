using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ReceiveDamage_Detector : MonoBehaviour
{
    Enemy01 EnemyScript;
    void Start()
    {
        EnemyScript = GetComponentInParent<Enemy01>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack_Hitbox")
        {
            StartCoroutine(EnemyScript.ReceiveDamage(collision.gameObject));
        }
    }
}
