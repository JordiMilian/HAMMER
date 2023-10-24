using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_OnMidRange: MonoBehaviour
{
    
    Enemy_AttacksProvider attacksProvider;
    private void Start()
    {
        attacksProvider = GetComponentInParent<Enemy_AttacksProvider>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            attacksProvider.OnMidRange = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            attacksProvider.OnMidRange = false;
        }
    }
}
