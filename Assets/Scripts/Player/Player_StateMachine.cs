using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateMachine : MonoBehaviour
{
    [SerializeField] Player_EventSystem eventSystem;
    [SerializeField] Player_Movement movement;
    [SerializeField] Player_FollowMouse_withFocus followMouse;
    [SerializeField] Player_ComboSystem comboSystem;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator swordAnimator;
    [SerializeField] Generic_DamageDetector detector;
    [SerializeField] GameObject weaponPivot;
    enum PlayerStates
    {
        Active, Inactive
    }
    PlayerStates currentState;
    private void OnEnable()
    {
        eventSystem.OnDeath += DisableInput;
    }
    private void OnDisable()
    {
        eventSystem.OnDeath -= DisableInput;
    }
    void DisableInput(object sender, Generic_EventSystem.Args_DeadCharacter args)
    {
        movement.enabled = false;
        followMouse.enabled = false;
        comboSystem.enabled = false;
        detector.gameObject.GetComponent<Collider2D>().enabled = false;
        swordAnimator.SetTrigger("Death");
        
        playerAnimator.SetTrigger("Death");

    }
    public void EV_TeleportPlayer()
    {
        if (eventSystem.OnRespawn != null) eventSystem.OnRespawn(this, EventArgs.Empty);

        weaponPivot.transform.eulerAngles = new Vector3(
            weaponPivot.transform.eulerAngles.x,
            weaponPivot.transform.eulerAngles.y,
            90
            );
    }
    public void EV_SwordRespawnAnimation()
    {
        swordAnimator.SetTrigger("Respawn");
    }
    public void EV_ReturnControl()
    {
        movement.enabled = true;
        followMouse.enabled = true;
        comboSystem.enabled = true;
        detector.gameObject.GetComponent<Collider2D>().enabled = true;
        swordAnimator.SetTrigger("ReturnControl");
    }
}
