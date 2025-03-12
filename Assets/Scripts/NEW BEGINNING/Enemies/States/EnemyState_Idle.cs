using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Idle : EnemyState
{
    public override void OnEnable()
    {
        base.OnEnable();
        EnemyState_Agroo agrooState = (EnemyState_Agroo)EnemyRefs.AgrooState;
        agrooState.isPlayerDetected = false;

        EnemyRefs.moveToTarget.SetMovementSpeed(MovementSpeeds.Regular);
        EnemyRefs.moveToTarget.SetRotatinSpeed(MovementSpeeds.Regular);
    }
}
