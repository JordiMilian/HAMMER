using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Windows;

public class Player_Controller : MonoBehaviour
{
    Rigidbody2D _rigitbody;
    Animator Player_Animator;
    Player_HealthSystem _HealthSystem;
    Collider2D DamageCollider;
    TrailRenderer WeaponTrail;
    HitStop hitStop;

    public float CurrentSpeed;
    public float BaseSpeed;
    float chargingSpeed;
   

   
    CameraShake cameraShake;
    public Collider2D WeaponCollider;
    Player_FollowMouse followMouse;
    Player_Roll playerRoll;
    
    public float damage;
    public float damageMultiplier;
    public float maxDamage;
    public float minDamage;
    public bool charging;
    public float Health;
    public float staggerTime = 1;

    public bool Attacking;

    
    
    public float DashPower;
    public float DashTime;
    public float DashCooldown;
  
    public bool receivingDamage = false;

    public AnimationCurve Curve;
        
    void Start()
    {
        Player_Animator = GetComponent<Animator>();
        _HealthSystem = GetComponent<Player_HealthSystem>();
        playerRoll = GetComponent<Player_Roll>();
       
        _rigitbody = GetComponent<Rigidbody2D>();
        damage = minDamage;
        charging = false;
        Attacking = false;
        CurrentSpeed = BaseSpeed;
       
        
        cameraShake = GameObject.Find("CM vcam1").GetComponent<CameraShake>();
        WeaponTrail = GetComponentInChildren<TrailRenderer>();
        DamageCollider = GameObject.Find("P_DamageCollider").GetComponent<Collider2D>();
        chargingSpeed = CurrentSpeed / 3;
        followMouse = GetComponentInChildren<Player_FollowMouse>();
        hitStop = FindObjectOfType<HitStop>();
    }


    void Update()
    {
         Move(); CheckWalking(); 
       
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Attacking == false)
            { Attack01_Charge(); }
            if (Attacking == true)
            {
                if (Player_Animator.GetInteger("ComboCue") <= 2)
                {
                    Player_Animator.SetInteger("ComboCue", Player_Animator.GetInteger("ComboCue")+1);
                    if (Player_Animator.GetInteger("ComboCue") > 2) { Player_Animator.SetInteger("ComboCue", 2); }
                }          
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            
            if (charging == true)
            {
               
                Player_Animator.SetBool("Attack01_Release", true);
                
            }
        }
        //While charging
        if (charging == true)
        {
            damage = damage * damageMultiplier;
            //_speed = chargingSpeed;
            if (damage >= maxDamage) { Player_Animator.SetBool("Attack01_Release", true); }
        }
    }
    void Move()
    {
        var input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        //_rigitbody.velocity = input.normalized * _speed;
        _rigitbody.AddForce(input.normalized*CurrentSpeed*Time.deltaTime *100);
    }
    void CheckWalking()
    {
        if (playerRoll.isDashing == false)
        { 
            if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
            {
                Player_Animator.SetBool("Walking",true);
            }
            else { Player_Animator.SetBool("Walking", false);
                   }
        }
    }
    void Attack01_Charge()
    {
            damage = minDamage;
            Player_Animator.SetBool("Attack01_Charging",true);
            charging = true;
            Player_Animator.SetInteger("ComboCue", Player_Animator.GetInteger("ComboCue") + 1);
    }
    public void Attack01()
    {
        Attacking = true;
        playerRoll.canDash = false;
        if (charging == true)
        {
            //Player_Animator.SetBool("Attack01_Release_Bool", false);
            charging = false;
            Player_Animator.SetBool("Attack01_Charging", false);
        }
    }
    public IEnumerator ReceiveDamage(Enemy_AttackCollider attackCollider)
    {
        
        receivingDamage = true;
       

        _HealthSystem.UpdateLife(attackCollider.Damage);

        cameraShake.ShakeCamera(1, 0.1f); ;
        CurrentSpeed = 0;
        
        hitStop.Stop(attackCollider.HitStop);
        Vector2 direction = (transform.position - attackCollider.gameObject.transform.position).normalized;
       ;
        _rigitbody.AddForce(direction * (attackCollider.Knockback), ForceMode2D.Impulse);
        yield return new WaitForSeconds(staggerTime);
       
        CurrentSpeed = BaseSpeed;
        receivingDamage = false;
    }
    public IEnumerator OnParry()
    {
        hitStop.Stop(0.5f);
        yield return null;
    }
    public IEnumerator OnHitEnemy()
    {
        cameraShake.ShakeCamera(1 * damage, 0.1f * damage);
        StartCoroutine(HealDamage(1));
        yield return null;
    }
    public IEnumerator HealDamage(float Heal)
    {
        _HealthSystem.UpdateLife(-Heal);
        yield return null;
    }

    public void ShowCollider() { WeaponCollider.enabled = true; }
    public void HideCollider() { WeaponCollider.enabled = false; }

    public void RemoveOneCombo()
    {
        if (Player_Animator.GetInteger("ComboCue") < 0)
        {
            Player_Animator.SetInteger("ComboCue", 0);
        }
        if (Player_Animator.GetInteger("ComboCue") > 2)
        {
            Player_Animator.SetInteger("ComboCue", 2);
        }
        if (Player_Animator.GetInteger("ComboCue") > 0)
        {
            Player_Animator.SetInteger("ComboCue", Player_Animator.GetInteger("ComboCue") - 1);
            Attack01();
        }

    }
    public void FinishAll()
    {
        Attacking = false;
        Player_Animator.SetBool("Attack01_Release", false);

    }
    public void CanDash()
    {
        if (Player_Animator.GetInteger("ComboCue") == 0)
        {
            playerRoll.canDash = true;
        }
    }
    public void HideTrail(){ WeaponTrail.enabled = false;}
    public void ShowTrail(){ WeaponTrail.enabled = true;}
    public void HidePlayerCollider() { DamageCollider.enabled = false; }
    public void ShowPlayerCollider() { DamageCollider.enabled = true; }
    public void AddForce(float force)
    {
        
        _rigitbody.AddForce(followMouse.gameObject.transform.up * force);
    }
}

   
