using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Generic_OnTriggerEnterEvents : MonoBehaviour
{
    public List<string> ActivatorTags = new List<string>();
    public List<TagsEnum> ActivatorTagsTags = new List<TagsEnum>();
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
        List<int> indexesToRemove = new List<int>();
        for (int i = 0; i < ActivatorTags.Count; i++)
        {
            if (ActivatorTags[i] == tag)
            {
                indexesToRemove.Add(i);
            }
        }
        foreach (int i in indexesToRemove)
        {
            ActivatorTags.RemoveAt(i);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (TagsEnum tag in ActivatorTagsTags)
        {
            if (Tags.TagsDictionary[tag] == collision.tag)
            {
                OnTriggerEntered?.Invoke(collision);
                break;
            }
        }
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
