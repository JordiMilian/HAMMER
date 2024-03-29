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
    [SerializeField] Player_ComboSystem_chargeless comboSystem;
    [SerializeField] Player_ActionPerformer actionPerformer;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Parry"));
            //playerAnimator.SetTrigger("Parry");
            //comboSystem.ResetCount();
        }
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
