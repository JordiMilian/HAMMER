using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_OnShortRange : MonoBehaviour
{
    Enemy_AttacksProvider attacksProvider;
    void Start()
    {
        attacksProvider = GetComponentInParent<Enemy_AttacksProvider>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            attacksProvider.OnShortRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            attacksProvider.OnShortRange = false;
        }
    }
}
