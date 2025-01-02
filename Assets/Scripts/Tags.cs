using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags
{
    public static string MainCharacter = "MainCharacter";
    public static string Player = "Player";
    public static string Player_SinglePointCollider = "Player_SinglePointCollider";

    public static string Player_Hitbox = "Attack_Hitbox";
    public static string Player_Hurtbox = "PlayerDamageCollider";
    public static string Enemy_Hitbox = "Enemy_Attack_hitbox";
    public static string Enemy_Hurtbox = "Enemy_Hurtbox";

    public static string ParryCollider = "ParryCollider";
    
    public static string Enemy = "Enemy";
    public static string Enemy_SinglePointCollider = "Enemy_SinglePointCollider";
    public static string Enemy_notFocus = "Enemy_notFocus";
    public static string IA_Obstacle = "IA_Obstacle";
    
    public static string Weapon_Pivot = "Weapon_Pivot";
    
    public static string StanceBroken = "StanceBroken";
    public static string TargetGroup = "TargetGroup";
    public static string CMvcam1 = "CM vcam1";
    public static string MouseCameraTarget = "MouseCameraTarget";
    public static string Walking = "Walking";
    public static string EnemyParryCollider = "EnemyParryCollider";
    public static string BlockingWalls = "BlockingWalls";
    public static string Object_Hurtbox = "Object_Hurtbox";

    public static string UpgradeContainer = "UpgradeContainer";
    public static string Pickeable = "Pickeable";


    //Enemy
    public static int InAgroo = Animator.StringToHash("inAgroo");
    public static int Attacking = Animator.StringToHash("Attacking");
    public static int HitShield = Animator.StringToHash("HitShield");
    public static int PushBack = Animator.StringToHash("PushBack");
    public static int AttackedWhileRecovering = Animator.StringToHash("AttackedWhileRecovering");
    public static int playerDetected = Animator.StringToHash("playerDetected");
    public static int death = Animator.StringToHash("death");


    //Player
    public static int canTransition = Animator.StringToHash("canTransition");
    public static int isInputing = Animator.StringToHash("inInputing");





}
