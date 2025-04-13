using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Idle : EnemyState
{
    [SerializeField] Generic_OnTriggerEnterEvents playerDetectionTrigger;
    IDamageReceiver rootDamageReceiver;
    public override void OnEnable()
    {
        base.OnEnable();
        EnemyState_Agroo agrooState = (EnemyState_Agroo)EnemyRefs.AgrooState;
        agrooState.isPlayerDetected = false;

        EnemyRefs.moveToTarget.SetMovementSpeed(SpeedsEnum.Regular);
        EnemyRefs.moveToTarget.SetRotatinSpeed(SpeedsEnum.Regular);

        EnemyRefs.moveToTarget.DoLook = false;
        EnemyRefs.moveToTarget.DoMove = true;

        playerDetectionTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        playerDetectionTrigger.OnTriggerEntered += OnPlayerEntered;

        rootDamageReceiver = EnemyRefs.gameObject.GetComponent<IDamageReceiver>();
        rootDamageReceiver.OnDamageReceived_event += OnReceivingDamage;
    }
    void OnPlayerEntered(Collider2D collider)
    {
        stateMachine.ChangeState(EnemyRefs.AgrooState);
    }
    void OnReceivingDamage(ReceivedAttackInfo info)
    {
        stateMachine.ChangeState(EnemyRefs.AgrooState);
    }
    public override void OnDisable()
    {
        playerDetectionTrigger.OnTriggerEntered -= OnPlayerEntered;
        rootDamageReceiver.OnDamageReceived_event -= OnReceivingDamage;
    }

}
