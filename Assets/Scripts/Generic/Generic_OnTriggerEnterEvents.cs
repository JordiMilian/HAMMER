using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_OnTriggerEnterEvents : MonoBehaviour
{
    public List<string> ActivatorTags;
    public class EventArgsCollisionInfo
    {
        public Collider2D Collision;
        public EventArgsCollisionInfo(Collider2D collision)
        {
            Collision = collision;
        }
    }
    public event EventHandler<EventArgsCollisionInfo> OnTriggerEntered;
    public event EventHandler<EventArgsCollisionInfo> OnTriggerExited;
    public void AddActivatorTag(string tag)
    {
        foreach (string t in ActivatorTags)
        {
            if (tag == t) { return; }
        }
        ActivatorTags.Add(tag);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in ActivatorTags)
        {
            if(collision.CompareTag(tag))
            {
                if (OnTriggerEntered != null) OnTriggerEntered(this, new EventArgsCollisionInfo(collision));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string tag in ActivatorTags)
        {
            if (collision.CompareTag(tag))
            {
                if (OnTriggerExited != null) OnTriggerExited(this, new EventArgsCollisionInfo(collision));
            }
        }
    }
}
