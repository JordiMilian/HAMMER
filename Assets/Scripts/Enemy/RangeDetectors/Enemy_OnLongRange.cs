using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_OnLongRange : MonoBehaviour
{
   
    Enemy_AttacksProvider attacksProvider;
    private void Start()
    {
        attacksProvider = GetComponentInParent<Enemy_AttacksProvider>();
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player_SinglePointCollider")
        {
            attacksProvider.OnLongRange = true;
           
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player_SinglePointCollider")
        {
            attacksProvider.OnLongRange = false;
        }
    }
}
