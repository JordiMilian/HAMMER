using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeContainer : MonoBehaviour
{
    public Upgrade upgradeEffect; //this should be set when instantiated
    public SpriteRenderer iconRenderer;
    public Action<int> OnPickedUp; //it passes the Index in group 
    public int IndexInGroup;

    //If its an upgrade with group, we call directly OnSpawnContainer from the Group. 
    //If its an upgrade on its own, we call it on Start
    public bool isSoloUpgrade; 
                               

    Collider2D ownCollider;
    
    private void Start()
    {
        if (isSoloUpgrade) { OnSpawnContainer(); }
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

    
}
