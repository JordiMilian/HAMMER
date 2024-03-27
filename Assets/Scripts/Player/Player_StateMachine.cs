using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateMachine : MonoBehaviour
{
    [SerializeField] Player_EventSystem eventSystem;

    [SerializeField] Player_Movement movement;
    [SerializeField] Player_FollowMouse_withFocus followMouse;
    [SerializeField] Player_ComboSystem_chargeless comboSystem;
    [SerializeField] Collider2D DamageDetector;
    [SerializeField] Collider2D PositionCollider;
    [SerializeField] Transform MouseTarget;
    [SerializeField] List<GameObject> SpritesRoot = new List<GameObject>();

    [SerializeField] Animator playerAnimator;

    [SerializeField] GameObject weaponPivot;
    
    
    enum PlayerStates
    {
        Active, Inactive
    }
    PlayerStates currentState;
    private void OnEnable()
    {
        eventSystem.OnDeath += DisablePlayer;
        eventSystem.CallActivation += ReturnPlayer;
    }
    private void OnDisable()
    {
        eventSystem.OnDeath -= DisablePlayer;
        eventSystem.CallActivation -= ReturnPlayer;
    }
    void DisablePlayer(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        movement.enabled = false;
        followMouse.enabled = false;
        comboSystem.enabled = false;
        TargetGroupSingleton.Instance.RemoveTarget(MouseTarget);
        DamageDetector.enabled = false;
        PositionCollider.enabled = false;
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
        eventSystem.CallRespawn?.Invoke(); //Go to Player_RespawnerManager
    }
    public void ReturnPlayer()
    {
        movement.enabled = true;
        followMouse.enabled = true;
        comboSystem.enabled = true;
        TargetGroupSingleton.Instance.AddTarget(MouseTarget,1,0);
        DamageDetector.enabled = true;
        PositionCollider.enabled = true;
        foreach (GameObject root in SpritesRoot)
        {
            root.SetActive(true);
        }
    }
}
