using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_invertOvniWhenClose : MonoBehaviour
{
    [SerializeField] Generic_OnTriggerEnterEvents enterEvents;
    [SerializeField] float InvertedStreght;
    [SerializeField] Enemy_EventSystem enemyEvents;
    [SerializeField] CurveToRigidBody enemyOvniMaker;
    [SerializeField] Enemy_AttacksProviderV2 attackProvider;
    float originalStrengh;

    private void OnEnable()
    {
        originalStrengh = enemyOvniMaker.Strengh;
        enterEvents.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        enterEvents.OnTriggerEntered += playerEntered;
        enemyEvents.OnAttackFinished += onAttackOver;
    }
    void playerEntered(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo info)
    {
        if(attackProvider.isAttacking && !attackProvider.currentAttack.notOvniInvertable) //if its attacking and the current atack is invertable
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
