using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Generic_OnTriggerEnterEvents : MonoBehaviour
{
    public List<string> ActivatorTags = new List<string>();
    public class EventArgsCollisionInfo
    {
        public Collider2D Collision;
        public EventArgsCollisionInfo(Collider2D collision)
        {
            Collision = collision;
        }
    }
    public event Action<Collider2D> OnTriggerEntered;
    public event Action<Collider2D> OnTriggerExited;
    public void AddActivatorTag(string tag)
    {
        foreach (string t in ActivatorTags)
        {
            if (tag == t) { return; }
        }
        ActivatorTags.Add(tag);
    }
    public void RemoveActivatorTag(string tag)
    {
        foreach(string t in ActivatorTags)
        {
            if(tag == t) { ActivatorTags.Remove(t); }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in ActivatorTags)
        {
            if(collision.CompareTag(tag))
            {
                OnTriggerEntered?.Invoke(collision);
                break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string tag in ActivatorTags)
        {
            if (collision.CompareTag(tag))
            {
                OnTriggerExited?.Invoke(collision);
                break;
            }
        }
    }
}
