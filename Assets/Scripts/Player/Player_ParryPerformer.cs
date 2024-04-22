using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_ParryPerformer : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;

    void onParryPressed()
    {
        playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Parry"));
    }
    private void OnEnable()
    {
        playerRefs.events.OnPerformParry += perfomedParry;
        InputDetector.Instance.OnParryPressed += onParryPressed;
    }
    private void OnDisable()
    {
        playerRefs.events.OnPerformParry -= perfomedParry;
        InputDetector.Instance.OnParryPressed -= onParryPressed;
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
