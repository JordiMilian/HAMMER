using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_ParryPerformer : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;

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
        playerRefs.events.OnPerformParry += perfomedParry;
    }
    void perfomedParry()
    {
        playerRefs.events.OnStaminaAction(0.5f);
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
