using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI titleTMP, descriptionTMP;
    [SerializeField] GameObject panelRoot;
    [SerializeField] Generic_OnTriggerEnterEvents UIPlayerDetector;
    [SerializeField] TextMeshProUGUI TMP_PressA;
    CircleCollider2D ownCollider;

    private void OnEnable()
    {
        UIPlayerDetector.AddActivatorTag(Tags.Player_SinglePointCollider);
        UIPlayerDetector.OnTriggerEntered += ShowPanel;
        UIPlayerDetector.OnTriggerExited += HidePanel;
        HidePanel(null);
    }
    private void OnDisable()
    {
        UIPlayerDetector.OnTriggerEntered -= ShowPanel;
        UIPlayerDetector.OnTriggerExited -= HidePanel;
    }
    void ShowPanel(Collider2D col)
    {
        panelRoot.SetActive(true);
        TMP_PressA.text = $"Press {InputDetector.Instance.Select_String()} to cannibalize";

    }
    void HidePanel(Collider2D col)
    {
        panelRoot.SetActive(false);
    }


    private void Start()
    {
        if (isSoloUpgrade) { Initialize(); }
    }
     public void Initialize()
    {
        ownCollider = GetComponent<CircleCollider2D>();
        iconRenderer.sprite = upgradeEffect.iconSprite;
        titleTMP.text = upgradeEffect.Title;
        descriptionTMP.text = upgradeEffect.shortDescription();
        ownCollider.enabled = true;
        containerAnimator.SetTrigger("Spawn");
    }
    public void OnPickedUpContainer()
    {
        OnPickedUp?.Invoke(IndexInGroup);
        ownCollider.enabled = false;
        StartCoroutine(UsefullMethods.destroyWithDelay(1.5f,gameObject));
        containerAnimator.SetTrigger("PickedUp");
        HidePanel(null);
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
