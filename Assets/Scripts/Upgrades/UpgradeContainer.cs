using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeContainer : MonoBehaviour, IDamageReceiver
{
    public Upgrade upgradeEffect; //this should be set when instantiated
    public SpriteRenderer iconRenderer;
    public Action<int> OnPickedUp; //it passes the Index in group 
    public int IndexInGroup;
    //If its an upgrade with group, we call directly OnSpawnContainer from the Group. 
    //If its an upgrade on its own, we call it on Start
    public bool isSoloUpgrade;

    [SerializeField] Animator containerAnimator;
    [SerializeField] Dialoguer dialoguer;
    CircleCollider2D ownCollider;

   

    private void Start()
    {
        if (isSoloUpgrade) { Initialize(); }
    }
     public void Initialize()
    {
        ownCollider = GetComponent<CircleCollider2D>();
        iconRenderer.sprite = upgradeEffect.iconSprite;
        dialoguer.TextLines[0] = upgradeEffect.title();
        dialoguer.TextLines[1] = upgradeEffect.shortDescription();
        ownCollider.enabled = true;
        containerAnimator.SetTrigger("Spawn");
    }
    public void OnPickedUpContainer()
    {
        OnPickedUp?.Invoke(IndexInGroup);
        ownCollider.enabled = false;
        StartCoroutine(UsefullMethods.destroyWithDelay(1.5f,gameObject));
        containerAnimator.SetTrigger("PickedUp");
    }
    public void OnDispawnContainer()
    {
        ownCollider.enabled = false;
       StartCoroutine( UsefullMethods.destroyWithDelay(1f, gameObject));
        containerAnimator.SetTrigger("Despawn");
    }

    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        OnDamageReceived_event?.Invoke(info);
    }
}
