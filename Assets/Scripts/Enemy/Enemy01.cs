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

    private void Awake()
    {
    }
    public virtual void OnEnable()
    {
        eventSystem.OnReceiveDamage += ReceiveDamage;
        eventSystem.OnGettingParried += GettingParried;
    }
    public virtual void OnDisable()
    {
        eventSystem.OnReceiveDamage -= ReceiveDamage;
        eventSystem.OnGettingParried -= GettingParried;
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
    public void GettingParried(object sender, EventArgs args)
    {
        EnemyAnimator.SetBool("HitShield", true);
        WeaponCollider.enabled = false;
    }
    public void EndHitShield()
    {
        EnemyAnimator.SetBool("HitShield", false);
    }
}
