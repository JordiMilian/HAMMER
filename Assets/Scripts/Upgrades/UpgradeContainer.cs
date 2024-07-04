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

    [SerializeField] Dialoguer dialoguer;
    Collider2D ownCollider;
    
    private void Start()
    {
        if (isSoloUpgrade) { OnSpawnContainer(); }
    }
     public void OnSpawnContainer()
    {
        ownCollider = GetComponent<Collider2D>();
        iconRenderer.sprite = upgradeEffect.iconSprite;
        dialoguer.TextLines[0] = upgradeEffect.shortDescription();
        ownCollider.enabled = true;
    }
    public void OnPickedUpContainer()
    {
        OnPickedUp?.Invoke(IndexInGroup);
        ownCollider.enabled = false;
        StartCoroutine(UsefullMethods.destroyWithDelay(1,gameObject));
    }
    public void OnDispawnContainer()
    {
       StartCoroutine( UsefullMethods.destroyWithDelay(.1f, gameObject));
    }

    
}
