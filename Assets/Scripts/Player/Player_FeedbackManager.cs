using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Player_EventSystem;
//using UnityEngine.Windows;

public class Player_FeedbackManager : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    [SerializeField] float staggerTime = 1;
    bool receivingDamage;

    

    private void OnEnable()
    {
        playerRefs.playerEvents.OnSuccessfulParry += OnSuccesfulParryCameraEffects;
        playerRefs.playerEvents.OnReceiveDamage += ReceiveDamageEffects;
        playerRefs.playerEvents.OnDealtDamage += OnHitEnemyCameraEffects;
        playerRefs.playerEvents.OnGettingParried += GettingParriedEffects;
        playerRefs.playerEvents.CallActivation += OnActivationFeedback;
    }
    private void OnDisable()
    {
        playerRefs.playerEvents.OnSuccessfulParry -= OnSuccesfulParryCameraEffects;
        playerRefs.playerEvents.OnReceiveDamage -= ReceiveDamageEffects;
        playerRefs.playerEvents.OnDealtDamage -= OnHitEnemyCameraEffects;
        playerRefs.playerEvents.OnGettingParried -= GettingParriedEffects;
        playerRefs.playerEvents.CallActivation -= OnActivationFeedback;
    }

    public void ReceiveDamageEffects(object sender, Player_EventSystem.ReceivedAttackInfo receivedAttackinfo)
    {
        playerRefs.playerEvents.OnStaminaAction?.Invoke(1);
        if(!receivingDamage)
        {
            receivingDamage = true;

            playerRefs.playerMovement.CurrentSpeed = 0;
            
            CameraShake.Instance.ShakeCamera(1, 0.1f); ;
            TimeScaleEditor.Instance.HitStop(receivedAttackinfo.Hitstop);
            playerRefs.playerFlasher.CallFlasher();

            Vector2 direction = (transform.position - receivedAttackinfo.Attacker.transform.position).normalized;
            StartCoroutine(ApplyForceOverTime(receivedAttackinfo.GeneralDirection * receivedAttackinfo.KnockBack, 0.1f));
            playerRefs.playerAnimator.SetTrigger("GetHit");
            StartCoroutine(InvulnerableAfterDamage());
        }
    }
    void GettingParriedEffects()
    {
        Player_ComboSystem_chargeless comboSystem = GetComponent<Player_ComboSystem_chargeless>();
        comboSystem.EV_HideWeaponCollider();
        comboSystem.ResetCount();
        playerRefs.playerMovement.EV_ReturnSpeed();
        playerRefs.playerMovement.EV_CanDash();
        playerRefs.playerAnimator.SetTrigger("Parried");
    }
     IEnumerator InvulnerableAfterDamage()
    {
        yield return new WaitForSeconds(staggerTime);
        playerRefs.playerMovement.CurrentSpeed = playerRefs.playerMovement.BaseSpeed;
        receivingDamage = false;
    }
    void OnActivationFeedback()
    {
        playerRefs.playerAnimator.SetTrigger("Reactivated");
        CameraShake.Instance.ShakeCamera(0.5f, 0.3f);
    }
    public void OnSuccesfulParryCameraEffects(object sender, Player_EventSystem.SuccesfulParryInfo position)
    {
        //hitStop.Stop(StopSeconds: 0.3f);
        TimeScaleEditor.Instance.HitStop(0.3f);
        CameraShake.Instance.ShakeCamera(0.6f, 0.1f);
    }
    public void OnHitEnemyCameraEffects(object sender, Player_EventSystem.DealtDamageInfo damageinfo)
    {
        CameraShake.Instance.ShakeCamera(1 * playerRefs.damageDealer.Damage, 0.1f * playerRefs.damageDealer.Damage);
        //hitStop.Stop( 0.1f);
        TimeScaleEditor.Instance.HitStop(0.1f);
        //_HealthSystem.RemoveLife(-1);

    }
    IEnumerator ApplyForceOverTime(Vector3 forceVector, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            playerRefs.playerRB.AddForce(forceVector / duration * Time.deltaTime);
            yield return null;
        }
    }


}

   
