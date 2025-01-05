using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ShowHideAttackCollider : Base_ShowHideAttackCollider
{
    [SerializeField] List <Generic_DamageDealer> damageDealer = new List<Generic_DamageDealer>();

    public Enemy_References enemyRefs;
    public TrailRenderer trailrendered;
    public TrailRenderer testTrailrendered;
    [HideInInspector] public bool isTesting;
    private void Awake()
    {
        EV_Enemy_HideAttackCollider();
    }
    private void OnEnable()
    {
        enemyRefs.enemyEvents.OnEnterAgroo += EV_Enemy_HideAttackCollider;
    }
    private void OnDisable()
    {
        enemyRefs.enemyEvents.OnEnterAgroo -= EV_Enemy_HideAttackCollider;
    }
    public override void EV_Enemy_ShowAttackCollider()
    {
        foreach (Generic_DamageDealer dealer in damageDealer)
        {
            dealer.GetComponent<Collider2D>().enabled = true;
        }
        
        if (isTesting) { testTrailrendered.emitting = true; return; }
        if (trailrendered != null) trailrendered.emitting = true;
        enemyRefs.enemyEvents.OnShowCollider?.Invoke();
    }
    public override void EV_Enemy_HideAttackCollider()
    {
        foreach (Generic_DamageDealer dealer in damageDealer)
        {
            dealer.GetComponent<Collider2D>().enabled = false;
        }
        if (isTesting) { testTrailrendered.emitting = false; return; }
        if (trailrendered != null) trailrendered.emitting = false;
    }
    public virtual void HideCollliderOnParry()
    {
        EV_Enemy_HideAttackCollider();
    }
}
