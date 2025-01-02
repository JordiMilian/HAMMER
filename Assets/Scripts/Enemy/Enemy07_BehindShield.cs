using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy07_BehindShield : MonoBehaviour
{
    [SerializeField] Collider2D ParryCollider;
    [SerializeField] float DeactivationTime;
    Coroutine Deactivator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.Player_Hitbox))
        {
            if (Deactivator != null) { StopCoroutine(Deactivator); }
            Deactivator = StartCoroutine(DeactivateShield());
        }
        
    }
    IEnumerator DeactivateShield()
    {
        ParryCollider.enabled = false;
        yield return new WaitForSeconds(DeactivationTime);
        ParryCollider.enabled = true;
    }
}
