using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackRangeDetector : MonoBehaviour
{
    public EventHandler OnPlayerEntered;
    public EventHandler OnPlayerExited;
    public BoxCollider2D collider;
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player_SinglePointCollider"))
        {
            if(OnPlayerEntered != null) OnPlayerEntered(this,EventArgs.Empty);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_SinglePointCollider"))
        {
            if (OnPlayerExited != null) OnPlayerExited(this, EventArgs.Empty);
        }
            
    }
}
