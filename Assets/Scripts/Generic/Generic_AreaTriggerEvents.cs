using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_AreaTriggerEvents : MonoBehaviour
{
    int enteredElements;
    public Action onAreaActive;
    public Action onAreaUnactive;
    public bool isAreaActive;

    List<string> ActivatorTags = new List<string>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in ActivatorTags)
        {
            if (collision.CompareTag(tag))
            {
                addedElement();
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
                removedElement();
                break;
            }
        }
    }
    void addedElement()
    {

        if (enteredElements <= 0) 
        {
            isAreaActive = true;
            onAreaActive?.Invoke();
            enteredElements = 0; 
        }
        enteredElements++;
       
    }
    void removedElement()
    {
        enteredElements--;

        if(enteredElements <= 0)
        {
            enteredElements = 0;
            isAreaActive = false;

            onAreaUnactive?.Invoke();
        }
    }
    public void AddActivatorTag(string tag)
    {
        foreach (string t in ActivatorTags)
        {
            if (tag == t) { return; }
        }
        ActivatorTags.Add(tag);
    }
}
