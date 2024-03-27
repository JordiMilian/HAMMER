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
    Rigidbody2D _rigitbody;

    [SerializeField] Generic_Flash player_Flash;
    [SerializeField] Generic_DamageDetector damageDetector;
    [SerializeField] Player_Movement playerMovement;
    [SerializeField] Generic_DamageDealer damageDealer;
    


    [SerializeField] Player_ParryPerformer parryPerformer;
    [SerializeField] Player_EventSystem eventSystem;
    [SerializeField] Animator playerAnimator;
    

    public float CurrentDamage;
    public float BaseDamage;
  

    [SerializeField] float staggerTime = 1;
    bool receivingDamage;

    private void Awake()
    {
        _rigitbody = GetComponent<Rigidbody2D>();
    }
    

    private void OnEnable()
    {
        eventSystem.OnSuccessfulParry += OnSuccesfulParryCameraEffects;
        eventSystem.OnReceiveDamage += ReceiveDamageEffects;
        eventSystem.OnDealtDamage += OnHitEnemyCameraEffects;
        eventSystem.OnGettingParried += GettingParriedEffects;
        eventSystem.CallActivation += OnActivationFeedback;
    }
    private void OnDisable()
    {
        eventSystem.OnSuccessfulParry -= OnSuccesfulParryCameraEffects;
        eventSystem.OnReceiveDamage -= ReceiveDamageEffects;
        eventSystem.OnDealtDamage -= OnHitEnemyCameraEffects;
        eventSystem.OnGettingParried -= GettingParriedEffects;
        eventSystem.CallActivation -= OnActivationFeedback;
    }

    public void ReceiveDamageEffects(object sender, Player_EventSystem.ReceivedAttackInfo receivedAttackinfo)
    {
        eventSystem.OnStaminaAction?.Invoke(this, new Player_EventSystem.EventArgs_StaminaConsumption(1));
        if(!receivingDamage)
        {
            receivingDamage = true;

            playerMovement.CurrentSpeed = 0;
            
            CameraShake.Instance.ShakeCamera(1, 0.1f); ;
            TimeScaleEditor.Instance.HitStop(receivedAttackinfo.Hitstop);
            player_Flash.CallFlasher();

            Vector2 direction = (transform.position - receivedAttackinfo.Attacker.transform.position).normalized;
            StartCoroutine(ApplyForceOverTime(receivedAttackinfo.GeneralDirection * receivedAttackinfo.KnockBack, 0.3f));
            playerAnimator.SetTrigger("GetHit");
            StartCoroutine(InvulnerableAfterDamage());
        }
    }
    void GettingParriedEffects()
    {
        Player_ComboSystem_chargeless comboSystem = GetComponent<Player_ComboSystem_chargeless>();
        comboSystem.EV_HideWeaponCollider();
        comboSystem.ResetCount();
        playerMovement.EV_ReturnSpeed();
        playerMovement.EV_CanDash();
        playerAnimator.SetTrigger("Parried");
    }
     IEnumerator InvulnerableAfterDamage()
    {
        yield return new WaitForSeconds(staggerTime);
        playerMovement.CurrentSpeed = playerMovement.BaseSpeed;
        receivingDamage = false;
    }
    void OnActivationFeedback()
    {
        playerAnimator.SetTrigger("Reactivated");
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
        CameraShake.Instance.ShakeCamera(1 * damageDealer.Damage, 0.1f * damageDealer.Damage);
        //hitStop.Stop( 0.1f);
        TimeScaleEditor.Instance.HitStop(0.1f);
        //_HealthSystem.RemoveLife(-1);

    }
    IEnumerator ApplyForceOverTime(Vector3 forceVector, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            _rigitbody.AddForce(forceVector / duration * Time.deltaTime);
            yield return null;
        }
    }


}

   
