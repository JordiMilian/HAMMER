using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static Generic_DamageDetector;

public class Enemy01 : MonoBehaviour
{

    public Collider2D WeaponCollider;
    [SerializeField] Animator EnemyAnimator;
    [SerializeField] Enemy_AgrooMovement enemyMovement;
    [SerializeField] HitStop hitStop;
    [SerializeField] Generic_Flash flasher;
    [SerializeField] TrailRenderer WeaponTrail;
    [SerializeField] Generic_DamageDetector damageDetector;
    [SerializeField] Generic_DamageDealer damageDealer;

    private void Awake()
    {
        hitStop = FindObjectOfType<HitStop>();
    }
    private void OnEnable()
    {
        damageDetector.OnReceiveDamage += ReceiveDamage;
        damageDealer.OnGettingParried += GettingParried;
    }
    private void OnDisable()
    {
        damageDetector.OnReceiveDamage -= ReceiveDamage;
    }
    public void ReceiveDamage(object sender, EventArgs_ReceivedAttackInfo receivedAttackinfo)
    {
        
        flasher.CallFlasher();
        enemyMovement.EV_SlowRotationSpeed();
        enemyMovement.EV_SlowMovingSpeed();
        //enemyMovement.IsAgroo = true;
        TimeScaleEditor.Instance.HitStop(0.05f);
        EnemyAnimator.SetTrigger("PushBack");
        StartCoroutine(WaitReceiveDamage());
       
    }  
    IEnumerator WaitReceiveDamage()
    {
        yield return new WaitForSeconds(0.3f);
        enemyMovement.EV_ReturnAllSpeed();
    }
    void GettingParried(object sender, EventArgs args)
    {
        EnemyAnimator.SetBool("HitShield", true);
        WeaponCollider.enabled = false;
    }
    public void HitShield()
    {
        EnemyAnimator.SetBool("HitShield", true);
        WeaponCollider.enabled = false;
    }
    public void EndHitShield()
    {
        EnemyAnimator.SetBool("HitShield", false);
    }


    
    public void ShowTrail() { if(WeaponTrail != null) WeaponTrail.enabled = true; }
    public void HideTrail() { if (WeaponTrail != null) WeaponTrail.enabled = false; }
}
