using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Generic_DamageDetector;
using static Player_ParryPerformer;
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
    [SerializeField] Generic_DamageDetector damageDetector;
    Player_Roll playerRoll;
    Player_ParryPerformer parryPerformer;
    

    public InputSystem inputSystem;

    public float CurrentSpeed;
    public float BaseSpeed;

    public float CurrentDamage;
    public float BaseDamage;
  

    [SerializeField] float staggerTime = 1;
    bool receivingDamage;

    [SerializeField] AnimationCurve Curve;

    public event EventHandler OnParriedEnemy;

    private void Awake()
    {
        Player_Animator = GetComponent<Animator>();
        _HealthSystem = GetComponent<Player_HealthSystem>();
        playerRoll = GetComponent<Player_Roll>();
        _rigitbody = GetComponent<Rigidbody2D>();
        player_Flash = GetComponent<Generic_Flash>();
        parryPerformer = GetComponent<Player_ParryPerformer>();

        CurrentDamage = BaseDamage;
        CurrentSpeed = BaseSpeed;
    }
    

    private void OnEnable()
    {
        parryPerformer.OnSuccessfulParry += OnSuccesfulParryCameraEffects;
        damageDetector.OnReceiveDamage += ReceiveDamage;
    }
    private void OnDisable()
    {
        parryPerformer.OnSuccessfulParry -= OnSuccesfulParryCameraEffects;
        damageDetector.OnReceiveDamage -= ReceiveDamage;
    }
    void Update()
    {
        var input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        Move(input);
    }
    void Move(Vector2 vector2)
    {
        //var input = new Vector2(x: Input.GetAxisRaw("Horizontal"), y: Input.GetAxisRaw("Vertical"));
        _rigitbody.AddForce(vector2.normalized*CurrentSpeed*Time.deltaTime *100);

        WalkingAnimation();
    }
    void WalkingAnimation()
    {
        if (!playerRoll.isDashing)
        { 
            if ((Input.GetAxisRaw("Horizontal") != 0) || (Input.GetAxisRaw("Vertical") != 0))
            {
                Player_Animator.SetBool("Walking",true);
            }
            else { Player_Animator.SetBool("Walking", false);
            }
        }
    }
    public void ReceiveDamage(object sender, EventArgs_ReceivedAttackInfo receivedAttackinfo)
    {
        if(!receivingDamage)
        {
            receivingDamage = true;

             CurrentSpeed = 0;
            
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
        CurrentSpeed = BaseSpeed;
        receivingDamage = false;
    }
    public void OnSuccesfulParryCameraEffects(object sender, EventArgs_ParryInfo position)
    {
        hitStop.Stop(StopSeconds: 0.3f);
        cameraShake.ShakeCamera(0.6f, 0.1f);
    }
    public void OnHitEnemyCameraEffects()
    {
        cameraShake.ShakeCamera(1 * CurrentDamage, 0.1f * CurrentDamage);
        //_HealthSystem.RemoveLife(-1);
        
    }
    public void RestartDamage() { CurrentDamage = BaseDamage; }
    
}

   
