using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_ParryPerformer : MonoBehaviour
{
    Animator playerAnimator;
    [SerializeField] Collider2D weaponParryCollider;
    [SerializeField] Collider2D damageDetectorCollider;

    public event EventHandler<EventArgs_ParryInfo> OnSuccessfulParry;
    public class EventArgs_ParryInfo : EventArgs
    {
        public Vector3 vector3data;
        public EventArgs_ParryInfo(Vector3 data)
        {
            vector3data = data;
        }
    }
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetTrigger("Parry");
        }
    }
    public void PublishSuccessfullParryDone(Vector3 closestPoint)
    {
        if (OnSuccessfulParry != null) OnSuccessfulParry(this, new EventArgs_ParryInfo(closestPoint)); 
        EV_HideParryColldier();
    }

    public void EV_ShowParryCollider()
    {
        weaponParryCollider.enabled = true;
        damageDetectorCollider.enabled = false;
    }
    public void EV_HideParryColldier()
    {
        weaponParryCollider.enabled = false;
        damageDetectorCollider.enabled = true;
    }

}
