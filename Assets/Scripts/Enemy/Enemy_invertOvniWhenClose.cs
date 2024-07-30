using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_invertOvniWhenClose : MonoBehaviour
{
    [SerializeField] Enemy_References enemyRefs;
    [SerializeField] Generic_OnTriggerEnterEvents enterEvents;
    [SerializeField] float InvertedStreght;
    [SerializeField] CurveToRigidBody enemyOvniMaker;
    float originalStrengh;

    private void OnEnable()
    {
        originalStrengh = enemyOvniMaker.Strengh;
        enterEvents.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        enterEvents.OnTriggerEntered += playerEntered;
        enemyRefs.enemyEvents.OnAttackFinished += onAttackOver;
    }
    private void OnDisable()
    {
        enterEvents.OnTriggerEntered -= playerEntered;
        enemyRefs.enemyEvents.OnAttackFinished -= onAttackOver;
    }
    void playerEntered(Collider2D collision)
    {
        if(enemyRefs.animator.GetBool("Attacking") && !enemyRefs.attackProvider.currentAttack.notOvniInvertable) //if its attacking and the current atack is invertable
        {
            InvertOvni();
        }
    }
    void onAttackOver()
    {
        returnOvni();
    }
    void InvertOvni()
    {
        enemyOvniMaker.Strengh = InvertedStreght;
        Debug.Log("inverted ovni");
    }
    void returnOvni()
    {
        enemyOvniMaker.Strengh = originalStrengh;
        Debug.Log("return to normal ovni");
    }
}
