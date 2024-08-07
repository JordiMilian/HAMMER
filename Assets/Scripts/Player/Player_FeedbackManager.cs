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
        playerRefs.events.OnSuccessfulParry += OnSuccesfulParryCameraEffects;
        playerRefs.events.OnReceiveDamage += ReceiveDamageEffects;
        playerRefs.events.OnDealtDamage += OnHitEnemyCameraEffects;
        playerRefs.events.OnGettingParried += GettingParriedEffects;
        playerRefs.events.CallShowAndEnable += OnActivationFeedback;
    }
    private void OnDisable()
    {
        playerRefs.events.OnSuccessfulParry -= OnSuccesfulParryCameraEffects;
        playerRefs.events.OnReceiveDamage -= ReceiveDamageEffects;
        playerRefs.events.OnDealtDamage -= OnHitEnemyCameraEffects;
        playerRefs.events.OnGettingParried -= GettingParriedEffects;
        playerRefs.events.CallShowAndEnable -= OnActivationFeedback;
    }

    public void ReceiveDamageEffects(object sender, Player_EventSystem.ReceivedAttackInfo receivedAttackinfo)
    {
        //playerRefs.events.OnStaminaAction?.Invoke(0.2f);
        if(!receivingDamage)
        {
            receivingDamage = true;

            playerRefs.playerMovement.CurrentSpeed = 0;
            
            CameraShake.Instance.ShakeCamera(1, 0.1f); ;
            TimeScaleEditor.Instance.HitStop(receivedAttackinfo.Hitstop);
            playerRefs.flasher.CallFlasher();

            Vector2 direction = (transform.position - receivedAttackinfo.Attacker.transform.position).normalized;
            StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRefs._rigidbody, receivedAttackinfo.ConcreteDirection * receivedAttackinfo.KnockBack, 0.3f));
            playerRefs.animator.SetTrigger("GetHit");
            StartCoroutine(InvulnerableAfterDamage());
        }
    }
    void GettingParriedEffects(int i)
    {
        Player_ComboSystem_chargeless comboSystem = GetComponent<Player_ComboSystem_chargeless>();
        comboSystem.EV_HideWeaponCollider();
        playerRefs.playerMovement.EV_ReturnSpeed();
        playerRefs.playerMovement.EV_CanDash();
        playerRefs.animator.SetTrigger("Parried");
    }
     IEnumerator InvulnerableAfterDamage()
    {
        yield return new WaitForSeconds(staggerTime);
        playerRefs.playerMovement.CurrentSpeed = playerRefs.playerMovement.BaseSpeed;
        receivingDamage = false;
    }
    void OnActivationFeedback()
    {
        playerRefs.animator.SetTrigger("Reactivated");
        CameraShake.Instance.ShakeCamera(0.5f, 0.3f);
    }
    public void OnSuccesfulParryCameraEffects(object sender, Player_EventSystem.SuccesfulParryInfo position)
    {
        TimeScaleEditor.Instance.HitStop(0.3f);
        CameraShake.Instance.ShakeCamera(0.6f, 0.1f);
    }
    public void OnHitEnemyCameraEffects(object sender, Player_EventSystem.DealtDamageInfo damageinfo)
    {
        CameraShake.Instance.ShakeCamera(1 * playerRefs.DamageDealersList[0].Damage, 0.1f * playerRefs.DamageDealersList[0].Damage);
        //hitStop.Stop( 0.1f);
        TimeScaleEditor.Instance.HitStop(0.1f);
        //_HealthSystem.RemoveLife(-1);

    }
    IEnumerator ApplyForceOverTime(Vector3 forceVector, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            playerRefs._rigidbody.AddForce(forceVector / duration * Time.deltaTime);
            yield return null;
        }
    }


}

   
