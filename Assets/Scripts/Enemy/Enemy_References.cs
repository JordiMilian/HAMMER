using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_References : Generic_References
{
    [Header("Specific")]
    public Enemy_AttacksProviderV2 attackProvider;
    public Enemy_EventSystem enemyEvents;
    public Enemy_HealthSystem healthSystem;
    public Enemy_MoveToTarget moveToTarget;
    public Enemy_StanceMeter stanceMeter;
    public Enemy_StateMachine stateMachine;
    public Enemy_VFXManager VFXManager;
    public Enemy01 feedbackManager;
    public Enemy_AgrooMovement agrooMovement;
    public Enemy_IdleMovement idleMovement;
    public Collider2D damageCollider;
}
