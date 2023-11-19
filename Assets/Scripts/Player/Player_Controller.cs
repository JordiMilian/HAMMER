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
    [SerializeField] HitStop hitStop;
    [SerializeField] Generic_Flash player_Flash;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] Collider2D WeaponCollider;
    Player_Roll playerRoll;

    public float CurrentSpeed;
    public float BaseSpeed;
    
    public float CurrentDamage;
    public float BaseDamage;
  

    [SerializeField] float staggerTime = 1;
    bool receivingDamage;

    [SerializeField] AnimationCurve Curve;
        
    void Start()
    {
        Player_Animator = GetComponent<Animator>();
        _HealthSystem = GetComponent<Player_HealthSystem>();
        playerRoll = GetComponent<Player_Roll>();
        _rigitbody = GetComponent<Rigidbody2D>();
        player_Flash = GetComponent<Generic_Flash>();

        CurrentDamage = BaseDamage;
         CurrentSpeed = BaseSpeed;       
      }


    void Update()
    {
         Move();  
       
       
    }
    void Move()
    {
        var input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        _rigitbody.AddForce(input.normalized*CurrentSpeed*Time.deltaTime *100);

        WalkingAnimation();
    }
    void WalkingAnimation()
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
    public void ReceiveDamage(Enemy_AttackCollider attackCollider)
    {
        if(!receivingDamage)
        {
            receivingDamage = true;

             CurrentSpeed = 0;
            _HealthSystem.UpdateLife(attackCollider.Damage);
            cameraShake.ShakeCamera(1, 0.1f); ;
            hitStop.Stop(attackCollider.HitStop);
            player_Flash.CallFlasher();

            Vector2 direction = (transform.position - attackCollider.gameObject.transform.position).normalized;
            _rigitbody.AddForce(direction * (attackCollider.Knockback), ForceMode2D.Impulse);
            StartCoroutine(InvulnerableAfterDamage());

        }
    }
     IEnumerator InvulnerableAfterDamage()
    {

        yield return new WaitForSeconds(staggerTime);
        CurrentSpeed = BaseSpeed;
        receivingDamage = false;
    }
    public void OnParry()
    {
        hitStop.Stop(StopSeconds: 0.5f);
    }
    public void OnHitEnemy()
    {
        cameraShake.ShakeCamera(1 * CurrentDamage, 0.1f * CurrentDamage);
        _HealthSystem.UpdateLife(-1);
        
    }
    public void RestartDamage() { CurrentDamage = BaseDamage; }
    
}

   
