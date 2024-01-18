using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Generic_DamageDealer;
using static Generic_DamageDetector;
using static Player_ParryPerformer;
//using UnityEngine.Windows;

public class Player_FeedbackManager : MonoBehaviour
{
    Rigidbody2D _rigitbody;

    [SerializeField] HitStop hitStop;
    [SerializeField] Generic_Flash player_Flash;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] Generic_DamageDetector damageDetector;
    [SerializeField] Player_Movement playerMovement;
    [SerializeField] Generic_DamageDealer damageDealer;


    [SerializeField] Player_ParryPerformer parryPerformer;
    

    public float CurrentDamage;
    public float BaseDamage;
  

    [SerializeField] float staggerTime = 1;
    bool receivingDamage;

    private void Awake()
    {
        _rigitbody = GetComponent<Rigidbody2D>();
        cameraShake = GameObject.Find("CM vcam1").GetComponent<CameraShake>();
        hitStop = GameObject.Find("HitStop").GetComponent<HitStop>();
    }
    

    private void OnEnable()
    {
        parryPerformer.OnSuccessfulParry += OnSuccesfulParryCameraEffects;
        damageDetector.OnReceiveDamage += ReceiveDamageEffects;
        damageDealer.OnDealtDamage += OnHitEnemyCameraEffects;
    }
    private void OnDisable()
    {
        parryPerformer.OnSuccessfulParry -= OnSuccesfulParryCameraEffects;
        damageDetector.OnReceiveDamage -= ReceiveDamageEffects;
        damageDealer.OnDealtDamage -= OnHitEnemyCameraEffects;
    }

    public void ReceiveDamageEffects(object sender, EventArgs_ReceivedAttackInfo receivedAttackinfo)
    {
        if(!receivingDamage)
        {
            receivingDamage = true;

            playerMovement.CurrentSpeed = 0;
            
            cameraShake.ShakeCamera(1, 0.1f); ;
            hitStop.Stop(receivedAttackinfo.Hitstop);
            player_Flash.CallFlasher();

            Vector2 direction = (transform.position - receivedAttackinfo.Attacker.transform.position).normalized;
            _rigitbody.AddForce(direction * (receivedAttackinfo.KnockBack), ForceMode2D.Impulse);
            StartCoroutine(InvulnerableAfterDamage());
        }
    }
     IEnumerator InvulnerableAfterDamage()
    {
        yield return new WaitForSeconds(staggerTime);
        playerMovement.CurrentSpeed = playerMovement.BaseSpeed;
        receivingDamage = false;
    }
    public void OnSuccesfulParryCameraEffects(object sender, EventArgs_ParryInfo position)
    {
        hitStop.Stop(StopSeconds: 0.3f);
        cameraShake.ShakeCamera(0.6f, 0.1f);
    }
    public void OnHitEnemyCameraEffects(object sender, EventArgs_DealtDamageInfo damageinfo)
    {
        cameraShake.ShakeCamera(1 * damageDealer.Damage, 0.1f * damageDealer.Damage);
        hitStop.Stop(StopSeconds: 0.1f);
        //_HealthSystem.RemoveLife(-1);

    }

    
}

   
