using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AgrooDetection : MonoBehaviour
{
    public event EventHandler OnPlayerDetected;
    public event EventHandler OnPlayerExited;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsCollection.instance.Player))
        {
            if (OnPlayerDetected != null) OnPlayerDetected(this,EventArgs.Empty);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsCollection.instance.Player))
        {
            if (OnPlayerExited != null) OnPlayerExited(this, EventArgs.Empty);
        }
    }

}
