using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_ParryPerformer : MonoBehaviour
{
    Player_References playerRefs;
    /*
    [SerializeField] Collider2D weaponParryCollider;
    [SerializeField] Collider2D damageDetectorCollider;
    [SerializeField] Player_ComboSystem_chargeless comboSystem;
    [SerializeField] Player_ActionPerformer actionPerformer;
    [SerializeField] Player_EventSystem playerEvents;
    */
    

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Parry"));
            

            //playerAnimator.SetTrigger("Parry");
            //comboSystem.ResetCount();
        }
    }
    private void OnEnable()
    {
        playerRefs.playerEvents.OnPerformParry += perfomedParry;
    }
    void perfomedParry()
    {
        playerRefs.playerEvents.OnStaminaAction(0.5f);
    }
    public void EV_ShowParryCollider()
    {
        playerRefs.parryCollider.enabled = true;
        playerRefs.damageDetectorCollider.enabled = false;
    }
    public void EV_HideParryColldier()
    {
        playerRefs.parryCollider.enabled = false;
        playerRefs.damageDetectorCollider.enabled = true;
    }

}
