using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMannequin : MonoBehaviour
{
    [SerializeField] bool isAttackingMannequin;
    [SerializeField] Generic_OnTriggerEnterEvents proximityTrigger;
    private void OnEnable()
    {
        if(!isAttackingMannequin) { return; }

        proximityTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        proximityTrigger.OnTriggerEntered += startAttacking;
        proximityTrigger.OnTriggerExited += stopAttacking;
    }
    private void OnDisable()
    {
        if (!isAttackingMannequin) { return; }

        proximityTrigger.OnTriggerEntered -= startAttacking;
        proximityTrigger.OnTriggerExited -= stopAttacking;
    }
    private void startAttacking(Collider2D collision)
    {
        gameObject.GetComponent<Animator>().SetBool("AttackingMannequin", true);
    }
    private void stopAttacking(Collider2D collision)
    {
        gameObject.GetComponent<Animator>().SetBool("AttackingMannequin", false); 
    }
}
