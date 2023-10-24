using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WeaponDetector : MonoBehaviour
{
    public Enemy01 Enemy01;
    public GameObject VFX_DealDamage;
    void Start()
    {
        Enemy01 = GetComponentInParent<Enemy01>();
        //Enemy01_Pivot = GetComponentInParent<Enemy01_Attack01_Collider>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack_Hitbox")
        {
            Enemy01.HitShield();
        }
        if (collision.CompareTag("PlayerDamageCollider"))
        {
            Instantiate(VFX_DealDamage, collision.ClosestPoint(transform.position), Quaternion.identity);
        }
    }
}
