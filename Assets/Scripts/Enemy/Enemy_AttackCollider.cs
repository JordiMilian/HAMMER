using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackCollider : MonoBehaviour
{
    public float Damage;
    public float Knockback;
    public float HitStop;
    [SerializeField] bool IsEnemyWeapon;

    Enemy01 Enemy01;
    public GameObject VFX_DealDamage;
    void Start()
    {
        if (IsEnemyWeapon) 
        {
            Enemy01 = GetComponentInParent<Enemy01>();
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnemyWeapon)
        {
            if (collision.tag == "Attack_Hitbox")
            {
                Enemy01.HitShield();
            }
        }
        if (IsEnemyWeapon == false)
        {
            Debug.Log("Sin responsabilidades");
        }
       
        if (collision.CompareTag("PlayerDamageCollider"))
        {
            Instantiate(VFX_DealDamage, collision.ClosestPoint(transform.position), Quaternion.identity);
        }
    }
}
