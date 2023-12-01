using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ParryDetection : MonoBehaviour
{
    Player_Controller Player;
    [SerializeField] GameObject ParryVFX;
    private void Awake()
    {
        Player = GetComponentInParent<Player_Controller>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Enemy_Attack_hitbox")
        {
            Player.OnParry();
            Instantiate(ParryVFX, collision.ClosestPoint(transform.position), Quaternion.identity);

        }
    }
}
