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
    [SerializeField] Player_ComboSystem comboSystem;
    [SerializeField] Player_EventSystem eventSystem;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetTrigger("Parry");
            comboSystem.ComboOver();
        }
    }
    public void PublishSuccessfullParryDone(Vector3 closestPoint)
    {
        if (eventSystem.OnSuccessfulParry != null) eventSystem.OnSuccessfulParry(this, new Player_EventSystem.EventArgs_ParryInfo(closestPoint)); 
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
