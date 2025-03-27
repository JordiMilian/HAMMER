using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManequin_Parried : State
{
    [SerializeField] string ParriedTriggerString;
    [SerializeField] TutorialManequin_Attacker_Controller controller;
    Coroutine currentCoroutine;
    public override void OnEnable()
    {
        base.OnEnable();
        animator.SetTrigger(ParriedTriggerString);
        controller.EV_HideAttackCollider();
        currentCoroutine = StartCoroutine(returnToAttackingCoroutine());
    }
    IEnumerator returnToAttackingCoroutine()
    {
        yield return null;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        if(currentCoroutine != null) {StopCoroutine(currentCoroutine);}
    }
}
