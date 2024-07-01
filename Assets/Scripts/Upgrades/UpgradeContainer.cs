using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeContainer : MonoBehaviour
{
    public Upgrade upgradeEffect;
    public SpriteRenderer iconRenderer;
    public Action<int> OnPickedUp; //it passes the Index in group 
    public int IndexInGroup;
    Collider2D ownCollider;
    private void Awake()
    {
        ownCollider = GetComponent<Collider2D>();
    }
    public void OnSpawnContainer()
    {
        Debug.Log("Just spawned container " + IndexInGroup);
        ownCollider = GetComponent<Collider2D>();
        iconRenderer.sprite = upgradeEffect.iconSprite;
        ownCollider.enabled = true;
    }
    public void OnPickedUpContainer()
    {
        OnPickedUp?.Invoke(IndexInGroup);
        Debug.Log("Picked container " + IndexInGroup);
        ownCollider.enabled = false;
        StartCoroutine(UsefullMethods.destroyWithDelay(1,gameObject));
    }
    public void OnDispawnContainer()
    {
        Debug.Log("Dispawned container " + IndexInGroup);
       StartCoroutine( UsefullMethods.destroyWithDelay(.1f, gameObject));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            OnPickedUpContainer();
        }
    }
    
}
