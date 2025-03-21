using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_References : Generic_References
{
    [Header("Specific")]
    public Player_EventSystem events;
    public Player_ComboSystem_chargeless comboSystem;
    public Player_FollowMouseWithFocus_V2 followMouse;
    public Player_ProximityDetector proximityDetector;
    public Player_Stamina playerStamina;
    public Player_UpgradesManager upgradesManager;
    public Generic_OnTriggerEnterEvents singlePointCollider;
    public Player_DisableController disableController;
    public Player_LevelStatsManager levelStatsManager;
    public Player_Movement2 movement2;
    public Player_StateMachine stateMachine;
    public PlayerState IdleState, RollingState, ParryingState, SpecialHealState, RunningState, ParriedState, DeadState, DisabledState, RespawningState;
    public PlayerState StartingComboAttackState, RollingAttackState, ParryAttackState, SpecialAttackState; //Each weapon picked should changes these states
    public GameObject WeaponStatesHolder;
    public Transform StatesRoots;
    public TrailRenderer weaponTrail;
    public Player_AnimationEvents animationEvents;
    public Player_WeaponSwitcher weaponSwitcher;

    public Collider2D parryCollider;

    public GameObject weaponScalingRoot;
    public GameObject weaponPivot;

    public GameState gameState;
    [Header("Stats")]
    public PlayerStats currentStats;
    public PlayerStats baseStats;



}
