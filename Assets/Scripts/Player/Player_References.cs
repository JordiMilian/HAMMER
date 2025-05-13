using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_References : Generic_References
{
    [Header("PLAYER SCRIPTS")]
    public Player_LookableDetector lookablesDetector;
    public Player_Stamina playerStamina;
    public Player_UpgradesManager upgradesManager;
    public Generic_OnTriggerEnterEvents singlePointCollider;
    public Player_LevelStatsManager levelStatsManager;
    public Player_Movement movement;
    public Player_AnimationEvents animationEvents;
    public Player_WeaponSwitcher weaponSwitcher;
    public Player_HideSprites hideSprites;
    public Player_SwordRotationController swordRotation;
    public GesturesDetector gesturesDetector;
    [Header("STATES")]
    public Player_StateMachine stateMachine;
    public PlayerState IdleState, RollingState, ParryingState, SpecialHealState, RunningState, ParriedState, DeadState, DisabledState, RespawningState, EnteringRoomState;
    public PlayerState StartingComboAttackState, RollingAttackState, ParryAttackState, SpecialAttackState; //Each weapon picked should changes these states
    public PlayerState StrongAttack;
    public GameObject WeaponStatesHolder;
    public Transform StatesRoots;
    [Header("OTHERS")]
    public TrailRenderer weaponTrail;


    public Collider2D parryCollider;

    public GameObject weaponScalingRoot;
    public GameObject weaponPivot;

    public GameState gameState;
    [Header("Stats")]
    public PlayerStats currentStats;
    public PlayerStats baseStats;



}
