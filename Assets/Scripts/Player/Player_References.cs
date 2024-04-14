using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_References : MonoBehaviour
{
    public Player_EventSystem playerEvents;
    public Player_ActionPerformer actionPerformer;
    public Player_ComboSystem_chargeless comboSystem;
    public Player_FeedbackManager feedbackManager;
    public Player_FollowMouse_withFocus followMouse;
    public Player_HealthSystem healthSystem;
    public Player_Movement playerMovement;
    public Player_ParryPerformer parryPerformer;
    public Player_ProximityDetector proximityDetector;
    public Player_Stamina playerStamina;
    public Player_StateMachine playerState;
    public Player_VFXManager playerVFX;
    public Rigidbody2D playerRB;
    public Generic_DamageDealer damageDealer;
    public Generic_DamageDetector damageDetector;
    public Generic_Stats playerStats;
    public Generic_FlipSpriteWithFocus playerSpriteFliper;
    public Generic_Flash playerFlasher;
    public FloatVariable currentHealth;
    public FloatVariable maxHealth;
    public FloatVariable currentStamina;
    public FloatVariable maxStamina;
    public Animator playerAnimator;
    public Collider2D weaponCollider;
    public Collider2D damageDetectorCollider;
    public Collider2D parryCollider;
    public Collider2D positionCollider;
}
