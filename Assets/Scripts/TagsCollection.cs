using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagsCollection : MonoBehaviour
{
    public string MainCharacter = "MainCharacter";
    public string Player = "Player";
    public string ParryCollider = "ParryCollider";
    public string Player_SinglePointCollider = "Player_SinglePointCollider";
    public string Attack_Hitbox = "Attack_Hitbox";
    public string Enemy_Attack_hitbox = "Enemy_Attack_hitbox";
    public string Enemy = "Enemy";
    public string Enemy_Hitbox = "Enemy_Hitbox";
    public string Weapon_Pivot = "Weapon_Pivot";
    public string PlayerDamageCollider = "PlayerDamageCollider";
    public string StanceBroken = "StanceBroken";
    public string TargetGroup = "TargetGroup";
    public string CMvcam1 = "CM vcam1";
    public string MouseCameraTarget = "MouseCameraTarget";
    public string HitShield = "HitShield";
    public string PushBack = "PushBack";
    public string Walking = "Walking";
    public string EnemyParryCollider = "EnemyParryCollider";

    public static TagsCollection Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
}
