using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_References : Generic_References
{
    [Header("Specific")]
    public Player_EventSystem events;
    public Player_ActionPerformer actionPerformer;
    public Player_ComboSystem_chargeless comboSystem;
    public Player_FeedbackManager feedbackManager;
    public Player_FollowMouseWithFocus_V2 followMouse;
    public Player_HealthSystem healthSystem;
    public Player_Movement playerMovement;
    public Player_ParryPerformer parryPerformer;
    public Player_ProximityDetector proximityDetector;
    public Player_Stamina playerStamina;
    public Player_VFXManager playerVFX;
    public Player_UpgradesManager upgradesManager;
    public Player_SpecialAttack specialAttack;
    public Generic_OnTriggerEnterEvents singlePointCollider;
    public Player_DisableController disableController;
    public Player_LevelStatsManager levelStatsManager;
    public Player_WeaponStatesHolder weaponStatesHolder;
    public Player_Movement2 movement2;
    public PlayerState IdleState, RollingState, ParryingState, SpecialHealState, RunningState;
    public PlayerState StartingComboAttackState, RollingAttackState, ParryAttackState, SpecialAttackState; //Each weapon picked should changes these states

    /*
    public FloatVariable currentHealth;
    public FloatVariable maxHealth;
    public FloatVariable baseHealth;
    public FloatVariable currentStamina;
    public FloatVariable maxStamina;
    public FloatVariable baseStamina;

    */
    public Collider2D parryCollider;

    public GameObject weaponScalingRoot;
    public GameObject weaponPivot;

    public GameState gameState;
    [Header("Stats")]
    public PlayerStats currentStats;
    public PlayerStats baseStats;

}
