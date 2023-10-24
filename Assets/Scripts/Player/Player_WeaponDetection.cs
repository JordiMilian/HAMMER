using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WeaponDetection : MonoBehaviour
{
    CameraShake CameraShake;
    public Player_Controller Player;
    public GameObject VFX_HitEnemy;
    void Start()
    {
       CameraShake = GameObject.Find("CM vcam1").GetComponent<CameraShake>();
        Player = GameObject.Find("MainCharacter").GetComponent<Player_Controller>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Hit Enemy
        if (collision.tag == "Enemy_Hitbox")
        {
            CameraShake.ShakeCamera(1*Player.damage, 0.1f*Player.damage);
            Instantiate(VFX_HitEnemy, collision.ClosestPoint(transform.position), Quaternion.identity);
            StartCoroutine(Player.HealDamage(1));
        }
        //Parry
        if (collision.tag == "Enemy_Attack_hitbox")
        {
            Instantiate(VFX_HitEnemy, collision.ClosestPoint(transform.position), Quaternion.identity);
        }
    }
}
