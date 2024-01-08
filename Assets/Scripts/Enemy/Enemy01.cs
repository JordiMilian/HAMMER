using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static Generic_DamageDetector;

public class Enemy01 : MonoBehaviour
{
   

    public float CurrentDamage;
    public float CurrentKnockBack;
    public Collider2D WeaponCollider;
    public Animator EnemyAnimator;
    Enemy_Movement Enemy_FollowPlayer;
    Enemy_HealthSystem Enemy_HealthSystem;


    HitStop hitStop;
    Generic_Flash flasher;


    TrailRenderer WeaponTrail;
    
    RotationConstraint SpritesConstraint;

    [SerializeField] Generic_DamageDetector damageDetector;

    private void Start()
    {

       
        //EnemyAttacks[0] = new EnemyAttack(8,1.43f,20,"Attack01",30,70);
        //EnemyAttacks[1] = new EnemyAttack(8,1.42f,15,"Attack02",80,0);
        //EnemyAttacks[2] = new EnemyAttack(5, 3.51f, 15, "Attack03",100,100);
        //EnemyAttacks[3] = new EnemyAttack(10, 2.4f, 25, "Attack04", 100,100);

        EnemyAnimator = GetComponent<Animator>();
        WeaponTrail = GetComponentInChildren<TrailRenderer>();

        ConstraintSource CameraConstrain = new ConstraintSource();
        CameraConstrain.sourceTransform = Camera.main.transform;
        CameraConstrain.weight = 1;
        SpritesConstraint = GetComponentInChildren<RotationConstraint>();
        SpritesConstraint.AddSource(CameraConstrain);

        Enemy_FollowPlayer = GetComponent<Enemy_Movement>();
        Enemy_HealthSystem = GetComponent<Enemy_HealthSystem>();

        hitStop = FindObjectOfType<HitStop>();

        flasher = GetComponent<Generic_Flash>();

    }
    private void OnEnable()
    {
        damageDetector.OnReceiveDamage += ReceiveDamage;
    }
    private void OnDisable()
    {
        damageDetector.OnReceiveDamage -= ReceiveDamage;
    }
    public void ReceiveDamage(object sender, EventArgs_ReceivedAttackInfo receivedAttackinfo)
    {
        
        flasher.CallFlasher();
        Enemy_FollowPlayer.EV_SlowRotationSpeed();
        Enemy_FollowPlayer.IsAgroo = true;
        hitStop.Stop(0.05f);
       
        EnemyAnimator.SetTrigger("PushBack");
        StartCoroutine(WaitReceiveDamage());
       
    }  
    IEnumerator WaitReceiveDamage()
    {
        yield return new WaitForSeconds(0.3f);
        Enemy_FollowPlayer.EV_ReturnRotationSpeed();  
    }
    public void HitShield()
    {
        EnemyAnimator.SetBool("HitShield", true);
    }
   
    public void Enemy_ShowAttackCollider()
    {
        WeaponCollider.enabled = true;
    }
    public void Enemy_HideAttackCollider()
    {
        WeaponCollider.enabled = false;
        Enemy_FollowPlayer.EV_ReturnRotationSpeed();
    }
    public void EndHitShield()
    {
        EnemyAnimator.SetBool("HitShield", false);
        Enemy_HideAttackCollider();
    }
    public void ShowTrail() { WeaponTrail.enabled = true; }
    public void HideTrail() { WeaponTrail.enabled = false; }
}
