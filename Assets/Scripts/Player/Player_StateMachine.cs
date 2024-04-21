using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateMachine : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;

    [SerializeField] Transform MouseTarget;
    [SerializeField] List<GameObject> SpritesRoot = new List<GameObject>();

    [SerializeField] GameObject weaponPivot;

    private void Awake()
    {
        MouseTarget = GameObject.Find(TagsCollection.MouseCameraTarget).transform;
    }
    private void OnEnable()
    {
        playerRefs.events.OnDeath += HideAndDisablePlayer;
        playerRefs.events.CallHideAndDisable += ShowAndEnablePlayer;
        playerRefs.events.CallDisable += DisablePlayerScripts;
        playerRefs.events.CallEnable += EnablePlayerScripts;
    }
    private void OnDisable()
    {
        playerRefs.events.OnDeath -= HideAndDisablePlayer;
        playerRefs.events.CallHideAndDisable -= ShowAndEnablePlayer;
        playerRefs.events.CallDisable -= DisablePlayerScripts;
        playerRefs.events.CallEnable -= EnablePlayerScripts;
    }
    void HideAndDisablePlayer(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        DisablePlayerScripts();

        foreach (GameObject root in SpritesRoot)
        {
            root.SetActive(false);
        }

        SetupForRespwan(); 
    }
    public void ShowAndEnablePlayer()
    {
        EnablePlayerScripts();
        foreach (GameObject root in SpritesRoot)
        {
            root.SetActive(true);
        }
    }
    void DisablePlayerScripts()
    {
        playerRefs.playerMovement.enabled = false;
        playerRefs.followMouse.enabled = false;
        playerRefs.comboSystem.enabled = false;
        TargetGroupSingleton.Instance.RemoveTarget(MouseTarget);
        playerRefs.damageDetector.enabled = false;
        playerRefs.positionCollider.enabled = false;
    }
    void SetupForRespwan()
    {
        weaponPivot.transform.eulerAngles = new Vector3(
                    weaponPivot.transform.eulerAngles.x,
                    weaponPivot.transform.eulerAngles.y,
                    90
                    );
        StartCoroutine(DelayedRespawn());
    }
    void EnablePlayerScripts()
    {
        playerRefs.playerMovement.enabled = true;
        playerRefs.followMouse.enabled = true;
        playerRefs.comboSystem.enabled = true;
        TargetGroupSingleton.Instance.AddTarget(MouseTarget, 1, 0);
        playerRefs.damageDetector.enabled = true;
        playerRefs.positionCollider.enabled = true;
    }
    IEnumerator DelayedRespawn()
    {
        yield return new WaitForSeconds(3.5f);
        playerRefs.events.CallRespawn?.Invoke(); //Go to Player_RespawnerManager
    }
    
}
