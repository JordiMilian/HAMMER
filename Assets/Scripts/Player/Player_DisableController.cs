using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DisableController : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;

     Transform MouseTarget;
    [SerializeField] List<GameObject> SpritesRoot = new List<GameObject>();
    public bool isScriptDisabled;

    private void Awake()
    {
        MouseTarget = MouseCameraTarget.Instance.transform;
    }
    /*
    private void OnEnable()
    {
        playerRefs.events.CallHideAndDisable += HideAndDisablePlayer;
        playerRefs.events.CallShowAndEnable += ShowAndEnablePlayer;
    }
    private void OnDisable()
    {
        playerRefs.events.CallHideAndDisable -= HideAndDisablePlayer;
        playerRefs.events.CallShowAndEnable -= ShowAndEnablePlayer;
    }
    */
    /*
    public void HideAndDisablePlayer()
    {
        DisablePlayerScripts();

        foreach (GameObject root in SpritesRoot)
        {
            root.SetActive(false);
        }

    }
     public void ShowAndEnablePlayer()
    {
        EnablePlayerScripts();
        foreach (GameObject root in SpritesRoot)
        {
            root.SetActive(true);
        }
        GameEvents.OnPlayerRespawned?.Invoke();
    }
    
    public void DisablePlayerScripts()
    {
        playerRefs.animator.SetBool("Walking", false);
        //playerRefs.playerMovement.enabled = false;
        playerRefs.followMouse.enabled = false;
        TargetGroupSingleton.Instance.RemoveTarget(MouseTarget);
        playerRefs.damageDetector.enabled = false;
        playerRefs.positionCollider.enabled = false;
        isScriptDisabled = true;
    }
   public void EnablePlayerScripts()
    {
        //playerRefs.playerMovement.enabled = true;
        playerRefs.followMouse.enabled = true;
        TargetGroupSingleton.Instance.AddTarget(MouseTarget, 1, 0);
        playerRefs.damageDetector.enabled = true;
        playerRefs.positionCollider.enabled = true;
        isScriptDisabled = false;
    }
    */
}
