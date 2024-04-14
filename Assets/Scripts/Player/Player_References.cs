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
    public Player_FollowMouse_withFocus followMouse;
    public Player_HealthSystem healthSystem;
    public Player_Movement playerMovement;
    public Player_ParryPerformer parryPerformer;
    public Player_ProximityDetector proximityDetector;
    public Player_Stamina playerStamina;
    public Player_StateMachine stateMachine;
    public Player_VFXManager playerVFX;

    public FloatVariable currentHealth;
    public FloatVariable maxHealth;
    public FloatVariable currentStamina;
    public FloatVariable maxStamina;

    public Collider2D parryCollider;
    
}
