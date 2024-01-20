using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagsCollection : MonoBehaviour
{
    public string Player = "Player";
    public string Player_SinglePointCollider = "Player_SinglePointCollider";
    public string Enemy_Attack_hitbox = "Enemy_Attack_hitbox";
    public string Enemy = "Enemy";
    public string Enemy_Hitbox = "Enemy_Hitbox";
    public string Weapon_Pivot = "Weapon_Pivot";
    public string PlayerDamageCollider = "PlayerDamageCollider";
    public string StanceBroken = "StanceBroken";

    //public static TagsCollection instance;
    public static TagsCollection Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(TagsCollection)) as TagsCollection;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static TagsCollection instance;
}
