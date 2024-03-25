using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static Generic_EventSystem;

public class Enemy01 : MonoBehaviour
{
    
    public Collider2D WeaponCollider;
    [SerializeField] Animator EnemyAnimator;
    public Enemy_AgrooMovement enemyMovement;
    [SerializeField] Generic_Flash flasher;
    [SerializeField] Enemy_EventSystem eventSystem;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] FloatVariable SlowMoPercentage;
    [SerializeField] Enemy_StateMachine stateMachine;

    private void Awake()
    {
    }
    public virtual void OnEnable()
    {
        eventSystem.OnReceiveDamage += ReceiveDamage;
        eventSystem.OnGettingParried += GettingParried;
        eventSystem.OnDeath += slowMoOnDeath;
    }
    public virtual void OnDisable()
    {
        eventSystem.OnReceiveDamage -= ReceiveDamage;
        eventSystem.OnGettingParried -= GettingParried;
    }
    public void ReceiveDamage(object sender, ReceivedAttackInfo receivedAttackinfo)
    {
        if(stateMachine.CurrentState != Enemy_StateMachine.States.Dead)
        {
            flasher.CallFlasher();
            TimeScaleEditor.Instance.HitStop(0.05f);
        }
        
        enemyMovement.EV_SlowRotationSpeed();
        enemyMovement.EV_SlowMovingSpeed();
        //enemyMovement.IsAgroo = true;
        
        EnemyAnimator.SetTrigger(TagsCollection.PushBack);
        StartCoroutine(WaitReceiveDamage());
        Vector2 AttackerDirection = (transform.position - receivedAttackinfo.Attacker.transform.position).normalized;
        StartCoroutine(UsefullMethods.ApplyForceOverTime(_rigidbody, AttackerDirection * receivedAttackinfo.KnockBack ,0.3f));
       
    }  
    IEnumerator WaitReceiveDamage()
    {
        yield return new WaitForSeconds(0.3f);
        enemyMovement.EV_ReturnAllSpeed();
    }
    public void GettingParried()
    {
        EnemyAnimator.SetBool(TagsCollection.HitShield, true);
        WeaponCollider.enabled = false;
    }
    public void EndHitShield()
    {
        EnemyAnimator.SetBool(TagsCollection.HitShield, false);
    }
    void slowMoOnDeath(object sender, Generic_EventSystem.DeadCharacterInfo args)
    {
        TimeScaleEditor.Instance.SlowMotion(SlowMoPercentage.Value, 1f);
    }
}
