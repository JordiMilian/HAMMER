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
    enum PlayerStates
    {
        Active, Inactive
    }
    PlayerStates currentState;
    private void OnEnable()
    {
        playerRefs.events.OnDeath += DisablePlayer;
        playerRefs.events.CallActivation += ReturnPlayer;
    }
    private void OnDisable()
    {
        playerRefs.events.OnDeath -= DisablePlayer;
        playerRefs.events.CallActivation -= ReturnPlayer;
    }
    void DisablePlayer(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        playerRefs.playerMovement.enabled = false;
        playerRefs.followMouse.enabled = false;
        playerRefs.comboSystem.enabled = false;
        TargetGroupSingleton.Instance.RemoveTarget(MouseTarget);
        playerRefs.damageDetector.enabled = false;
        playerRefs.positionCollider.enabled = false;
        foreach (GameObject root in SpritesRoot)
        {
            root.SetActive(false);
        }

        weaponPivot.transform.eulerAngles = new Vector3(
            weaponPivot.transform.eulerAngles.x,
            weaponPivot.transform.eulerAngles.y,
            90
            );
        StartCoroutine(DelayedRespawn());
    }
    IEnumerator DelayedRespawn()
    {
        yield return new WaitForSeconds(3.5f);
        playerRefs.events.CallRespawn?.Invoke(); //Go to Player_RespawnerManager
    }
    public void ReturnPlayer()
    {
        playerRefs.playerMovement.enabled = true;
        playerRefs.followMouse.enabled = true;
        playerRefs.comboSystem.enabled = true;
        TargetGroupSingleton.Instance.AddTarget(MouseTarget,1,0);
        playerRefs.damageDetector.enabled = true;
        playerRefs.positionCollider.enabled = true;
        foreach (GameObject root in SpritesRoot)
        {
            root.SetActive(true);
        }
    }
}
