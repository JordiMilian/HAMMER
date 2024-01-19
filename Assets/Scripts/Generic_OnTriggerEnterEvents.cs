using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_OnTriggerEnterEvents : MonoBehaviour
{
    public List<string> ActivatorTags;
    public class EventArgsTriggererInfo
    {
        public string Tag;
        public Collider2D Collision;
        public EventArgsTriggererInfo(string tag,Collider2D collision)
        {
            Tag = tag;
            Collision = collision;
        }
    }
    public event EventHandler<EventArgsTriggererInfo> OnTriggerEntered;
    public event EventHandler<EventArgsTriggererInfo> OnTriggerExited;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in ActivatorTags)
        {
            if(collision.CompareTag(tag))
            {
                if (OnTriggerEntered != null) OnTriggerEntered(this, new EventArgsTriggererInfo(collision.tag, collision));
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string tag in ActivatorTags)
        {
            if (collision.CompareTag(tag))
            {
                if (OnTriggerExited != null) OnTriggerExited(this, new EventArgsTriggererInfo(collision.tag, collision));
            }
        }
    }
}
