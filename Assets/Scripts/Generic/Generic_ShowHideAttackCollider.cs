using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_ShowHideAttackCollider : MonoBehaviour
{
    [SerializeField] List <Generic_DamageDealer> damageDealer = new List<Generic_DamageDealer>();
    public Generic_EventSystem eventSystem;
    public TrailRenderer trailrendered;
    public TrailRenderer testTrailrendered;
    [HideInInspector] public bool isTesting;
    public  void EV_Enemy_ShowAttackCollider()
    {
        foreach (Generic_DamageDealer dealer in damageDealer)
        {
            dealer.GetComponent<Collider2D>().enabled = true;
        }
        
        if (isTesting) { testTrailrendered.emitting = true; return; }
        if (trailrendered != null) trailrendered.emitting = true;
        eventSystem.OnShowCollider?.Invoke();
    }
    public  void EV_Enemy_HideAttackCollider()
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
