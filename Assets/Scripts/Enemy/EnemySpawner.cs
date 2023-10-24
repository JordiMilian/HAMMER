using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject EnemyPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Attack_Hitbox")
        {
        var Enemy = Instantiate(EnemyPrefab, transform.position, transform.rotation);
        }
        
    }
}
