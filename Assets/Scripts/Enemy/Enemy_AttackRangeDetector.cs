using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackRangeDetector : MonoBehaviour
{
    public Action OnPlayerEntered;
    public Action OnPlayerExited;
    public BoxCollider2D ownCollider;
    private void Awake()
    {
        ownCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            if(OnPlayerEntered != null) OnPlayerEntered();
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            if (OnPlayerExited != null) OnPlayerExited();
        }
            
    }
}
