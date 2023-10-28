using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Enemy01 : MonoBehaviour
{
   

    public float CurrentDamage;
    public float CurrentKnockBack;

    public Transform Player;
    public Collider2D WeaponCollider;
    public Animator EnemyAnimator;
    Enemy_FollowPlayer Enemy_FollowPlayer;
    Enemy_HealthSystem Enemy_HealthSystem;
    Enemy_AttacksProvider enemy_AttackProvider;
    HitStop hitStop;


    public bool Attacking = false;

    TrailRenderer WeaponTrail;
    
    RotationConstraint SpritesConstraint;

    private void Start()
    {
        Player = GameObject.Find("MainCharacter").transform;
       
        //EnemyAttacks[0] = new EnemyAttack(8,1.43f,20,"Attack01",30,70);
        //EnemyAttacks[1] = new EnemyAttack(8,1.42f,15,"Attack02",80,0);
        //EnemyAttacks[2] = new EnemyAttack(5, 3.51f, 15, "Attack03",100,100);
        //EnemyAttacks[3] = new EnemyAttack(10, 2.4f, 25, "Attack04", 100,100);
        EnemyAnimator = GetComponent<Animator>();
        WeaponTrail = GetComponentInChildren<TrailRenderer>();

        ConstraintSource CameraConstrain = new ConstraintSource();
        CameraConstrain.sourceTransform = GameObject.Find("Main Camera").transform;
        CameraConstrain.weight = 1;
        SpritesConstraint = GetComponentInChildren<RotationConstraint>();
        SpritesConstraint.AddSource(CameraConstrain);

        Enemy_FollowPlayer = GetComponent<Enemy_FollowPlayer>();
        Enemy_HealthSystem = GetComponent<Enemy_HealthSystem>();

        hitStop = FindObjectOfType<HitStop>();

    }
    public IEnumerator Attack(Enemy_AttacksProvider.EnemyAttack Attack)
    {
        Attacking = true;
        CurrentDamage = Attack.Damage;
        CurrentKnockBack = Attack.KnockBack;
        EnemyAnimator.SetTrigger(Attack.TriggerName);
        yield return new WaitForSeconds(Attack.AnimationTime);
        ResetAllTriggers();
        Attacking = false;

    }
    public IEnumerator ReceiveDamage(GameObject Weapon)
    {
        Debug.Log("EnemyAttacked");
        Enemy_HealthSystem.UpdateLife(Weapon.GetComponent<Player_WeaponDetection>().Player.damage);
        Enemy_FollowPlayer.SlowSpeed();

        hitStop.Stop(0.05f);
        //EnemyRigidBody.AddForce(transform.up *-1000);
        EnemyAnimator.SetTrigger("PushBack");
        yield return new WaitForSeconds(0.3f);
        Enemy_FollowPlayer.ReturnSpeed();
        Enemy_FollowPlayer.IsAgroo = true;
        
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
        Enemy_FollowPlayer.ReturnSpeed();
    }
    public void EndHitShield()
    {
        EnemyAnimator.SetBool("HitShield", false);
        Enemy_HideAttackCollider();
    }
    public void ShowTrail() { WeaponTrail.enabled = true; }
    public void HideTrail() { WeaponTrail.enabled = false; }
    private void ResetAllTriggers()
    {
        foreach (var param in EnemyAnimator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                EnemyAnimator.ResetTrigger(param.name);
            }
        }
    }
}
